using System.Collections.Generic;
using System.Text.Json.Serialization;
using IPdLookUp.Types;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IPdLookUp.Entities
{
    public interface IAppResult
    {
        public string Address { get; set; }

        public List<LookUpResult> Results { get; set; }
    }

    public interface IAppErrorResult
    {
        public string ErrorMessage { get; set; }

        public List<LookUpService> FailServices { get; set; }
    }

    public interface ILookUpResult
    {
        public LookUpService Type { get; set; }

        public LookUpStatus Status { get; set; }

        public object? Data { get; set; }

        public string? ErrorMessage { get; set; }
    }

    public struct AppResult : IAppResult
    {
        public string Address { get; set; }

        public List<LookUpResult> Results { get; set; }
    }

    public struct AppErrorResult : IAppErrorResult
    {
        public string ErrorMessage { get; set; }
        public ModelErrorCollection Errors { get; set; }
        
        public List<LookUpService> FailServices { get; set; }
    }

    public struct AppPartialResult : IAppResult, IAppErrorResult
    {
        public string Address { get; set; }

        public List<LookUpResult> Results { get; set; }

        public string ErrorMessage { get; set; }

        public List<LookUpService> FailServices { get; set; }
    }

    public struct LookUpResult : ILookUpResult
    {
        public LookUpService Type { get; set; }

        public LookUpStatus Status { get; set; }

        public object? Data { get; set; }
        
        public string? ErrorMessage { get; set; }
    }
}