﻿namespace LoginAPI.Model
{
    public class Login_Data
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Login_Response
    {
        public string Message { get; set;}
    }
}
