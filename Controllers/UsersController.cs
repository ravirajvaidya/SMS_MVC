using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS_MVC.Models;
using SMS_MVC.Models.Model_Tables;

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

            if (tab == "All")
            {
                userDashboardModel._Users = _Context.Users.ToList();
            }
            else
            {
                userDashboardModel._Users = (from admins in _Context.Users
                                             join roles in _Context.Roles
                                             on admins.RoleId equals roles.id
                                             where roles.RoleType == tab.Remove(tab.Length - 1)
                                             select admins).ToList();

            }

            userDashboardModel.TotalUsers = _Context.Users.ToList().Count();

            var roleCounts = _Context.Users
                .Join(_Context.Roles,
                      u => u.RoleId,
                      r => r.id,
                      (u, r) => new { r.RoleType })
                .GroupBy(x => x.RoleType)
                .Select(g => new { RoleType = g.Key, Count = g.Count() })
                .ToDictionary(x => x.RoleType, x => x.Count);

            userDashboardModel.Admins = roleCounts.GetValueOrDefault("Admin", 0);
            userDashboardModel.Teachers = roleCounts.GetValueOrDefault("Teacher", 0);
            userDashboardModel.Parents = roleCounts.GetValueOrDefault("Parent", 0);
            userDashboardModel.Accountants = roleCounts.GetValueOrDefault("Accountant", 0);
            userDashboardModel.Librarians = roleCounts.GetValueOrDefault("Librarian", 0);
            userDashboardModel.Students = roleCounts.GetValueOrDefault("Student", 0);
            userDashboardModel.Staffs = roleCounts.GetValueOrDefault("Staff", 0);

            return View(userDashboardModel);
        }

        /// <summary>
        /// This is the Add User Part
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public IActionResult PageAddUser()
        {
            ViewBag.Roles = new SelectList(_Context.Roles.ToList(), "id", "RoleType");
            return View("PageAddUser");
        }

        public IActionResult OnAddUserClicked(Users aUser)
        {
            _Context.Users.Add(aUser);
            _Context.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// This is the Edit User Part
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult PageEditUser(int userid)
        {
            Users aUser = _Context.Users.Where(u => u.id == userid).FirstOrDefault();
            ViewBag.Roles = new SelectList(_Context.Roles.ToList(), "id", "RoleType", aUser.RoleId);

            if (aUser != null)
            {
                return View("PageEditUser", aUser);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult OnUpdateUserClicked(Users aUser)
        {
            _Context.Users.Update(aUser);
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
