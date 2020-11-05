using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IPdLookUp.Types;
using IPdLookUp.Validators;

namespace IPdLookUp.Entities
{
    public struct LookUpRequest
    {
        [ValidAddress]
        public string Address { get; set; }

        public List<LookUpService>? Services { get; set; }
    }
}