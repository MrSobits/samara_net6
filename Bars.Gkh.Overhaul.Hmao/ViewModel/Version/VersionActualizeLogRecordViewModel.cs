namespace Bars.Gkh.Overhaul.Hmao.ViewModel.Version
{
    using System;
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    using Entities.Version;

    /// <summary>
    /// View-модель для <see cref="VersionActualizeLogRecord"/>
    /// </summary>
    public class VersionActualizeLogRecordViewModel : BaseViewModel<VersionActualizeLogRecord>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<VersionActualizeLogRecord> domainService, BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAsId("versionId");
            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            var versionActualizeLogDomain = this.Container.ResolveDomain<VersionActualizeLog>();

            using (this.Container.Using(versionActualizeLogDomain))
            {
                return versionActualizeLogDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId)
                    .WhereIf(dateStart != null, x => x.DateAction.Date >= ((DateTime)dateStart).Date)
                    .WhereIf(dateEnd != null, x => x.DateAction.Date <= ((DateTime)dateEnd).Date)
                    .Join(domainService.GetAll(),
                        x => x.Id,
                        y => y.ActualizeLog.Id,
                        (x, y) => new
                        {
                            ActualizeLog = x,
                            ActualizeLogRecord = y
                        })
                    .Select(x => new
                    {
                        x.ActualizeLogRecord.Id,
                        x.ActualizeLog.DateAction,
                        x.ActualizeLog.ActualizeType,
                        x.ActualizeLogRecord.Action,
                        x.ActualizeLog.InputParams,
                        x.ActualizeLogRecord.RealityObject.Address,
                        x.ActualizeLogRecord.WorkCode,
                        x.ActualizeLogRecord.Ceo,
                        x.ActualizeLogRecord.PlanYear,
                        x.ActualizeLogRecord.ChangePlanYear,
                        x.ActualizeLogRecord.PublishYear,
                        x.ActualizeLogRecord.ChangePublishYear,
                        x.ActualizeLogRecord.Volume,
                        x.ActualizeLogRecord.ChangeVolume,
                        x.ActualizeLogRecord.Sum,
                        x.ActualizeLogRecord.ChangeSum,
                        x.ActualizeLogRecord.Number,
                        x.ActualizeLogRecord.ChangeNumber
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), usePaging: !isExport);
            }
        }
    }
}