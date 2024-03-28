namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructionObjectTypeWorkViewModel : BaseViewModel<ConstructionObjectTypeWork>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<ConstructionObjectTypeWork> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var objectId = baseParams.Params.GetAsId("objectId");

            var query =
                domainService.GetAll()
                             .Where(x => x.ConstructionObject.Id == objectId)
                             .Select(
                                 x =>
                                 new
                                     {
                                         x.Id,
                                         x.ConstructionObject,
										 WorkName = x.Work.Name,
										 TypeWork = x.Work != null ? x.Work.TypeWork : TypeWork.NotSet,
										 UnitMeasureName = x.Work.UnitMeasure.Name,
										 x.HasExpertise,
                                         x.HasPsd,
                                         x.Volume,
                                         x.Sum,
                                         x.CountWorker,
                                         x.VolumeOfCompletion,
                                         x.PercentOfCompletion,
                                         x.CostSum,
                                         x.DateStartWork,
                                         x.DateEndWork,
										 x.Deadline
                                     })
                             .Filter(loadParams, this.Container);

            return new ListDataResult(query.Order(loadParams).Paging(loadParams), query.Count());
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<ConstructionObjectTypeWork> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var entity = domainService.Get(id);
            if (entity == null)
            {
                return BaseDataResult.Error("Не найден вид работы");
            }

            return
                new BaseDataResult(
                    new
                        {
                            entity.Id,
                            entity.ConstructionObject,
                            entity.Work,
							TypeWork = entity.Work.Return(x => x.TypeWork),
                            UnitMeasureName = entity.Work.Return(x => x.UnitMeasure).Return(x => x.Name),
                            entity.YearBuilding,
                            entity.HasExpertise,
                            entity.HasPsd,
                            entity.Volume,
                            entity.Sum,
                            entity.Description,
                            entity.VolumeOfCompletion,
                            entity.PercentOfCompletion,
                            entity.CostSum,
                            entity.DateStartWork,
                            entity.DateEndWork,
							entity.Deadline
                        });
        }
    }
}