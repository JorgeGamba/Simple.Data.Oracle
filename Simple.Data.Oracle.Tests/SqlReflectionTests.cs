using System.Collections.Specialized;
using NUnit.Framework;
using Simple.Data.Oracle.Configuration;

namespace Simple.Data.Oracle.Tests
{
    [TestFixture]
    public class SqlReflectionTests
    {
        private string _expectedSchema = "EXPECTEDSCHEMA";

        [Test]
        public void WhenConnectionProviderHasSchemaItShouldBeUsed()
        {
            var connectionProvider = new OracleConnectionProvider();
            connectionProvider.SetSchema(_expectedSchema);

            var schemaReflector = new SqlReflection(connectionProvider);

            Assert.AreEqual(_expectedSchema, schemaReflector.Schema);
        }

        [Test]
        public void WhenConnectionProviderHasNoSchemaSettingsFileSchemaShouldBeUsed()
        {
            var connectionProvider = new OracleConnectionProvider();
            connectionProvider.SetSchema(null);
            ConfigurationProvider.AppSettings = () => new NameValueCollection { { "Simple.Data.Oracle.Schema", _expectedSchema } };

            var schemaReflector = new SqlReflection(connectionProvider);

            Assert.AreEqual(_expectedSchema, schemaReflector.Schema);
        }

        [Test]
        public void WhenSettingsFileHasNoSchemaUserOfConnectionShouldBeUsed()
        {
            var connectionProvider = new OracleConnectionProvider();
            connectionProvider.SetSchema(null);
            ConfigurationProvider.AppSettings = () => new NameValueCollection();
            var connectionString = "user id=" + _expectedSchema;
            connectionProvider.SetConnectionString(connectionString);

            var schemaReflector = new SqlReflection(connectionProvider);

            Assert.AreEqual(_expectedSchema, schemaReflector.Schema);
        }
    }
}