namespace SMS_MVC.Models.Model_Tables
{
    public class Teachers
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int SubjectId { get; set; }
        public string Qualification { get; set; }
        public int Experience { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}
