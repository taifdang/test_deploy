using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("Database") ?? string.Empty;
        }
        [HttpPost]
        public async Task<IActionResult> get()
        {
            var data = new List<User>();
            var query = "select*from users";
            using var connection = new SqlConnection(connectionString);
            data = connection.Query<User>(query).ToList();

            return Ok(data);
        }
    }
}
