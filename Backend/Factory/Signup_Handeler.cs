using Npgsql;

namespace LoginAPI.Factory
{
    public class Signup_Handeler
    {
        public static int Signup(LoginAPI.Model.Signup_Data data)
        {
            Console.WriteLine($"{{");
            Console.WriteLine($"\tName: {data.Name},");
            Console.WriteLine($"\tEmail: {data.Email},");
            Console.WriteLine($"\tUsername: {data.Username},");
            Console.WriteLine($"\tPassword: {data.Password}");
            Console.WriteLine($"}}");


            string connectionString = "Host=localhost:5432;Username=postgres;Password=postgres;Database=Logins";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"SELECT ard_create_user(@username, @password, @name, @email)";
                    cmd.Parameters.AddWithValue("username", data.Username);
                    cmd.Parameters.AddWithValue("password", data.Password);
                    cmd.Parameters.AddWithValue("name", data.Name);
                    cmd.Parameters.AddWithValue("email", data.Email);

                    return (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
