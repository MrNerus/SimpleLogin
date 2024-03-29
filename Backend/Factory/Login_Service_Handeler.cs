﻿using Npgsql;

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


            string connectionString = "Host=localhost:5432;Username=postgres;Password=postgres;Database=Logins";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"SELECT ard_validate_creds(@username, @password)";
                    cmd.Parameters.AddWithValue("username", data.Username);
                    cmd.Parameters.AddWithValue("password", data.Password);

                    return (int)cmd.ExecuteScalar() != 0;
                }
            }
        }
    }
}

