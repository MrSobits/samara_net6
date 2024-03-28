namespace Bars.GkhGji.Regions.Tatarstan.SchedulerTasks
{
    using System;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    using Castle.MicroKernel.Lifestyle;

    /// <summary>
    /// Действие по обновлению статусов обращений в СОПР с истекшим контрольным сроком
    /// </summary>
    public class UpdateRapidResponseSystemApealStateTask : BaseTask, ITask<DynamicDictionary>
    {
        /// <inheritdoc />
        public override void Execute(DynamicDictionary @params)
        {
            using (this.Container.BeginScope())
            {
                var rapidResponseSystemAppealDetailsDomain = this.Container.ResolveDomain<RapidResponseSystemAppealDetails>();
                var stateDoamin = this.Container.ResolveDomain<State>();

                using (this.Container.Using(rapidResponseSystemAppealDetailsDomain, stateDoamin))
                {
                    var appealsToUpdateStates = rapidResponseSystemAppealDetailsDomain.GetAll()
                        .Where(x => !x.State.FinalState)
                        .Where(x => x.ControlPeriod < DateTime.Today)
                        .ToList();

                    if (!appealsToUpdateStates.Any())
                    {
                        return;
                    }
                    
                    var typeId = appealsToUpdateStates.First().State.TypeId;
                    var unprocessedState = stateDoamin.FirstOrDefault(x => x.Code == "4" && x.TypeId == typeId);

                    using (var transaction = this.Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            appealsToUpdateStates.ForEach(x =>
                            {
                                x.State = unprocessedState;

                                rapidResponseSystemAppealDetailsDomain.Update(x);
                            });
                            
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            
                            throw;
                        }
                    }
                }
            };
        }
    }
}