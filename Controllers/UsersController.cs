using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS_MVC.Models;

namespace SMS_MVC.Controllers
{
    public class UsersController : Controller
    {
        private SMSDBContext _Context;

        public UsersController(SMSDBContext context)
        {
            _Context = context;
        }
        public IActionResult Index(string tab = "All")
        {
            ViewBag.Active = "Users";
            ViewBag.PageTitle = "Users Dashboard";
            UserDashboardModel userDashboardModel = new UserDashboardModel();
            userDashboardModel.ActiveTab = tab;
            // Filter based on tab
            switch (tab)
            {
                case "Admins":
                    userDashboardModel._Users = (from admins in _Context.Users
                                                 join roles in _Context.Roles
                                                 on admins.RoleId equals roles.id
                                                 where roles.RoleType == "Admin"
                                                 select admins).ToList();
                    break;

                case "Teachers":
                    userDashboardModel._Users = (from admins in _Context.Users
                                                 join roles in _Context.Roles
                                                 on admins.RoleId equals roles.id
                                                 where roles.RoleType == "Teacher"
                                                 select admins).ToList();
                    break;

                case "Parents":
                    userDashboardModel._Users = (from admins in _Context.Users
                                                 join roles in _Context.Roles
                                                 on admins.RoleId equals roles.id
                                                 where roles.RoleType == "Parent"
                                                 select admins).ToList();
                    break;

                case "Accountants":
                    userDashboardModel._Users = (from admins in _Context.Users
                                                 join roles in _Context.Roles
                                                 on admins.RoleId equals roles.id
                                                 where roles.RoleType == "Accountant"
                                                 select admins).ToList();
                    break;

                case "Librarians":
                    userDashboardModel._Users = (from admins in _Context.Users
                                                 join roles in _Context.Roles
                                                 on admins.RoleId equals roles.id
                                                 where roles.RoleType == "Librarian"
                                                 select admins).ToList();
                    break;
                case "Students":
                    userDashboardModel._Users = (from admins in _Context.Users
                                                 join roles in _Context.Roles
                                                 on admins.RoleId equals roles.id
                                                 where roles.RoleType == "Student"
                                                 select admins).ToList();
                    break;
                case "Staffs":
                    userDashboardModel._Users = (from admins in _Context.Users
                                                 join roles in _Context.Roles
                                                 on admins.RoleId equals roles.id
                                                 where roles.RoleType == "Staff"
                                                 select admins).ToList();
                    break;
                case "All":
                default:
                    userDashboardModel._Users = _Context.Users.ToList();
                    break;
            }


            userDashboardModel.TotalUsers = _Context.Users.ToList().Count();

            userDashboardModel.Admins = (from admins in _Context.Users
                                         join roles in _Context.Roles
                                         on admins.RoleId equals roles.id
                                         where roles.RoleType == "Admin"
                                         select admins).ToList().Count;

            userDashboardModel.Teachers = (from teachers in _Context.Users
                                           join roles in _Context.Roles
                                           on teachers.RoleId equals roles.id
                                           where roles.RoleType == "Teacher"
                                           select teachers).ToList().Count;

            userDashboardModel.Parents = (from parents in _Context.Users
                                          join roles in _Context.Roles
                                          on parents.RoleId equals roles.id
                                          where roles.RoleType == "Parent"
                                          select parents).ToList().Count;

            userDashboardModel.Accountants = (from accountants in _Context.Users
                                              join roles in _Context.Roles
                                              on accountants.RoleId equals roles.id
                                              where roles.RoleType == "Accountant"
                                              select accountants).ToList().Count;

            userDashboardModel.Librarians = (from accountants in _Context.Users
                                             join roles in _Context.Roles
                                             on accountants.RoleId equals roles.id
                                             where roles.RoleType == "Librarian"
                                             select accountants).ToList().Count;

            userDashboardModel.Students = (from students in _Context.Users
                                            join roles in _Context.Roles
                                            on students.RoleId equals roles.id
                                            where roles.RoleType == "Student"
                                            select students).ToList().Count;

            userDashboardModel.Staffs = (from students in _Context.Users
                                           join roles in _Context.Roles
                                           on students.RoleId equals roles.id
                                           where roles.RoleType == "Staff"
                                           select students).ToList().Count;
            
            return View(userDashboardModel);
        }

        [HttpPost]
        public IActionResult PageAddUser()
        {
            ViewBag.Roles = new SelectList(_Context.Roles.ToList(), "id", "RoleType");
            return View("PageAddUser");
        }

        public IActionResult OnCancleClicked()
        {
            return RedirectToAction("Index");
        }
        public IActionResult OnAddUserClicked(Users aUser)
        {
            _Context.Users.Add(aUser);
            _Context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
