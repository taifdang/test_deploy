using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using test_api_deploy.Models;

namespace test_api_deploy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        string connectionString;
        public GetValuesController(IConfiguration configuration)
        {
            //12312321
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("Database") ?? string.Empty;
        }
        [HttpGet]
        public Task<IActionResult> get()
        {
            var data = new List<User>();
            var query = "select*from users";
            using var connection = new SqlConnection(connectionString);
            data = connection.Query<User>(query).ToList();

            return Task.FromResult<IActionResult>(Ok(data));
        }
        [HttpGet("get_id")]
        public Task<IActionResult> getId(int id)
        {
            
            var query = $"select*from users";
            using var connection = new SqlConnection(connectionString);
            User data = connection.Query<User>(query).FirstOrDefault(x => x.id == id);

            return Task.FromResult<IActionResult>(Ok(data));
        }
        [HttpPost]
        public Task<IActionResult> add(string name)
        {
            User user = new User() { name = name };

            var query = "INSERT INTO users (name) VALUES (@name)";
            using IDbConnection connection = new SqlConnection(connectionString);

            int rowsAffected = connection.Execute(query, user);
            User data = connection.Query<User>("select*from users").FirstOrDefault(x => x.name == name);
            return Task.FromResult<IActionResult>(Ok(data));
        }

    }
}