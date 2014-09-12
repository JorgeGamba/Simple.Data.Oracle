using NUnit.Framework;

namespace Simple.Data.Oracle.Tests
{
    [TestFixture]
    public class OracleConnectionProviderTests
    {
        [Test]
        public void SetConnectionStringShouldSetConnectionProviderKeyToo()
        {
            var connectionProvider = new OracleConnectionProvider();
            var anyConnectionString = "anyConnectionString";
            
            connectionProvider.SetConnectionString(anyConnectionString);

            Assert.AreEqual(anyConnectionString, connectionProvider.ConnectionProviderKey);
        }
    }
}