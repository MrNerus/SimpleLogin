namespace LoginAPI.Model
{
    public class Signup_Data
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }

    public class Signup_Response
    {
        public string Message { get; set;}
    }
}
