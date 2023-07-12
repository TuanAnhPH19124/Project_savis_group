using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalApi.DataTransfer
{
    public class DynamicParamsDto
    {
        public bool? Wifi { get; set; } = null;
        public bool? Laundry { get; set; } = null;
        public string? City { get; set; } = string.Empty;
        public string? District { get; set; } = string.Empty;
        public string? Ward { get; set; } = string.Empty;
        public double? LowPrice { get; set; } = 0;
        public double? HighPrice {get;set;} = 0;
        
    }
}