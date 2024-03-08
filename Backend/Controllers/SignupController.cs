using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace LoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : Controller
    {
        [HttpPost]
        public IActionResult PostLogin()
        {
            // Read the request body as a string
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = reader.ReadToEndAsync().Result;

            // Parse the JSON string to a object
            var jsonDocument = JsonDocument.Parse(body);
            var root = jsonDocument.RootElement;

            var signup_details = new LoginAPI.Model.Signup_Data
            {
                Name = root.GetProperty("name").GetString(),
                Email = root.GetProperty("email").GetString(),
                Username = root.GetProperty("username").GetString(),
                Password = root.GetProperty("password").GetString()
            };

            int user_id = LoginAPI.Factory.Signup_Handeler.Signup(signup_details);
            var json_to_response = new LoginAPI.Model.Signup_Response { Message = "" };
            if (user_id == 0)
            {
                json_to_response.Message = "User Creation Failed.";
                return BadRequest(JsonSerializer.Serialize(json_to_response));
            }
            json_to_response.Message = "Registered Successfully.";
            return Ok(JsonSerializer.Serialize(json_to_response));
        }
    }
}
