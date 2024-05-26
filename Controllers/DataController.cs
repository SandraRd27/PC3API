using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PC3API.Integration.API;
using PC3API.Integration.data;

namespace PC3API.Controllers
{
    public class DataController : Controller
    {
        private readonly ILogger<DataController> _logger;
        private readonly APIIntegration _integracion;

        public DataController(ILogger<DataController> logger, APIIntegration integracion)
        {
            _logger = logger;
            _integracion = integracion;
        }

        public async Task<IActionResult> Index()
        {
            List<Datos> datos = await _integracion.Lista();

            // logica para filtrar data del servicio
            List<Datos> lista = datos.ToList();
            return View(lista);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        
        public async Task<IActionResult> Info(int id)
        {
            List<Datos> datos = await _integracion.Lista();            
            List<Datos> lista = datos.Where(todo => todo.id == id)
            .OrderBy(todo => todo.id).ToList();            
            return View(lista);
        }
        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Datos dato)
        {

            await _integracion.AgregarUsu(dato);
            return  RedirectToAction("Index"); 
        }
    }
}