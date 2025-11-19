using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS_MVC.Models;
using SMS_MVC.Models.Model_Tables;
using System.Formats.Asn1;

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
            ViewBag.GradesList = new SelectList(_Context.Grades.ToList(), "id", "GradeName");
            ViewBag.SectionsList = new SelectList(_Context.Section.ToList(), "id", "SectionName");
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
            ViewBag.GradesList = new SelectList(_Context.Grades.ToList(), "id", "GradeName");
            ViewBag.SectionsList = new SelectList(_Context.Section.ToList(), "id", "SectionName");
            ViewBag.TeachersList = new SelectList(_Context.Teachers.ToList(), "id", "Name");
            ViewBag.SubjectsList = new SelectList(_Context.Subjects.ToList(), "id", "SubjectName");
            ViewBag.AdditionalTeachers = new SelectList(_Context.Teachers.ToList(), "id", "Name");


            return View("PageAddClass");
        }

        [HttpPost]
        public IActionResult OnAddClassClicked(Classes aClass, List<int> SubjectsLst, List<int> ExtraTeachersLst) //string ClassName,int GradeId,int SectionId,int ClassTeacherId,List<int> SubjectsLst, List<int> ExtraTeachersLst)
        {
            if (!ModelState.IsValid || SubjectsLst.Count <= 0 || ExtraTeachersLst.Count <= 0)
            {
                TempData["AlertMessage"] = "Please Fill all Fields of the Form";
                return RedirectToAction("PageAddClass");
            }
            _Context.Classes.Add(aClass);
            _Context.SaveChanges();

            foreach (var subjectId in SubjectsLst)
            {
                ClassAndSubject classAndSubjects = new ClassAndSubject();
                classAndSubjects.SubjectId = subjectId;
                classAndSubjects.ClassId = aClass.id;
                _Context.ClassAndSubject.Add(classAndSubjects);
            }
            _Context.SaveChanges();

            foreach (var teacherId in ExtraTeachersLst)
            {
                TeacherAndClass teachersAndClass = new TeacherAndClass();
                teachersAndClass.TeacherId = teacherId;
                teachersAndClass.ClassId = aClass.id;
                _Context.TeacherAndClass.Add(teachersAndClass);
            }
            _Context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult PageEditClass(int id)
        {
            var classDetails = _Context.Classes.FirstOrDefault(c => c.id == id);
            if (classDetails == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.GradesList = new SelectList(_Context.Grades.ToList(), "id", "GradeName", classDetails.GradeId);
            ViewBag.SectionsList = new SelectList(_Context.Section.ToList(), "id", "SectionName", classDetails.SectionId);
            ViewBag.TeachersList = new SelectList(_Context.Teachers.ToList(), "id", "Name", classDetails.ClassTeacherId);
            ViewBag.SubjectsList = new SelectList(_Context.Subjects.ToList(), "id", "SubjectName");
            ViewBag.AdditionalTeachers = new SelectList(_Context.Teachers.ToList(), "id", "Name");
            ViewBag.SelectedSubjects = _Context.ClassAndSubject.Where(cs => cs.ClassId == id).Select(cs => cs.SubjectId).ToList();
            ViewBag.SelectedAdditionalTeachers = _Context.TeacherAndClass.Where(tc => tc.ClassId == id).Select(tc => tc.TeacherId).ToList();
            return View("PageEditClass", classDetails);
        }

        [HttpPost]
        public IActionResult OnUpdateClassClicked(Classes updatedClass, List<int> SubjectsLst, List<int> ExtraTeachersLst)
        {
            if (!ModelState.IsValid || SubjectsLst.Count <= 0 || ExtraTeachersLst.Count <= 0)
            {
                TempData["AlertMessage"] = "Please Fill all Fields of the Form";
                return RedirectToAction("PageEditClass", updatedClass.id);
            }
            var existingClass = _Context.Classes.FirstOrDefault(c => c.id == updatedClass.id);
            if (existingClass == null)
            {
                return RedirectToAction("Index");
            }
            existingClass.ClassName = updatedClass.ClassName;
            existingClass.GradeId = updatedClass.GradeId;
            existingClass.SectionId = updatedClass.SectionId;
            existingClass.ClassTeacherId = updatedClass.ClassTeacherId;
            _Context.SaveChanges();
            var existingSubjects = _Context.ClassAndSubject.Where(cs => cs.ClassId == updatedClass.id).ToList();
            _Context.ClassAndSubject.RemoveRange(existingSubjects);
            _Context.SaveChanges();
            foreach (var subjectId in SubjectsLst)
            {
                ClassAndSubject classAndSubjects = new ClassAndSubject();
                classAndSubjects.SubjectId = subjectId;
                classAndSubjects.ClassId = updatedClass.id;
                _Context.ClassAndSubject.Add(classAndSubjects);
            }
            _Context.SaveChanges();
            var existingTeachers = _Context.TeacherAndClass.Where(tc => tc.ClassId == updatedClass.id).ToList();
            _Context.TeacherAndClass.RemoveRange(existingTeachers);
            _Context.SaveChanges();
            foreach (var teacherId in ExtraTeachersLst)
            {
                TeacherAndClass teachersAndClass = new TeacherAndClass();
                teachersAndClass.TeacherId = teacherId;
                teachersAndClass.ClassId = updatedClass.id;
                _Context.TeacherAndClass.Add(teachersAndClass);
            }
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
