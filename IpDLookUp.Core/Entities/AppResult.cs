using System.Collections.Generic;
using System.Net;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IPdLookUp.Core.Entities
{
    public interface IAppResult
    {
        public string Address { get; set; }

        public IEnumerable<ServiceType> Services { get; set; }

        public IServiceResult<GeoIpModel>? GeoIp { get; set; }

        public IServiceResult<RdapModel>? Rdap { get; set; }

        public IServiceResult<IPHostEntry>? ReverseDns { get; set; }

        public IServiceResult<SslLabsModel>? SslLabs { get; set; }

        public IServiceResult<PingModel>? Ping { get; set; }
    }

    public interface IAppErrorResult
    {
        public string ErrorMessage { get; set; }

        public IEnumerable<ServiceType> FailServices { get; set; }
    }

    /// <summary>
    /// Main result to be returned by the controller
    /// <para>
    /// Each service has it's own property as they have specific values to each.
    /// Only services that are ran should have a value
    /// </para>
    /// <para>
    /// this is a class because we need to be able to set properties by reference in WorkerHelper.CopyItem()
    /// </para>
    /// </summary>
    public class AppResult : IAppResult, IAppErrorResult
    {
        public string Address { get; set; }

        public IEnumerable<ServiceType> Services { get; set; }

        public IServiceResult<GeoIpModel>? GeoIp { get; set; }

        public IServiceResult<RdapModel>? Rdap { get; set; }

        public IServiceResult<IPHostEntry>? ReverseDns { get; set; }

        public IServiceResult<SslLabsModel>? SslLabs { get; set; }

        public IServiceResult<PingModel>? Ping { get; set; }

        /// <summary>
        /// this is only set when an error happens
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// This is only set when an error happens and is used for knowing what services had an error
        /// </summary>
        public IEnumerable<ServiceType>? FailServices { get; set; }
    }

    /// <summary>
    /// Simple error object for controller to use
    /// </summary>
    public struct AppErrorResult : IAppErrorResult
    {
        public string ErrorMessage { get; set; }

        public ModelErrorCollection Errors { get; set; }

        public IEnumerable<ServiceType> FailServices { get; set; }
    }
}