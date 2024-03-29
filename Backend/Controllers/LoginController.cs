﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace LoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {

        // POST: LoginController/Post
        [HttpPost]
        public ActionResult PostLogin()
        {
            // Read the request body as a string
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = reader.ReadToEndAsync().Result;

            // Parse the JSON string to a object
            var jsonDocument = JsonDocument.Parse(body);
            var root = jsonDocument.RootElement;

            var credential = new LoginAPI.Model.Login_Data
            {
                Username = root.GetProperty("username").GetString(),
                Password = root.GetProperty("password").GetString()
            };

            bool login_Status = LoginAPI.Factory.Login_Service_Handeler.Login_Valiator(credential);
            var json_to_response = new LoginAPI.Model.Login_Response { Message = "" };

            if (!login_Status)
            {
                json_to_response.Message = "Invalid Username or Password.";
                return Unauthorized(JsonSerializer.Serialize(json_to_response));
            }
            json_to_response.Message = "Login Successful.";
            return Ok(JsonSerializer.Serialize(json_to_response));
        }

    }
}
