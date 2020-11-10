using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IpDLookUp.Services.Types;
using IPdLookUp.Core.Validators;

namespace IPdLookUp.Core.Entities
{
    /// <summary>
    /// Model for incoming request with custom validator for Address
    /// </summary>
    public struct LookUpRequest
    {
        [ValidAddress]
        public string Address { get; set; }

        public List<ServiceType>? Services { get; set; }
    }
}