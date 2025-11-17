using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS_MVC.Models;
using SMS_MVC.Models.Model_Tables;

namespace SMS_MVC.Controllers
{
    public class TeachersController : Controller
    {
        private SMSDBContext _Context;

        public TeachersController(SMSDBContext context)
        {
            _Context = context;
        }

        public IActionResult Index(string filterSubject = "All")
        {
            ViewBag.Active = "Teachers";
            ViewBag.PageTitle = "Teachers Dashboard";
            ViewBag.SelectedSubject = filterSubject;

            List<string> lstOfSubs = new List<string>();
            lstOfSubs = _Context.Subjects.Select(s => s.SubjectName).Distinct().ToList();

            ViewBag.SubjectList = lstOfSubs;
            ViewBag.TotalTeachers = _Context.Teachers.Count();
            ViewBag.TeacherList = (from t in _Context.Teachers
                                   join u in _Context.Users on t.UserId equals u.id
                                   join s in _Context.Subjects on t.SubjectId equals s.id
                                   select new
                                   {
                                       t.id,
                                       t.Name,
                                       t.Email,
                                       UserName = u.UserName,
                                       SubjectName = s.SubjectName,
                                       t.Qualification,
                                       t.Experience,
                                       JoiningDate = t.JoiningDate.ToShortDateString()
                                   }).ToList();

            return View();
        }

        /// <summary>
        /// Add new Teacher Page
        /// </summary>
        /// <returns></returns>
        public IActionResult PageAddTeacher()
        {
            ViewBag.UserList = new SelectList(_Context.Users.Where(u => u.RoleId == 2).ToList(), "id", "UserName");
            ViewBag.SubjectList = new SelectList(_Context.Subjects.ToList(), "id", "SubjectName");
            return View("PageAddTeacher");
        }

        [HttpPost]
        public IActionResult OnAddTeacherClicked(Teachers teacher)
        {
            if (ModelState.IsValid)
            {
                _Context.Teachers.Add(teacher);
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserList = new SelectList(_Context.Users.Where(u => u.RoleId == 2).ToList(), "id", "UserName");
            ViewBag.SubjectList = new SelectList(_Context.Subjects.ToList(), "id", "SubjectName");
            return View("PageAddTeacher", teacher);
        }

        
    }
}
