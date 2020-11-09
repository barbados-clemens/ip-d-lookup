using System;
using System.Net.Http;
using System.Threading.Tasks;
using IpDLookUp.Services;
using IpDLookUp.Services.Types;
using NUnit.Framework;

namespace IpDLookUp.Tests
{
    public class Tests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();
        }


        [Test]
        public async Task should_get_geo_by_domain()
        {
            var geo = new GeoIp(_client);
            var actual = await geo.DoLookUp("calebukle.com", AddressType.DomainName);

            Assert.IsNotNull(actual.Data);
            Assert.AreEqual(ServiceStatus.Ok, actual.Status);
        }

        [Test]
        public async Task should_get_geo_by_ip()
        {
            var geo = new GeoIp(_client);
            var actual = await geo.DoLookUp("1.1.1.1", AddressType.Ip);

            Assert.IsNotNull(actual.Data);
            Assert.AreEqual(ServiceStatus.Ok, actual.Status);
        }

        [Test]
        public async Task should_ping_domain()
        {
            var p = new PingService();
            var actual = await p.DoLookUp("calebukle.com", AddressType.DomainName);

            Assert.IsNotNull(actual.Data);
            Assert.AreEqual(ServiceStatus.Ok, actual.Status);
        }

        [Test]
        public async Task should_handle_null_ping()
        {
            var p = new PingService();
            var actual = await p.DoLookUp(null, AddressType.DomainName);

            Assert.IsNotNull(actual.ErrorMessage);
            Assert.AreEqual(ServiceStatus.Bad, actual.Status);
        }

        [Test]
        public async Task should_get_Rdap_info()
        {
            var rdap = new Rdap(_client);
            var actual = await rdap.DoLookUp("calebukle.com", AddressType.DomainName);

            Assert.IsNotNull(actual.Data);
            Assert.AreEqual(ServiceStatus.Ok, actual.Status);
        }

        [Test]
        public async Task should_get_rdap_by_ip()
        {
            var rdap = new Rdap(_client);
            var actual = await rdap.DoLookUp("1.1.1.1", AddressType.Ip);

            Assert.IsNotNull(actual.Data);
            Assert.AreEqual(ServiceStatus.Ok, actual.Status);
        }

        [Test]
        public async Task rdap_should_throw_bad_address_type()
        {
            var rdap = new Rdap(_client);

            Assert.Catch<AggregateException>(() => rdap.DoLookUp("calebukle.com", (AddressType) 50).Wait());
        }

        [Test]
        public async Task should_get_reverse_dns_data()
        {
            var rd = new ReverseDns();
            var actual = await rd.DoLookUp("69.109.161.49", AddressType.Ip);

            Assert.IsNotNull(actual.Data);
            Assert.AreEqual(ServiceStatus.Ok, actual.Status);
        }


        [Test]
        public async Task should_have_reverse_dns_user_error()
        {
            var rd = new ReverseDns();
            var actual = await rd.DoLookUp("calebukle.com", AddressType.DomainName);

            Assert.IsNull(actual.Data);
            Assert.IsNotNull(actual.ErrorMessage);
            Assert.AreEqual(ServiceStatus.Bad, actual.Status);
        }

        [Test]
        public async Task should_handle_reverse_dns_socket_error()
        {
            var rd = new ReverseDns();
            // ip address that shouldn't resolve.
            var actual = await rd.DoLookUp("18.45.89.54", AddressType.Ip);

            Assert.IsNull(actual.Data);
            Assert.IsNotNull(actual.ErrorMessage);
            Assert.AreEqual(ServiceStatus.Error, actual.Status);
        }

        [Test]
        public async Task should_handle_reverse_dns_format_error()
        {
            var rd = new ReverseDns();
            var actual = await rd.DoLookUp("blahg.bad.ip", AddressType.Ip);
            Assert.IsNull(actual.Data);
            Assert.IsNotNull(actual.ErrorMessage);
            Assert.AreEqual(ServiceStatus.Bad, actual.Status);
        }


        [Test]
        public async Task should_get_ssl_report()
        {
            var ssl = new SslLabs(_client);
            var actual = await ssl.DoLookUp("calebukle.com", AddressType.DomainName);

            Assert.IsNotNull(actual.Data);
            Assert.AreEqual(ServiceStatus.Ok, actual.Status);
        }

        [Test]
        public void should_normalize_domain_name()
        {
            var expected = "calebukle.com";
            var actual = ServiceProcessor.NormalizeAddress("https://calebukle.com", out var actualType);

            Assert.AreEqual(AddressType.DomainName, actualType);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void should_normalize_ip()
        {
            var expected = "1.1.1.1";
            var actual = ServiceProcessor.NormalizeAddress("    1.1.1.1  ", out var actualType);

            Assert.AreEqual(AddressType.Ip, actualType);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void should_throw_on_unknown_address()
        {
            Assert.Throws<ArgumentException>(() => ServiceProcessor.NormalizeAddress(string.Empty, out var type));
        }

        [Test]
        public async Task should_catch_on_unknown_service()
        {
            var actual = await ServiceProcessor.Process<object>("calebukle.com", (ServiceType) 50);

            Assert.IsNotNull(actual.ErrorMessage);
            Assert.AreEqual(ServiceStatus.Error, actual.Status);
        }
    }
}