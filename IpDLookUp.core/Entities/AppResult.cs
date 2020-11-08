using System.Collections.Generic;
using System.Net;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IPdLookUp.Entities
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

    public class AppResult : IAppResult
    {
        public string Address { get; set; }

        public IEnumerable<ServiceType> Services { get; set; }

        public IServiceResult<GeoIpModel>? GeoIp { get; set; }

        public IServiceResult<RdapModel>? Rdap { get; set; }

        public IServiceResult<IPHostEntry>? ReverseDns { get; set; }

        public IServiceResult<SslLabsModel>? SslLabs { get; set; }

        public IServiceResult<PingModel>? Ping { get; set; }
    }

    public struct AppErrorResult : IAppErrorResult
    {
        public string ErrorMessage { get; set; }

        public ModelErrorCollection Errors { get; set; }

        public IEnumerable<ServiceType> FailServices { get; set; }
    }

    public struct AppPartialResult : IAppResult, IAppErrorResult
    {
        public string Address { get; set; }

        public IEnumerable<ServiceType> Services { get; set; }

        public IServiceResult<GeoIpModel>? GeoIp { get; set; }

        public IServiceResult<RdapModel>? Rdap { get; set; }

        public IServiceResult<IPHostEntry>? ReverseDns { get; set; }

        public IServiceResult<SslLabsModel>? SslLabs { get; set; }

        public IServiceResult<PingModel>? Ping { get; set; }

        public string ErrorMessage { get; set; }

        public IEnumerable<ServiceType> FailServices { get; set; }
    }
}