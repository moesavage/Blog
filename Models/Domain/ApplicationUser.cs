using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        //public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<IdentityUserRole<string>> Roles { get; set; } = new List<IdentityUserRole<string>>();

    }

    //[Table("AspNetRoles")]
    //public class Role
    //{
    //    [Required(ErrorMessage = "Enter Role name")]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
    //    public string RoleName { get; set; }
        
    //    [Key]
    //    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    //    public int RoleId { get; set; }
    //}
}
