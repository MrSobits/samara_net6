namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Utils;
    using Domain.ImportExport;
    using Wcf.Contracts.PersonalAccount;

    public class FsImportSetupController : BaseController
    {
        public ActionResult GetObjectMeta()
        {
            var resultType = typeof (ImportResult<PersonalAccountPaymentInfoIn>);
            var genericType = typeof (PersonalAccountPaymentInfoIn);

            var meta = resultType.GetProperties().Where(x => x.IsDefined(typeof (DisplayAttribute), true))
                .Select(
                    x => new
                    {
                        PropertyName = x.Name,
                        DisplayName = x.GetAttribute<DisplayAttribute>(true).Value + " ({0})".FormatUsing(x.Name)
                    })
                .Union(
                    genericType.GetProperties().Where(x => x.IsDefined(typeof (DisplayAttribute), true))
                        .Select(
                            x => new
                            {
                                PropertyName = x.Name,
                                DisplayName = x.GetAttribute<DisplayAttribute>(true).Value + " ({0})".FormatUsing(x.Name)
                            })).ToList();

            return new JsonListResult(meta);
        }
    }
}