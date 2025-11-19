using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS_MVC.Models;
using SMS_MVC.Models.Model_Tables;

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

        /// <summary>
        /// Add Class Section
        /// </summary>
        /// <returns></returns>
        public IActionResult PageAddClass()
        {
            ViewBag.GradesList = new SelectList(_Context.Grades.ToList(),"id", "GradeName");
            ViewBag.SectionsList = new SelectList(_Context.Section.ToList(),"id", "SectionName");
            ViewBag.TeachersList = new SelectList(_Context.Teachers.ToList(), "id", "Name");
            ViewBag.SubjectsList = new SelectList(_Context.Subjects.ToList(), "id", "SubjectName");
            ViewBag.AdditionalTeachers = new SelectList(_Context.Teachers.ToList(), "id", "Name");


            return View("PageAddClass");
        }

        [HttpPost]
        public IActionResult OnAddClassClicked(Classes aClass, List<int> SubjectsLst, List<int> ExtraTeachersLst) //string ClassName,int GradeId,int SectionId,int ClassTeacherId,List<int> SubjectsLst, List<int> ExtraTeachersLst)
        {
            if(!ModelState.IsValid)
            {
                return View("PageAddClass", aClass);
            }
            _Context.Classes.Add(aClass);
            _Context.SaveChanges();

            TeacherAndClass teachersAndClass = new TeacherAndClass();
            foreach (var teacherId in ExtraTeachersLst)
            {
                teachersAndClass.TeacherId = teacherId;
                teachersAndClass.ClassId = aClass.id;
                _Context.TeacherAndClass.Add(teachersAndClass);
            }
            _Context.SaveChanges();

            ClassAndSubject classAndSubjects = new ClassAndSubject();
            foreach (var subjectId in SubjectsLst)
            {
                classAndSubjects.SubjectId = subjectId;
                classAndSubjects.ClassId = aClass.id;
                _Context.ClassAndSubject.Add(classAndSubjects);
            }
            _Context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
