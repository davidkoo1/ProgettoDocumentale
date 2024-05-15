using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Models.Enum;

namespace WebUI.Models.JsonResponseModels
{
    public class BaseJsonResponse
    {
        public virtual ExecutionResult Result { get; set; }
        public string Message { get; set; }
        public bool ShowToast { get; set; }
    }
}