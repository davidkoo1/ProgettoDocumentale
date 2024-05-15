using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.JsonResponseModels
{
    public class SingleJsonResponse<TRecord> : BaseJsonResponse where TRecord : class
    {
        public TRecord Item { get; set; }
    }
}