using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IPdLookUp.Core.Controllers;
using IPdLookUp.Core.Entities;
using IPdLookUp.Core.Models;
using IpDLookUp.Services.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace IpDLookUp.Tests
{
    public class CoreTests
    {
        private ILogger<LookupController> _fakeLog;

        private IConfiguration _fakeConfig;


        [SetUp]
        public void SetUp()
        {
            _fakeLog = new NullLogger<LookupController>();

            _fakeConfig = new ConfigurationBuilder()
                .AddJsonFile("appSettings.Test.json")
                .Build();
        }

        [Test]
        public void should_handle_bad_result()
        {
            var actual = WorkerHelper.HandleInvalidResult((ServiceType) 50);

            Assert.IsNotNull(actual.ErrorMessage);
            Assert.IsNotEmpty(actual.FailServices);
        }

        [Test]
        public async Task should_set_item_with_bad_result()
        {
            var actual = await WorkerHelper.SendToWorkers("", "", new List<ServiceType>()
            {
                ServiceType.GeoIP
            });

            Assert.IsNotNull(actual.GeoIp.ErrorMessage);
            Assert.AreEqual(ServiceStatus.Error, actual.GeoIp.Status);
        }

        /// <summary>
        /// this test requires running the worker project on whatever address is described in the appSettings.Test.json
        /// Default is localhost:6001
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task should_return_results()
        {
            var ctrl = new LookupController(_fakeLog, _fakeConfig);
            var req = new LookUpRequest
            {
                Address = "calebukle.com",
            };

            var actual = await ctrl.RunTasks(req);

            Assert.IsInstanceOf<OkObjectResult>(actual);
        }

        [Test]
        public void should_throw_without_correct_config()
        {
            var badConfig = new ConfigurationBuilder()
                .Build();
            Assert.Throws<ArgumentException>(() => new LookupController(_fakeLog, badConfig));
        }
    }
}