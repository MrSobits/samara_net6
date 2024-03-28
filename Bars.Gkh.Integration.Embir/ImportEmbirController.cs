using System.Collections;
using System.Linq;
using System.Web.Mvc;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.Gkh.Entities;
using Bars.Gkh.Import;

namespace Bars.Gkh.Integration.Embir
{
    public class ImportEmbirController : BaseController
    {
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var logImportDomain = Container.ResolveDomain<LogImport>();
            var importKeys =
                Container.ResolveAll<IGkhImport>().Where(x => x.CodeImport == "ImportEmbir").Select(x => x.Key).ToList();
            try
            {
                var logs = logImportDomain.GetAll()
                    .Where(x => importKeys.Contains(x.ImportKey))
                    .AsEnumerable()
                    .GroupBy(x => x.ImportKey)
                    .ToDictionary(x => x.Key, y => y.SafeMax(x => x.UploadDate));

                return new JsonNetResult(logs);
            }
            finally
            {
                Container.Release(logImportDomain);
            }

        }

        public ActionResult ListLog(BaseParams baseParams)
        {
            var logImportDomain = Container.ResolveDomain<LogImport>();
            var importProvider = Container.Resolve<IGkhImportService>();
            var items = importProvider.GetImportList(baseParams)
                    .Where(x => x.CodeImport == "ImportEmbir")
                    .GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.First().Name);
            try
            {
                var loadParams = baseParams.GetLoadParam();

                var data = logImportDomain.GetAll()
                              .Where(x => items.Keys.Contains(x.ImportKey))
                              .Select(x => new
                              {
                                  x.Id,
                                  Operator = x.Operator.User.Login ?? string.Empty,
                                  x.UploadDate,
                                  x.FileName,
                                  x.ImportKey,
                                  x.CountWarning,
                                  x.CountError,
                                  x.CountImportedRows,
                                  x.CountChangedRows,
                                  x.CountImportedFile,
                                  x.File,
                                  x.LogFile
                              })
                              .OrderIf(loadParams.Order.Length == 0, false, x => x.UploadDate)
                              .Filter(loadParams, Container);

                var totalCount = data.Count();

                data = data.Order(loadParams).Paging(loadParams);

                var result = data
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.Id,
                            x.Operator,
                            x.UploadDate,
                            x.FileName,
                            ImportKey = items.ContainsKey(x.ImportKey) ? items[x.ImportKey] : string.Empty,
                            x.CountWarning,
                            x.CountError,
                            CountImportedRows = x.CountImportedRows + x.CountChangedRows,
                            x.CountChangedRows,
                            x.CountImportedFile,
                            x.File,
                            x.LogFile
                        })
                        .ToList();

                return new JsonListResult(result.ToList(), totalCount);
            }
            finally
            {
                Container.Release(logImportDomain);
                Container.Release(importProvider);
            }     
        }
    }
}