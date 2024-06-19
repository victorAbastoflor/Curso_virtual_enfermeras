using Microsoft.AspNetCore.Mvc;
using NurseCourse.Models;

using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

using System;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using NewtonsoftJson = Newtonsoft.Json;
using NurseCourse.Controllers.Service.Certificate;
using QuestPDF.Previewer;
using QuestPDF.Fluent;
using NurseCourse.Services;

namespace NurseCourse.Controllers;

public class NurseController : Controller
{
    private readonly HttpClient _httpClient;

    public NurseController()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5053/api/")
        };
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RegistroCurso()
    {
        return View();
    }

    public IActionResult RegistroUser()
    {
        return View();
    }

    public IActionResult Calificaciones()
    {
        return View();
    }

    public async Task<IActionResult> DetallesModulo()
    {
        List<gUsuario> users = new List<gUsuario>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/usuarios"))
            {
                //Console.WriteLine(response+"-- response --"+response.Content.ReadAsStringAsync()+"-- response.Contentaaaa --"+response.Equals("value"));
                string apiResponse = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<gUsuario>>(apiResponse);
            }
        }

        return View(users);
    }

    public async Task<IActionResult> ListaCursos()
    {
        List<gCurso> cursos = new List<gCurso>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/Cursos"))
            {
                //Console.WriteLine(response+"-- response --"+response.Content.ReadAsStringAsync()+"-- response.Contentaaaa --"+response.Equals("value"));
                string apiResponse = await response.Content.ReadAsStringAsync();
                cursos = JsonConvert.DeserializeObject<List<gCurso>>(apiResponse);
            }
        }

        return View(cursos);
    }

    public async Task<IActionResult> ListaUsers()
    {
        List<gUsuario> users = new List<gUsuario>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/usuarios"))
            {
                //Console.WriteLine(response+"-- response --"+response.Content.ReadAsStringAsync()+"-- response.Contentaaaa --"+response.Equals("value"));
                string apiResponse = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<gUsuario>>(apiResponse);
            }
        }

        return View(users);
    }

    // GET: Movies/Edit/5
    public async Task<IActionResult> DetallesCurso(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        ViewBag.CourseId = id;

        List<gModulo> modulos = new List<gModulo>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/Modulos/Curso/"+id))
            {
                if(response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    modulos = JsonConvert.DeserializeObject<List<gModulo>>(apiResponse);
                }else{
                    modulos = null;
                }
            }
        }
        List<gExamenes> examenes = new List<gExamenes>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/Examenes/Curso/"+id))
            {
                if(response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    examenes = JsonConvert.DeserializeObject<List<gExamenes>>(apiResponse);
                }else{
                    examenes = null;
                }
            }
        }
        List<gUsuario> personas = new List<gUsuario>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/usuarios"))
            {
                if(response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    personas = JsonConvert.DeserializeObject<List<gUsuario>>(apiResponse);
                }else{
                    personas = null;
                }
            }
        }       
        List<gUsuario> score = new List<gUsuario>();

        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/usuarios"))
            {
                if(response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    score = JsonConvert.DeserializeObject<List<gUsuario>>(apiResponse);

                    // Iterar sobre cada usuario para obtener sus notas de exámenes
                    foreach(var usuario in score)
                    {
                        Console.WriteLine($"Usuario ID: {usuario.usuarioId}, Nombre: {usuario.nombre}");

                        // Iterar sobre las notas de exámenes del usuario
                        foreach(var notaExamen in usuario.notasExamenes)
                        {
                            Console.WriteLine($"Nota del Examen - ID: {notaExamen.notaExamenId}, Calificación: {notaExamen.calificacion}");
                        }
                    }
                }
                else
                {
                    score = null;
                }
            }
        }
        
        ViewData["id"] = id;
        ViewData["Link1"] = examenes[0].linkExame;
        ViewData["Link2"] = examenes[1].linkExame;
        ViewData["NumTimes"] = examenes.Count;

        // Pasar las listas al modelo de la vista
        var model = Tuple.Create(modulos, personas);
        return View(model);
    }
    
    public async Task<IActionResult> Certificar(string userName, string email, int courseId)
    {
        var httpClient = new HttpClient();
        var course = await httpClient.GetAsync($"http://localhost:5053/api/Cursos/{courseId}");
        string data = await course.Content.ReadAsStringAsync();
        var curso = JsonConvert.DeserializeObject<Curso>(data)!;

        Certificate certificate = new(curso.Titulo, userName);
        SmtpServer smtpServer = new();

        using MemoryStream memoryStream = new();
        certificate.CreateDocument().GeneratePdf(memoryStream); 
        memoryStream.Position = 0;
        smtpServer.SendEmail(email, memoryStream);
        return RedirectToAction("DetallesCurso", "Nurse", new {id = courseId});
    }

    public async Task<IActionResult> ListaModulos()
    {
        List<gCurso> cursos = new List<gCurso>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/Cursos"))
            {
                //Console.WriteLine(response+"-- response --"+response.Content.ReadAsStringAsync()+"-- response.Contentaaaa --"+response.Equals("value"));
                string apiResponse = await response.Content.ReadAsStringAsync();
                cursos = JsonConvert.DeserializeObject<List<gCurso>>(apiResponse);
            }
        }

        return View(cursos);
    }

    public async Task<IActionResult> RegistroModulo()
    {
        List<gCurso> cursos = new List<gCurso>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/Cursos"))
            {
                //Console.WriteLine(response+"-- response --"+response.Content.ReadAsStringAsync()+"-- response.Contentaaaa --"+response.Equals("value"));
                string apiResponse = await response.Content.ReadAsStringAsync();
                cursos = JsonConvert.DeserializeObject<List<gCurso>>(apiResponse);
            }
        }

        return View(cursos);
    }
}