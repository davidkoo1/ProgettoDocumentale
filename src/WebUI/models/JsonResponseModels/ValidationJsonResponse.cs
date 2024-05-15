using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.JsonResponseModels
{
    public class ValidationJsonResponse : BaseJsonResponse
    {
        public Dictionary<string, string> Errors { get; set; }
    }
}