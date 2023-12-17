using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models.DTO
{
    public class CreateAdmins
    {
        public string Email { get; set; }

        //[Required]
        //[Display(Name = "Username")]
        public string Username { get; set; }

        //[Required]
        //[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Minimum length 6 and must contain 1 Uppercase, 1 lowercase, 1 special character and 1 digit")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        public string Password { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm Password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //[Required]
        //[Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        //[Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string? Role { get; set; } = "";
    }

    public class SaveUsers
    {
        public string Email { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Role { get; set; }
    }
    public class UpdateUsers
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }
        //public string Password { get; set; } = "Password1";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? SelectedRole { get; set; }
    }

    public class OtherUsers
    {
        public string Id { get; set; }
    }

    //public class CreateUsersViewModel
    //{
    //    public IEnumerable<IdentityRole> Roles { get; set; }
    //}
    public class CreateUsersViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public InputModel Input { get; set; } // Add this property

        public string SelectedRole { get; set; }

        public string Role { get; set; }


        public IEnumerable<IdentityRole> Roles { get; set; }
    }

    public class InputModel
    {
        public string Role { get; set; }
        public string RoleId { get; set; }
        // Add other properties as needed
    }
}
