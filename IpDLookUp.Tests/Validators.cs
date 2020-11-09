using NUnit.Framework;

namespace IpDLookUp.Tests
{
    public class Validators
    {
        [Test]
        public void should_match_domain_names()
        {
            var names = new[]
            {
                "calebukle.com",
                "music.calebukle.com",
            };

            foreach (var name in names)
            {
                Assert.True(Services.Models.Validators.DomainName.IsMatch(name), $"Didn't match domain address: {name}");
            }
        }

        [Test]
        public void should_match_ipv4_address()
        {
            var ips = new[]
            {
                "104.18.61.137",
                "141.101.64.10",
            };

            foreach (var ip in ips)
            {
                Assert.True(Services.Models.Validators.IPv4.IsMatch(ip), $"Didn't match IP address as IPv4 Address: {ip}");
            }
        }

        [Test]
        public void should_not_match_invalid_ipv4()
        {
            var ips = new[]
            {
                "104.18.61",
                "2001:db8:85a3:8d3:1319:8a2e:370:7348",
                "10",
                "Blah",
                "test.test.test.test",
                ".10.10.10",
                "800.900.011.099",
                "256,256,256,256"
            };

            foreach (var ip in ips)
            {
                Assert.False(Services.Models.Validators.IPv4.IsMatch(ip),
                    $"Shouldn't match IP address as IPv4 Address: {ip}");
            }
        }

        [Test]
        public void should_match_protocols()
        {
            var protos = new[]
            {
                "https://",
                "ftps://",
                "ftp://",
                "http://",
                "https://calebukle.com",
                "http://music.calebukle.com",
            };

            foreach (var proto in protos)
            {
                Assert.True(Services.Models.Validators.Protocol.IsMatch(proto), $"Didn't match protocol: {proto}");
            }
        }
    }
}