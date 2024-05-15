using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Models.Enum;

namespace WebUI.Models.JsonResponseModels
{
    public class ExceptionJsonResponse : BaseJsonResponse
    {

        public override ExecutionResult Result => ExecutionResult.EXCEPTION;
        public string StackTrace { get; set; }
        public IDictionary<string, string[]> Failures { get; set; }
    }
}