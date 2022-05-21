using System.ComponentModel.DataAnnotations;

namespace topGlove.Model
{
    public class UserDetails
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public string Role { get; set; }     
        public string AdditionalInfo { get; set; }
        public string CreatedBy { get; set; }

    }
}
