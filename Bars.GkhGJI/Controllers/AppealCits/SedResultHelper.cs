namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Utils;

    public static class SedResultHelper
    {
        public static ActionResult ToSedError(this string message)
        {
            return new JsonNetResult("<appeal><error>{0}</error></appeal>".FormatUsing(message));
        }

        public static ActionResult ToSedSuccess(this long id)
        {
            return new JsonNetResult("<id_sed>{0}</id_sed>".FormatUsing(id));
        }
    }
}