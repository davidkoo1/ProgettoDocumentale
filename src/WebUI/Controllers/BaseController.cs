using System.Collections.Generic;
using System.Linq;
using System;
using System.Web.Mvc;
using WebUI.Models.JsonResponseModels;
using WebUI.Models.Enum;


namespace WebUI.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected virtual ActionResult CreateJsonOk(string message = null, bool showToast = false)
        {
            return Json(new BaseJsonResponse { Result = ExecutionResult.OK, Message = message, ShowToast = showToast });
        }

        protected virtual ActionResult CreateJsonOk<T>(T record, string message = null, bool showToast = false) where T : class
        {
            return Json(new SingleJsonResponse<T> { Result = ExecutionResult.OK, Message = message, ShowToast = showToast, Item = record });
        }

        protected virtual ActionResult CreateJsonOk<T>(IEnumerable<T> records, string message = null, bool showToast = false) where T : class
        {
            return Json(new CollectionJsonResponse<T> { Result = ExecutionResult.OK, Message = message, ShowToast = showToast, Items = records });
        }

        protected virtual ActionResult CreateJsonKo(string message = null)
        {
            return Json(new BaseJsonResponse { Result = ExecutionResult.KO, Message = message });
        }

        //protected virtual ActionResult CreateJsonNotValid(ModelStateDictionary keyValuePairs, bool showToast = false)
        //{
        //    Dictionary<string, string> errors = new();

        //    foreach (var item in keyValuePairs)
        //    {
        //        if (item.Value.ValidationState == ModelValidationState.Invalid)
        //        {
        //            errors.Add(item.Key, string.Join("\n", item.Value.Errors.Select(x => x.ErrorMessage)));
        //        }
        //    }

        //    return Json(new ValidationJsonResponse { Result = ExecutionResult.NOTVALID, Message = "One or more validation errors occurred.", Errors = errors, ShowToast = showToast });
        //}

        protected virtual ActionResult CreateJsonError(string message = null)
        {
            return Json(new BaseJsonResponse { Result = ExecutionResult.ERROR, Message = message });
        }

        protected virtual ActionResult CreateJsonException(Exception exception)
        {
            return Json(new ExceptionJsonResponse { Message = exception.Message, StackTrace = exception.StackTrace });
        }

    }
}
