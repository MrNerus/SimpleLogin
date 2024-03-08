namespace LoginAPI.Factory
{
    public class Login_Service_Handeler
    {
        public static bool Login_Valiator(LoginAPI.Model.Login_Data data)
        {
            Console.WriteLine($"{{");
            Console.WriteLine($"\tUsername: {data.Username},");
            Console.WriteLine($"\tPassword: {data.Password}");
            Console.WriteLine($"}}");
            return true;
        }
    }
}

