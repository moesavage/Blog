namespace Blog.Models
{
    public class UserModel
    {
        public string Username { get; set; }

        public string Email { get; set; }
        public string Password { get; set; } = "00000";
        public string Role { get; set; }
    }

    public class UserWithRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> RolesNames { get; set; }
    }
}
