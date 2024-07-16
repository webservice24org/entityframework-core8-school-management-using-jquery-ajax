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
                    m.CourseId,
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

        [HttpGet]
        public async Task<IActionResult> GetModulesByCourseId(int courseId)
        {
            var modules = await _context.Modules.Where(m => m.CourseId == courseId).ToListAsync();
            return Json(modules);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateModules([FromBody] ModuleViewModel model)
        {
            try
            {
                var existingModules = await _context.Modules
                    .Where(m => m.CourseId == model.CourseId)
                    .ToListAsync();

                
                foreach (var existingModule in existingModules.ToList())
                {
                    if (!model.Modules.Any(m => m.ModuleId == existingModule.ModuleId))
                    {
                        _context.Modules.Remove(existingModule);
                    }
                }

                
                foreach (var moduleViewModel in model.Modules)
                {
                    if (!string.IsNullOrWhiteSpace(moduleViewModel.ModuleName) && moduleViewModel.Duration > 0)
                    {
                        if (int.TryParse(moduleViewModel.ModuleId.ToString(), out int moduleId))
                        {
                            var existingModule = existingModules.FirstOrDefault(m => m.ModuleId == moduleId);
                            if (existingModule != null)
                            {
                                existingModule.ModuleName = moduleViewModel.ModuleName;
                                existingModule.Duration = moduleViewModel.Duration;
                            }
                            else
                            {
                                _context.Modules.Add(new Module
                                {
                                    ModuleName = moduleViewModel.ModuleName,
                                    Duration = moduleViewModel.Duration,
                                    CourseId = model.CourseId
                                });
                            }
                        }
                        else
                        {
                            return Json(new { success = false, message = $"Invalid ModuleId: {moduleViewModel.ModuleId}" });
                        }
                    }
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Modules updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error updating modules: {ex.Message}" });
            }
        }


        [HttpGet]
        public JsonResult GetCourses()
        {
            List<SelectListItem> courses = _context.Courses
                .Select(cor => new SelectListItem
                {
                    Value = cor.CourseId.ToString(),
                    Text = cor.CourseName
                })
                .ToList();
            return Json(courses);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllModules(int courseId)
        {
            var modules = _context.Modules.Where(m => m.CourseId == courseId).ToList();
            if (modules.Any())
            {
                _context.Modules.RemoveRange(modules);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "No modules found for the given course." });
        }


    }
}
