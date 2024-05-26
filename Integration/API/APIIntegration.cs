using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PC3API.Integration.data;

namespace PC3API.Integration.API
{
    public class APIIntegration
    {
        private readonly ILogger<APIIntegration> _logger;
        private const string Direccion = "https://reqres.in/api/users";

        public APIIntegration(ILogger<APIIntegration> logger)
        {
            _logger = logger;
        }

public async Task<List<Datos>> Lista()
{
    string requestUrl = $"{Direccion}";

    using (var client = new HttpClient())
    {
        try
        {
            var respuesta = await client.GetAsync(requestUrl);
            respuesta.EnsureSuccessStatusCode();

            var json = await respuesta.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(json).RootElement;

            if (jsonObject.TryGetProperty("data", out var dataArray))
            {
                var dataList = new List<Datos>();

                foreach (var dataItem in dataArray.EnumerateArray())
                {
                    var datos = new Datos
                    {
                        id = dataItem.GetProperty("id").GetInt32(),
                        email = dataItem.GetProperty("email").GetString(),
                        first_name = dataItem.GetProperty("first_name").GetString(),
                        last_name = dataItem.GetProperty("last_name").GetString(),
                        avatar = dataItem.GetProperty("avatar").GetString()
                    };

                    dataList.Add(datos);
                }

                return dataList;
            }
            else
            {
                _logger.LogError("La propiedad 'data' no está presente en la respuesta JSON.");
                return new List<Datos>();
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError($"Error al procesar la respuesta JSON: {ex.Message}");
            throw; // Re-lanzar la excepción para que se propague
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error al llamar a la API: {ex.Message}");
            throw; // Re-lanzar la excepción para que se propague
        }
    }
}
            

          public async Task AgregarUsu(Datos newUser)
{


    using (HttpClient client = new HttpClient())
    {


            var jsonContent = JsonSerializer.Serialize(newUser);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage respuesta = await client.PostAsync(Direccion, content);
            respuesta.EnsureSuccessStatusCode();
            _logger.LogInformation($"{respuesta}");
    }

}
    }
}