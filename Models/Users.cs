namespace SMS_MVC.Models
{
    public class Users
    {
        public int id { get; set; }
        public String UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
