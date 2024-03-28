namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;

    /// <summary>
    /// Представление <see cref="LogImport" />
    /// </summary>
    public class LogImportViewModel : BaseViewModel<LogImport>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<LogImport> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var service = this.Container.Resolve<IDomainService<LogImport>>();

            var importProvider = this.Container.Resolve<IGkhImportService>();
            var items = importProvider.GetImportInfoList(baseParams)
                .GroupBy(x => x.Key, x => x.Name)
                .ToDictionary(x => x.Key, x => x.First());

            var data = service.GetAll()
                .Where(x => x.LogFile != null)
                .Select(
                    x => new
                    {
                        x.Id,
                        Operator = x.Operator.User.Login ?? x.Login,
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
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return
                new ListDataResult(
                    data
                        .AsEnumerable()
                        .Select(
                            x => new
                            {
                                x.Id,
                                x.Operator,
                                x.UploadDate,
                                x.FileName,
                                ImportKey = items.ContainsKey(x.ImportKey) ? items[x.ImportKey] : string.Empty,
                                x.CountWarning,
                                x.CountError,
                                x.CountImportedRows,
                                x.CountChangedRows,
                                x.CountImportedFile,
                                x.File,
                                x.LogFile
                            })
                        .ToList(),
                    totalCount);
        }
    }
}