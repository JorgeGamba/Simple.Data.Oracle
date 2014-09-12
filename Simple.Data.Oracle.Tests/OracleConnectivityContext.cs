using System.Configuration;
using Simple.Data.Ado.Schema;
using System.Collections.Generic;
using System.Linq;
#if DEVART
using Devart.Data.Oracle;
#elif MANAGEDODP
using Oracle.ManagedDataAccess.Client;
#else
using Oracle.DataAccess.Client;
#endif

namespace Simple.Data.Oracle.Tests
{
    internal class OracleConnectivityContext
    {
        #if DEVART
        const string ConnectionName = "DevartOracle";
        #elif MANAGEDODP
        const string ConnectionName = "ManagedOdpOracle";
        #else
        const string ConnectionName = "OracleClient";
        #endif

        protected dynamic _db;
        protected string _connectionString;
        protected string _providerName;

        public OracleConnectivityContext()
        {
            _connectionString = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
            _providerName = ConfigurationManager.ConnectionStrings[ConnectionName].ProviderName;
        }

        protected void InitDynamicDB()
        {
            _db = Database.Opener.OpenConnection(_connectionString, _providerName, "PEPITO");
        }

        protected List<Table> Tables { get; private set; }

        protected Table TableByName(string name)
        {
            return Tables.Single(t => t.ActualName.InvariantEquals(name));
        }

        protected OracleConnectionProvider GetConnectionProvider()
        {
            var p = new OracleConnectionProvider();
            p.SetConnectionString(_connectionString);
            return p;
        }

        protected SqlReflection GetSqlReflection()
        {
            return new SqlReflection(GetConnectionProvider());
        }

        protected OracleSchemaProvider GetSchemaProvider()
        {
            var schemaProvider = new OracleSchemaProvider(GetConnectionProvider());
            Tables = schemaProvider.GetTables().ToList();
            return schemaProvider;
        }

        protected OracleCommand GetCommand(string text)
        {
            var con = new OracleConnection(_connectionString);
            var c = new OracleCommand(text, con);
            return c;
        }
    }
}