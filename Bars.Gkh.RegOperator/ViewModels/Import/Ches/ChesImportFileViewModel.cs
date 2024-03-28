namespace Bars.Gkh.RegOperator.ViewModels.Import
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.Utils;

    public class ChesImportFileViewModel : BaseViewModel<ChesImportFile>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ChesImportFile> domainService, BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAs<long>("periodId");
            var loadParams = baseParams.GetLoadParam();

            return domainService.GetAll()
                .WhereIf(periodId != 0, x => x.ChesImport.Period.Id == periodId)
                .ToListDataResult(loadParams, this.Container);
        }
    }
}