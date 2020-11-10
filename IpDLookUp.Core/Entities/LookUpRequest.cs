using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IpDLookUp.Services.Types;
using IPdLookUp.Core.Validators;

namespace IPdLookUp.Core.Entities
{
    public struct LookUpRequest
    {
        [ValidAddress]
        public string Address { get; set; }

        public List<ServiceType>? Services { get; set; }
    }
}