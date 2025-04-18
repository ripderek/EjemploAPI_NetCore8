using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using EjemploAPI.Models;

namespace EjemploAPI.Controllers
{
    //el [controller] es el nombre de la clase en minusculas y sin el Controller del final
    // es decir, PersonasController => localhost:[puerto]/personas
    [ApiController]
    [Route("[controller]")]
    public class PersonasController: ControllerBase
    {
            private readonly IConfiguration _configuration;

            public PersonasController(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            private SqlConnection GetConnection() =>
                new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        // GET: /personas
        [HttpGet]
            public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
            {
                var persons = new List<Person>();

                using (var conn = GetConnection())
                {
                    using var cmd = new SqlCommand("sp_GetAllPersons", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();
                    var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        persons.Add(new Person
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Age = reader.GetInt32(2)
                        });
                    }
                }

                return persons;
            }

        // GET: /personas/{id}
        [HttpGet("{id}")]
            public async Task<ActionResult<Person>> GetPerson(int id)
            {
                using var conn = GetConnection();
                using var cmd = new SqlCommand("sp_GetPersonById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new Person
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Age = reader.GetInt32(2)
                    };
                }

                return NotFound();
            }

        // POST: /personas
        [HttpPost]
            public async Task<ActionResult> PostPerson(Person person)
            {
                using var conn = GetConnection();
                using var cmd = new SqlCommand("sp_InsertPerson", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", person.Name);
                cmd.Parameters.AddWithValue("@Age", person.Age);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return Ok();
            }

        // PUT: /personas/5
        [HttpPut("{id}")]
            public async Task<IActionResult> PutPerson(int id, Person person)
            {
                using var conn = GetConnection();
                using var cmd = new SqlCommand("sp_UpdatePersonById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", person.Name);
                cmd.Parameters.AddWithValue("@Age", person.Age);

                await conn.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();

                if (rows == 0) return NotFound();
                return NoContent();
            }

        // DELETE: /personas/5
        [HttpDelete("{id}")]
            public async Task<IActionResult> DeletePerson(int id)
            {
                using var conn = GetConnection();
                using var cmd = new SqlCommand("sp_DeletePersonById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();

                if (rows == 0) return NotFound();
                return NoContent();
            }
        
    }
}
