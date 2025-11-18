using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS_MVC.Models;

namespace SMS_MVC.Controllers
{
    public class ClassesController : Controller
    {
        private SMSDBContext _Context;

        public ClassesController(SMSDBContext context)
        {
            _Context = context;
        }

        public IActionResult Index(string search = "", string gradeFilter = "All", string sectionFilter = "All")
        {
            ViewBag.Active = "Classes";
            ViewBag.PageTitle = "Classes Dashboard";
            ViewBag.TotalClasses = _Context.Classes.Count();
            ViewBag.Search = search;
            ViewBag.SelectedGrade = gradeFilter;
            ViewBag.SelectedSection = sectionFilter;
            ViewBag.GradesList = _Context.Grades.Select(g => g.GradeName).Distinct().ToList();
            ViewBag.SectionsList = _Context.Section.Select(s => s.SectionName).Distinct().ToList();

            var lstClasses = (from c in _Context.Classes
                                   join t in _Context.Teachers on c.ClassTeacherId equals t.id
                                   join g in _Context.Grades on c.GradeId equals g.id
                                   join s in _Context.Section on c.SectionId equals s.id
                                   select new
                                   {
                                       c.id,
                                       c.ClassName,
                                       ClassTeacherName = t.Name,
                                       GradeName = g.GradeName,
                                       SectionName = s.SectionName
                                   }).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                lstClasses = lstClasses.Where(x => x.ClassName.Contains(search)).ToList();
            }

            if (!string.IsNullOrEmpty(gradeFilter) && gradeFilter != "All")
            {
                lstClasses = lstClasses.Where(x => x.GradeName == gradeFilter).ToList();
            }

            if (!string.IsNullOrEmpty(sectionFilter) && sectionFilter != "All")
            {
                lstClasses = lstClasses.Where(x => x.SectionName == sectionFilter).ToList();
            }

            ViewBag.ClassesList = lstClasses;

            return View();
        }

        public IActionResult PageAddClass()
        {
            ViewBag.ClassTeacherList = new SelectList(_Context.Teachers.ToList(), "id", "Name");
            ViewBag.GradesList = new SelectList(_Context.Grades.ToList(), "id", "GradeName");
            ViewBag.SectionsList = new SelectList(_Context.Section.ToList(), "id", "SectionName");
            return View("PageAddClass");
        }
    }
}
