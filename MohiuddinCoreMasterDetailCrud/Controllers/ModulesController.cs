using Microsoft.AspNetCore.Mvc;
using MohiuddinCoreMasterDetailCrud.Models;
using MohiuddinCoreMasterDetailCrud.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MohiuddinCoreMasterDetailCrud.Controllers
{
    public class ModulesController : Controller
    {
        private readonly MohiuddinCoreMasterDetailsContext _context;

        public ModulesController(MohiuddinCoreMasterDetailsContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var viewModel = new ModuleViewModel
            {
                Modules = new List<ModuleViewModel>()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllModules()
        {
            var modules = await _context.Modules
                .Include(m => m.Course)
                .Select(m => new
                {
                    m.ModuleId,
                    m.ModuleName,
                    m.Duration,
                    CourseName = m.Course.CourseName
                })
                .ToListAsync();

            return Json(modules);
        }


        [HttpPost]
        public async Task<JsonResult> InsertModules([FromBody] ModuleViewModel model)
        {
            
            var modules = model.Modules.Select(m => new Module
            {
                ModuleName = m.ModuleName,
                Duration = m.Duration,
                CourseId = model.CourseId
            }).ToList();

            _context.Modules.AddRange(modules);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Modules inserted successfully." });
            

        }


        public JsonResult GetCourses()
        {
            List<SelectListItem> course = (from cor in _context.Courses
                                           select new SelectListItem
                                           {
                                               Value = cor.CourseId.ToString(),
                                               Text = cor.CourseName
                                           }).ToList();
            return Json(course);
        }

    }
}
