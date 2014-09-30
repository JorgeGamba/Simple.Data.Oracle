using NUnit.Framework;
using Simple.Data.Ado;
using Trespasser;

namespace Simple.Data.Oracle.Tests
{
    [TestFixture]
    internal class OracleConnectionProviderTests
    {
        private AdoAdapter _noMatterAdapter = null;
        private string _expectedSchema = "EXPECTEDSCHEMA";
        private string _unexpectedSchema = "UNEXPECTEDSCHEMA";
        private string _existingProcedureName = "ADD_JOB_HISTORY";
        private string _noSetProcedureSchema = null;

        [Test]
        public void SetConnectionStringShouldSetConnectionProviderKeyToo()
        {
            var connectionProvider = new OracleConnectionProvider();
            connectionProvider.SetConnectionString("anyConnectionString");

            Assert.AreEqual("anyConnectionString", connectionProvider.ConnectionProviderKey);
        }

        [Test]
        public void WhenProcedureSchemaWasSetItShouldBeUsedInProcedureName()
        {
            var procedure = new ObjectName(_expectedSchema, _existingProcedureName);
            var connectionProvider = new OracleConnectionProvider();
            var connectionString = "user id=" + _unexpectedSchema;
            connectionProvider.SetConnectionString(connectionString);

            var result = connectionProvider.GetProcedureExecutor(_noMatterAdapter, procedure);

            Assert.AreEqual(_expectedSchema, UsedSchemaFrom(result));
        }

        [Test]
        public void WhenSetEmptySchemaItShouldBeDismissed()
        {
            var connectionProvider = new OracleConnectionProvider();
            connectionProvider.SetSchema(_expectedSchema);

            connectionProvider.SetSchema(string.Empty);
            
            Assert.AreEqual(_expectedSchema, connectionProvider.Schema);
        }

        [Test]
        public void WhenProcedureSchemaWasNotSetItsOwnSchemaShouldBeUsedInProcedureName()
        {
            var procedureWithoutSchema = new ObjectName(schema: null, name: _existingProcedureName);
            var connectionProvider = new OracleConnectionProvider();
            var connectionString = "user id=" + _unexpectedSchema;
            connectionProvider.SetConnectionString(connectionString);
            connectionProvider.SetSchema(_expectedSchema);

            var result = connectionProvider.GetProcedureExecutor(_noMatterAdapter, procedureWithoutSchema);

            Assert.AreEqual(_expectedSchema, UsedSchemaFrom(result));
        }

        [Test]
        public void WhenItsOwnSchemaWasNotSetUserOfConnectionShouldBeUsedInProcedureName()
        {
            var anyProcedure = new ObjectName(_noSetProcedureSchema, _existingProcedureName);
            var connectionProvider = new OracleConnectionProvider();
            var connectionString = "user id=" + _expectedSchema;
            connectionProvider.SetConnectionString(connectionString);

            var result = connectionProvider.GetProcedureExecutor(_noMatterAdapter, anyProcedure);

            Assert.AreEqual(_expectedSchema, UsedSchemaFrom(result));
        }


        private static dynamic UsedSchemaFrom(IProcedureExecutor result)
        {
            var proxy = Proxy.Create(result);
            var usedSchema = proxy._procedureName.ToString().Split('.')[0];
            return usedSchema;
        }
    }
}