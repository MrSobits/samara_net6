namespace Bars.Gkh.RegOperator.ViewModels.Import
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Imports.Ches;
    using Bars.Gkh.Utils;

    using ChesImport = Bars.Gkh.RegOperator.Entities.Import.Ches.ChesImport;

    public class ChesImportViewModel : BaseViewModel<ChesImport>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ChesImport> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var listDataResult = domainService.GetAll()
                .Select(x => new
                {
                    x.Period.Id,
                    Period = x.Period.Name,
                    x.ObjectEditDate.Date,
                    x.State,
                    x.AnalysisState,
                    User = x.User.Login,
                    Task = (int?)x.Task.Percentage,
                    x.LoadedFiles
                })
                .ToListDataResult(loadParams, this.Container);

            return listDataResult;
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<ChesImport> domainService, BaseParams baseParams)
        {
            var entity = domainService.Get(baseParams.Params.GetAsId());

            if (entity.IsNull())
            {
                var periodId = baseParams.Params.GetAsId("periodId");
                entity = domainService.GetAll().SingleOrDefault(x => x.Period.Id == periodId);
            }

            if (entity.IsNull())
            {
                return BaseDataResult.Error("Не найдена запись");
            }

            entity.LoadedFiles.Remove(FileType.Pay);

            return new BaseDataResult(new
            {
                entity.Id,
                Period = entity.Period.Id,
                entity.LoadedFiles
            });
        }
    }
}