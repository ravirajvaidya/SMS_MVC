namespace SMS_MVC.Models.Model_Tables
{
    public class Students
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime AdmissionDate { get; set; }
    }
}
