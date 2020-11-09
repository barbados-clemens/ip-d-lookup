using IPdLookUp.Core.Validators;
using NUnit.Framework;

namespace IpDLookUp.Tests
{
    public class Validators
    {
        [Test]
        [TestCase("calebukle.com")]
        [TestCase("music.calebukle.com")]
        public void should_validate_domain_name(string domainToTest)
        {
            var v = new ValidAddressAttribute();

            Assert.IsTrue(v.IsValid("calebukle.com"),
                $"Valid domain was validated false. Expect {domainToTest} to have passed ");
        }

        [Test]
        [TestCase(".104.18.61")]
        [TestCase("2001:db8:85a3:8d3:1319:8a2e:370:7348")]
        public void should_not_validate_bad_ip(string ipToTest)
        {
            var v = new ValidAddressAttribute();

            Assert.IsFalse(v.IsValid(ipToTest), $"Bad IP passed that shouldn't, {ipToTest}");

            Assert.IsTrue(v.IsValid("1.1.1.1"), "Valid IPv4 address was wrongly rejected!");
        }

        [Test]
        [TestCase("calebukle.com")]
        [TestCase("music.calebukle.com")]
        public void should_match_domain_names(string domain)
        {
            Assert.True(Services.Models.Validators.DomainName.IsMatch(domain),
                $"Didn't match domain address: {domain}");
        }

        [Test]
        [TestCase("104.18.61.137")]
        [TestCase("141.101.64.10")]
        public void should_match_ipv4_address(string ipToTest)
        {
            Assert.True(Services.Models.Validators.IPv4.IsMatch(ipToTest),
                $"Didn't match IP address as IPv4 Address: {ipToTest}");
        }

        [Test]
        [TestCase("104.18.61")]
        [TestCase("2001:db8:85a3:8d3:1319:8a2e:370:7348")]
        [TestCase("10")]
        [TestCase("Blah")]
        [TestCase("test.test.test.test")]
        [TestCase(".10.10.10")]
        [TestCase("800.900.011.099")]
        [TestCase("256,256,256,256")]
        public void should_not_match_invalid_ipv4(string ipToTest)
        {
            Assert.False(Services.Models.Validators.IPv4.IsMatch(ipToTest),
                $"Shouldn't match IP address as IPv4 Address: {ipToTest}");
        }

        [Test]
        [TestCase("https://")]
        [TestCase("ftps://")]
        [TestCase("ftp://")]
        [TestCase("http://")]
        [TestCase("https://calebukle.com")]
        [TestCase("http://music.calebukle.com")]
        public void should_match_protocols(string domainWithProtocol)
        {
            Assert.True(Services.Models.Validators.Protocol.IsMatch(domainWithProtocol),
                $"Didn't match protocol: {domainWithProtocol}");
        }
    }
}