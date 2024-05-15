using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.JsonResponseModels
{
    public class CollectionJsonResponse<TRecord> : BaseJsonResponse where TRecord : class
    {
        public IEnumerable<TRecord> Items { get; set; }
    }
}