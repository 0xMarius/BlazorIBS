using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DatabaseWork
{
    public class OracleService
    {
        // username ibs conflicts with common user or role name, so it must start with prefix 'C##'
        // user C##ibs has CRUD, SESSION, SELECT privileges and 100M quota
        private readonly string _connectionString;
        // accept the connection string from the appsettings.json file
        public OracleService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public DataTable getDataTable(string query)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    var dataTable = new DataTable();
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable;
                }
            }
        }

        public long getValue(string query)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    return Convert.ToInt64(command.ExecuteScalar());
                }
            }
        }
    }
}
