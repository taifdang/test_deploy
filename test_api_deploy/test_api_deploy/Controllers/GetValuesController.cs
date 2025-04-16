﻿using Dapper;
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
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("Database") ?? string.Empty;
        }
        [HttpGet]
        public IActionResult get()
        {
            var data = new List<User>();
            var query = "select*from users";
            using var connection = new SqlConnection(connectionString);
            data = connection.Query<User>(query).ToList();

            return Ok(data);
        }
        [HttpGet("get_id")]
        public IActionResult getId( int id)
        {
            var data = new User();
            var query = $"select*from users";
            using var connection = new SqlConnection(connectionString);
            data =  connection.Query<User>(query).FirstOrDefault(x => x.id == id);

            return Ok(data);
        }
        [HttpPost]
        public IActionResult add(string name)
        {
            //1231
            User user = new User() { name = name };

            var query = "INSERT INTO users (name) VALUES (@name)";
            using IDbConnection connection = new SqlConnection(connectionString);

            int rowsAffected = connection.Execute(query, user);
            User data = connection.Query<User>("select*from users").FirstOrDefault(x => x.name == name);
            return Ok(data);
        }
    }
}
