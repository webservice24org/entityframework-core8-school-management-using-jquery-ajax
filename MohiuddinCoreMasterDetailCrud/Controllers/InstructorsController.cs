using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MohiuddinCoreMasterDetailCrud.Models;
using MohiuddinCoreMasterDetailCrud.Models.ViewModels;

namespace MohiuddinCoreMasterDetailCrud.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly MohiuddinCoreMasterDetailsContext _context;
        private readonly IWebHostEnvironment _webHost;

        public InstructorsController(MohiuddinCoreMasterDetailsContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllCourses()
        {
            var courses = _context.Courses
                .Include(c => c.Department) 
                .Select(c => new {
                    c.CourseId,
                    c.CourseName
                })
                .ToList();

            return Json(courses);
        }

    }
}
