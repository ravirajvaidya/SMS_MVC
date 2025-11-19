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

        public IActionResult Index(string search = "", string subjectFilter = "All")
        {
            ViewBag.Active = "Teachers";
            ViewBag.PageTitle = "Teachers Dashboard";
            ViewBag.SelectedSubject = subjectFilter;
            ViewBag.Search = search;
            ViewBag.SubjectList = new SelectList(_Context.Subjects.ToList(), "id", "SubjectName");
            ViewBag.TotalTeachers = _Context.Teachers.Count();

            var lstTeachers = from t in _Context.Teachers
                        join u in _Context.Users on t.UserId equals u.id
                        join s in _Context.Subjects on t.SubjectId equals s.id
                        select new
                        {
                            t.id,
                            t.Name,
                            t.Email,
                            t.Qualification,
                            t.Experience,
                            JoiningDate = t.JoiningDate.ToShortDateString(),
                            UserName = u.UserName,
                            SubjectName = s.SubjectName,
                            SubjectId = s.id
                        };

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                lstTeachers = lstTeachers.Where(x => x.Name.Contains(search));
            }

            // Apply subject filter ONLY if not "All"
            if (!string.IsNullOrEmpty(subjectFilter) && subjectFilter != "All")
            {
                lstTeachers = lstTeachers.Where(x => x.SubjectId == Convert.ToInt32(subjectFilter));
            }

            ViewBag.TeacherList = lstTeachers.ToList();

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
            return RedirectToAction("PageAddTeacher");
        }

        /// <summary>
        /// Edit Teacher Page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PageEditTeacher(int id)
        {
            Teachers teacher = _Context.Teachers.FirstOrDefault(x =>x.id == id);
            ViewBag.UserList = new SelectList(_Context.Users.Where(u => u.RoleId == 2).ToList(), "id", "UserName", teacher.UserId);
            ViewBag.SubjectList = new SelectList(_Context.Subjects.ToList(), "id", "SubjectName", teacher.SubjectId);
            return View(teacher);
        }

        public IActionResult OnUpdateTeacherClicked(Teachers teacher)
        {
            if (ModelState.IsValid)
            {
                _Context.Teachers.Update(teacher);
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }            
            return RedirectToAction("PageAddTeacher", teacher.id);

        }
    }
}
