using System.Collections.Generic;
using IpDLookUp.Services;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IPdLookUp.Entities
{
    public interface IAppResult
    {
        public string Address { get; set; }

        public List<ServiceType> Services { get; set; }

        public List<IServiceResult> Results { get; set; }
    }

    public interface IAppErrorResult
    {
        public string ErrorMessage { get; set; }

        public List<ServiceType> FailServices { get; set; }
    }

    public struct AppResult : IAppResult
    {
        public string Address { get; set; }

        public List<ServiceType> Services { get; set; }
        public List<IServiceResult> Results { get; set; }
    }

    public struct AppErrorResult : IAppErrorResult
    {
        public string ErrorMessage { get; set; }

        public ModelErrorCollection Errors { get; set; }

        public List<ServiceType> FailServices { get; set; }
    }

    public struct AppPartialResult : IAppResult, IAppErrorResult
    {
        public string Address { get; set; }

        public List<ServiceType> Services { get; set; }

        public List<IServiceResult> Results { get; set; }

        public string ErrorMessage { get; set; }

        public List<ServiceType> FailServices { get; set; }
    }

    public interface IServiceSpecResult
    {
    }
}