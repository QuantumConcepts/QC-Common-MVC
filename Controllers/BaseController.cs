using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using QuantumConcepts.Common.Mvc.Models;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Mvc.Controllers
{
    public class BaseController : Controller
    {
        public const string Key = "Controller";

        protected readonly ILog Logger = null;

        public BaseController()
        {
            this.Logger = LogManager.GetLogger(this.GetType());
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (this.HttpContext != null)
                this.HttpContext.Items.Add(BaseController.Key, this);

            base.OnException(filterContext);
        }

        protected ActionResult RedirectToReturnUrlOrDefault(BaseModel model, string defaultAction) {
            return RedirectToReturnUrlOrDefault(model.ValueOrDefault(o => o.ReturnUrl), defaultAction);
        }

        protected ActionResult RedirectToReturnUrlOrDefault(string returnUrl, string defaultAction) {
            return RedirectToReturnUrlOrDefault(returnUrl, this.RedirectToAction(defaultAction));
        }

        protected ActionResult RedirectToReturnUrlOrDefault(BaseModel model, ActionResult defaultAction) {
            return RedirectToReturnUrlOrDefault(model.ValueOrDefault(o => o.ReturnUrl), defaultAction);
        }

        protected ActionResult RedirectToReturnUrlOrDefault(string returnUrl, ActionResult defaultAction) {
            return RedirectToReturnUrlOrDefault(returnUrl, () => defaultAction);
        }

        protected ActionResult RedirectToReturnUrlOrDefault(BaseModel model, Func<ActionResult> defaultAction) {
            return RedirectToReturnUrlOrDefault(model.ValueOrDefault(o => o.ReturnUrl), defaultAction);
        }

        protected ActionResult RedirectToReturnUrlOrDefault(string returnUrl, Func<ActionResult> defaultAction) {
            if (!returnUrl.IsNullOrEmpty())
                return this.Redirect(returnUrl);

            return defaultAction();
        }
    }
}