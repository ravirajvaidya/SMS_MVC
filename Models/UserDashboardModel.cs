using SMS_MVC.Models.Model_Tables;

namespace SMS_MVC.Models
{
    public class UserDashboardModel
    {
        public List<Users> _Users { get; set; }
        public int TotalUsers { get; set; }
        public int Admins { get; set; }
        public int Teachers { get; set; }
        public int Parents { get; set; }
        public int Accountants { get; set; }
        public int Librarians { get; set; }
        public int Students { get; set; }
        public int Staffs { get; set; }
        public string ActiveTab { get; set; }
    }
}
