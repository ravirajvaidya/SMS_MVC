using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS_MVC.Models;
using SMS_MVC.Models.Model_Tables;

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

        /// <summary>
        /// Add Student Page Implemented
        /// </summary>
        /// <returns></returns>

        public IActionResult PageAddStudent()
        {
            ViewBag.UserList = new SelectList(_Context.Users.Where(x=>x.RoleId == 6).ToList(), "id", "UserName");
            ViewBag.Classes = new SelectList(_Context.Classes.ToList(), "id", "ClassName");
            return View("PageAddStudent",new Students());
        }

        [HttpPost]
        public IActionResult AddStudent(Students aStudent)
        {
            if (ModelState.IsValid)
            {
                _Context.Students.Add(aStudent);
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult PageEditStudent()
        {
            return View();
        }
    }
}
