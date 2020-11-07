using System.Threading.Tasks;
using IpDLookUp.Services;
using IpDLookUp.Services.Types;
using NUnit.Framework;

namespace IpDLookUp.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public async Task reverse_dns_check()
        {
            var rd = new ReverseDns();

            var res  = await rd.DoLookUp("69.109.161.49", AddressType.Ip);
            Assert.IsNotNull(res);
        }
    }
}