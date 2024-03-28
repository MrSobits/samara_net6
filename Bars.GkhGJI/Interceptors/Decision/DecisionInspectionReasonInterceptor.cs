namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;

    public class DecisionInspectionReasonInterceptor : EmptyDomainInterceptor<DecisionInspectionReason>
    {
        public override IDataResult BeforeCreateAction(IDomainService<DecisionInspectionReason> service, DecisionInspectionReason entity)
        {
            var serviceAppealCits = Container.Resolve<IDomainService<InspectionAppealCits>>();
            try
            {
                if (string.IsNullOrEmpty(entity.Description))
                {
                    string baseName = "сведениями, изложенными в обращении граждан ";

                    // Получаем из основания наименование плана
                    var baseStatement = serviceAppealCits
                        .GetAll()
                            .Where(x => x.Inspection.Id == entity.Decision.Inspection.Id)
                            .Select(x => (x.AppealCits.DocumentNumber != ""? x.AppealCits.DocumentNumber : x.AppealCits.NumberGji) + (x.AppealCits.DateFrom.HasValue ? " от " + x.AppealCits.DateFrom.Value.ToString("dd.MM.yyyy") + " г." : ""))
                            .AggregateWithSeparator(", ");

                    if (!string.IsNullOrWhiteSpace(baseStatement))
                    {
                        baseName += string.Format("№ {0}", baseStatement);
                    }
                    entity.Description = baseName;
                }
            }
            finally
            {
                Container.Release(serviceAppealCits);
            }

            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<DecisionInspectionReason> service, DecisionInspectionReason entity)
        {
            if (entity.Description == "" || entity.Description == null)
            {
                var serviceAppealCits = Container.Resolve<IDomainService<InspectionAppealCits>>();
                var serviceDecisionInspectionReason = Container.Resolve<IDomainService<DecisionInspectionReason>>();
                if (entity.InspectionReason == null)
                {
                    var ir = serviceDecisionInspectionReason.Get(entity.Id).InspectionReason;
                    entity.InspectionReason = new Entities.Dict.InspectionReason {Id = ir.Id };
                }                
                try
                {
                    string baseName = "сведениями, изложенными в обращении граждан ";

                    // Получаем из основания наименование плана
                    var baseStatement = serviceAppealCits
                        .GetAll()
                            .Where(x => x.Inspection.Id == entity.Decision.Inspection.Id)
                            .Select(x => x.AppealCits.DocumentNumber + " от " + x.AppealCits.DateFrom.Value.ToString("dd.MM.yyyy"))
                            .AggregateWithSeparator(", ");

                    if (!string.IsNullOrWhiteSpace(baseStatement))
                    {
                        baseName += string.Format("№ {0}", baseStatement);
                    }
                    entity.Description = baseName;
                }
                finally
                {
                    Container.Release(serviceAppealCits);
                    Container.Release(serviceDecisionInspectionReason);
                }
            }

            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<DecisionInspectionReason> service, DecisionInspectionReason entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DecisionInspectionReason, entity.Id, entity.GetType(), GetPropertyValues(), (entity.Decision.Id.ToString() + " " + entity.InspectionReason.Name).Substring(0, 150));
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<DecisionInspectionReason> service, DecisionInspectionReason entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DecisionInspectionReason, entity.Id, entity.GetType(), GetPropertyValues(), (entity.Decision.DocumentNumber + " " + entity.InspectionReason.Name).Substring(0, 150));
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DecisionInspectionReason> service, DecisionInspectionReason entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DecisionInspectionReason, entity.Id, entity.GetType(), GetPropertyValues(), (entity.Decision.DocumentNumber + " " + entity.InspectionReason.Name).Substring(0, 150));
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "Decision", "Распоряжение" },
                { "InspectionReason", "Мероприятие по контролю" },
                { "Description", "Описание мериприятия по контролю" }
            };
            return result;
        }
    }
}