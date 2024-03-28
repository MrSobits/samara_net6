namespace Bars.Gkh.ImportExport
{
    using System.Linq;

    using Bars.B4;

    public class ImportExportViewModel : BaseViewModel<ImportExport>
    {
        public override IDataResult List(IDomainService<ImportExport> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = domainService.GetAll().Select(x => new
            {
                x.Id,
                x.Type,
                x.DateStart,
                x.FileInfo,
                HasErrors = x.HasErrors ? "Да" : "Нет",
                HasMessages = x.HasMessages ? "Да" : "Нет"
            }).OrderByDescending(x => x.DateStart).Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}