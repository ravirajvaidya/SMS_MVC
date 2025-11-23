using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS_MVC.Models;

namespace SMS_MVC.Controllers
{
    public class StudentsController : Controller
    {
        private SMSDBContext _Context;

        public StudentsController(SMSDBContext context)
        {
            _Context = context;
        }

        public IActionResult Index(string search = "", string gradeFilter = "All", string sectionFilter = "All")
        {
            ViewBag.Active = "Students";
            ViewBag.PageTitle = "Students Dashboard";
            ViewBag.TotalClasses = _Context.Students.Count();
            ViewBag.Search = search;
            ViewBag.SelectedGrade = gradeFilter;
            ViewBag.SelectedSection = sectionFilter;
            ViewBag.GradesList = new SelectList(_Context.Grades.ToList(), "id", "GradeName");
            ViewBag.SectionsList = new SelectList(_Context.Section.ToList(), "id", "SectionName");
            return View(_Context.Students.ToList());
        }

        [HttpGet]
        public IActionResult PageAddStudent()
        {
            return View();
        }
    }
}
