namespace Bars.Gkh.Reforma.Tasks
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Reforma.Entities.ChangeTracker;
    using Bars.Gkh.Reforma.PerformerActions.SetHouseProfile;

    /// <summary>
    /// Задача ручного обновления информации по домам
    /// </summary>
    public class SetHouseProfile988Task : BaseManualIntegrationTask<IEnumerable<SetHouseProfileParams>>
    {
        /// <summary>
        /// Выполяемое действие
        /// </summary>
        protected override void Execute()
        {
            foreach (var parameters in this.TaskParams)
            {
                this.Performer.AddToQueue<SetHouseProfile988Action, SetHouseProfileParams, object>().WithParameters(new SetHouseProfileParams
                {
                    RobjectId = parameters.RobjectId,
                    PeriodDiId = parameters.PeriodDiId,
                    ManOrgId = parameters.ManOrgId
                }).WithCallback(result =>
                    {
                        this.Container.InTransaction(() =>
                        {
                            this.Container.UsingForResolved<IDomainService<ChangedRobject>>(
                                (container, service) =>
                                {
                                    long roId;
                                    if (long.TryParse(result.ToString(), out roId))
                                    {
                                        service.GetAll()
                                        .Where(x => x.RealityObject.Id == roId && x.PeriodDi.Id == parameters.PeriodDiId)
                                        .ForEach(x => service.Delete(x.Id));
                                    }
                                });
                        });
                    });
            }

            this.Performer.Perform();
        }

        /// <summary>
        /// Извлечь параметры задачи
        /// </summary>
        /// <param name="params">Словарь параметров</param>
        /// <returns>Параметры</returns>
        protected override IEnumerable<SetHouseProfileParams> ExtractParamsFromArgs(DynamicDictionary @params)
        {
            var periodDiId = @params.GetAsId("PeriodDiId");
            var manOrgId = @params.GetAsId("ManOrgId");

            return @params.GetAs<IList<long>>("RealityObjectIds")
                .Select(x => new SetHouseProfileParams { PeriodDiId = periodDiId, RobjectId = x, ManOrgId = manOrgId});
        }
    }
}