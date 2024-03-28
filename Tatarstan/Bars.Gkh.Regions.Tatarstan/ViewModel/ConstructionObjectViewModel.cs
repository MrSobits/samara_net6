namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities;
    using Utils;

    /// <summary>
    /// ViewModel для <see cref="ConstructionObject"/>
    /// </summary>
    public class ConstructionObjectViewModel : BaseViewModel<ConstructionObject>
    {
        /// <summary>
        /// Получить список <see cref="ConstructionObject"/>
        /// </summary>
        /// <param name="domain">Домен сервис</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult List(IDomainService<ConstructionObject> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var municipalityId = baseParams.Params.GetAs("municipalityId", string.Empty);
            var municipalityIds = !string.IsNullOrEmpty(municipalityId) ? municipalityId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var resettlementProgramIds = baseParams.Params.GetAs<string>("resettlementProgramId").ToLongArray();
            var stateId = baseParams.Params.GetAs<long>("stateId");

            var smrDomain = this.Container.Resolve<IDomainService<ConstructObjMonitoringSmr>>();

            using (this.Container.Using(smrDomain))
            {
                var data = domain.GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id) || municipalityIds.Contains(x.MoSettlement.Id))
                    .WhereIf(resettlementProgramIds.Length > 0, x => resettlementProgramIds.Contains(x.ResettlementProgram.Id))
                    .WhereIf(stateId > 0, x => x.State.Id == stateId)
                    .Select(
                        x => new
                        {
                            x.Id,
                            Municipality = x.Municipality.Name,
                            Settlement = x.MoSettlement.Name,
                            ResettlementProgram = x.ResettlementProgram.Name,
                            x.Address,
                            x.State,
                            MonitoringSmrId = smrDomain.GetAll().Where(z => z.ConstructionObject.Id == x.Id).Select(z => z.Id).FirstOrDefault(),
                            MonitoringSmrState = smrDomain.GetAll().Where(z => z.ConstructionObject.Id == x.Id).Select(z => z.State).FirstOrDefault()
                        })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                data = data.Order(loadParams).Paging(loadParams);

                return new ListDataResult(data.ToList(), totalCount);
            }
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<ConstructionObject> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("Id");
            var record = domainService.Get(id);

            return new BaseDataResult(
                new
                {
                    record.Id,
                    record.Municipality,
                    record.MoSettlement,
                    FiasAddress = record.FiasAddress?.GetFiasProxy(this.Container),
                    record.Address,
                    record.Description,
                    record.State,
                    record.SumSmr,
                    record.SumDevolopmentPsd,
                    record.DateEndBuilder,
                    record.DateStartWork,
                    record.DateStopWork,
                    record.DateResumeWork,
                    record.ReasonStopWork,
                    record.DateCommissioning,
                    record.LimitOnHouse,
                    record.TotalArea,
                    record.NumberApartments,
                    record.NumberEntrances,
                    record.ResettleProgNumberApartments,
                    record.NumberFloors,
                    record.NumberLifts,
                    record.RoofingMaterial,
                    record.TypeRoof,
                    record.WallMaterial,
                    record.ResettlementProgram
                });
        }
    }
}