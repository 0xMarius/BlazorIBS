using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DatabaseWork
{
    [ApiController]
    [Route("api/")]
    public class BaseController : ControllerBase
    {
        private readonly OracleService _oracleService;
        public BaseController(OracleService oracleService)
        {
            _oracleService = oracleService;
        }

        // GET: api/
        [HttpGet]
        public ActionResult<string> Get()
        {
            DataTable dataTable = _oracleService.getDataTable("SELECT * FROM Saskaitos ORDER BY pavarde");

            // Serialize the DataTable to JSON
            string jsonData = JsonConvert.SerializeObject(dataTable);

            // Return the JSON data as the response
            return Content(jsonData, "application/json");
        }

        // POST: api/create
        [HttpPost("create")]
        public ActionResult<string> CreateNewAccount([FromBody] string data)
        {
            // deserialize the JSON string into a dynamic object
            dynamic jsonData = JsonConvert.DeserializeObject(data);

            // access individual properties
            string vardas = jsonData.vardas;
            string pavarde = jsonData.pavarde;
            string asmensKodas = jsonData.asmensKodas;
            string saskaitosNr = GenerateSaskaitosNr(asmensKodas);

            string numberOfAccounts = $"SELECT COUNT(*) FROM Saskaitos WHERE asmens_kodas = {asmensKodas}";

            // if the count of rows where the value in the asmens_kodas column matches asmensKodas equals 0
            if (_oracleService.getValue(numberOfAccounts) == 0)
            {
                string insertQuery = $"INSERT INTO Saskaitos(vardas, pavarde, asmens_kodas, saskaitos_numeris, lesos) VALUES('{vardas}', '{pavarde}', '{asmensKodas}', '{saskaitosNr}', 0.00)";
                _oracleService.getDataTable(insertQuery);
                // Return the JSON data as the response
                return "Account creation successful.";
            }
            else
            {
                return "Account creation failed: already exists.";
            }
        }
        private string GenerateSaskaitosNr(string asmensKodas)
        {
            byte[] hashBytes;

            // TO-DO: sreplace with stronger hashing + random salting values
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(asmensKodas));
            }

            return Convert.ToBase64String(hashBytes);
        }
    }
}
