// Example controller class for a sample API
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;

namespace DatabaseWork
{
    [ApiController]
    [Route("api/controller")]
    public class SampleController : ControllerBase
    {
        private readonly OracleService _oracleService;
        public SampleController(OracleService oracleService)
        {
            _oracleService = oracleService;
        }


        // GET: api/controller/datatable
        [HttpGet("datatable")]
        public ActionResult<string> Get()
        {
            DataTable dataTable = _oracleService.getDataTable("SELECT * FROM Saskaitos ORDER BY pavarde");

            // Serialize the DataTable to JSON
            string jsonData = JsonConvert.SerializeObject(dataTable);

            // Return the JSON data as the response
            return Content(jsonData, "application/json");
        }

        // POST: api/controller/process   
        [HttpPost("process")]
        public ActionResult<string> ProcessData([FromBody] string data)
        {
            // Deserialize the JSON string into a dynamic object
            dynamic jsonData = JsonConvert.DeserializeObject(data);

            // Access individual properties
            string vardas = jsonData.vardas;
            string pavarde = jsonData.pavarde;
            string asmensKodas = jsonData.asmensKodas;
            string saskaitosNr = jsonData.saskaitosNr;

            string insertQuery = $"INSERT INTO Saskaitos(vardas, pavarde, asmens_kodas, saskaitos_numeris, lesos) VALUES('{vardas}', '{pavarde}', '{asmensKodas}', '{saskaitosNr}', 0.00)";
            _oracleService.getDataTable(insertQuery);

            // Return the JSON data as the response
            return "Account created successfully.";
        }
    }
}
