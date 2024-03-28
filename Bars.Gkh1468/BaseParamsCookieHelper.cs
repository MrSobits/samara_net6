using System.Linq;
using Bars.B4;
using Bars.B4.Utils;

namespace Bars.Gkh1468.DomainService
{
    public static class BaseParamsCookieHelper
    {
        public static string GetCookie(this BaseParams baseParams, string cookieName)
        {
            var cookieString = baseParams.Params.GetAs<string>("HTTP_COOKIE");
            var result = string.Empty;

            if (!string.IsNullOrEmpty(cookieString))
            {
                var items = cookieString.Split(";");
                if (items.Length > 0)
                {
                    foreach (var item in items)
                    {
                        var keyValue = item.Split("=");
                        if (keyValue.Length == 2 && keyValue[0].Trim() == cookieName)
                        {
                            result = keyValue[1];
                            break;    
                        }
                    }
                }
            }

            return result;
        }
    }
}