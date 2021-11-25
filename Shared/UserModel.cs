namespace ProITM.Shared
{
    public class UserModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool IsAdmin { get; set; }
    }
}
