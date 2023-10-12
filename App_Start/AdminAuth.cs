using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Food.App_Start
{
    public class AdminAuth : AuthorizeAttribute
    {
        public int MaCV { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //check session
            User userSession = (User)HttpContext.Current.Session["user"];

            if (userSession != null)
            {
                #region Check quyền

                DBShopeeFoodEntities db = new DBShopeeFoodEntities();

                var phanQuyen = db.PhanQuyens.Count(n => n.MaTK == userSession.MaTK && n.MaCV == MaCV);
                if (phanQuyen != 0)
                {
                    return;
                }
                else
                {
                    var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                    filterContext.Result = new RedirectToRouteResult(
                                           new System.Web.Routing.RouteValueDictionary(new
                                           {
                                               controller = "Error",
                                               action = "KoCoQuyen",
                                               Area = "Admin",
                                               returnUrl = returnUrl.ToString()
                                           }));
                }

                #endregion Check quyền

                return;
            }
            else
            {
                var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new
                    {
                        controller = "AdHome",
                        action = "Login",
                        Area = "Admin",
                        returnUrl = returnUrl.ToString()
                    }));
            }
        }
    }
}