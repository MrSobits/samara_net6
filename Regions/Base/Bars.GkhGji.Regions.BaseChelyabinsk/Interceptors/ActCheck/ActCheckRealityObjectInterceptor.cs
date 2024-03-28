namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.ActCheck
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    public class ActCheckRealityObjectInterceptor : EmptyDomainInterceptor<ActCheckRealityObject>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ActCheckRealityObject> service, ActCheckRealityObject entity)
        {
            var domainLongText = this.Container.Resolve<IDomainService<ActCheckRoLongDescription>>();
            try
            {
                // Удаляем все дочерние Нарушения акта
                var ids = domainLongText.GetAll()
                    .Where(x => x.ActCheckRo.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                foreach (var id in ids)
                {
                    domainLongText.Delete(id);
                }
            }
            finally
            {
                this.Container.Release(domainLongText);
            }
            return base.BeforeDeleteAction(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<ActCheckRealityObject> service, ActCheckRealityObject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.Id.ToString() + " " + entity.RealityObject.Address);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActCheckRealityObject> service, ActCheckRealityObject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.Id.ToString() + " " + entity.RealityObject.Address);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActCheckRealityObject> service, ActCheckRealityObject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.Id.ToString() + " " + entity.RealityObject.Address);
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
                { "ActCheck", "Акт проверки" },
                { "RealityObject", "Жилой дом" },
                { "Description", "Описание" },
                { "NotRevealedViolations", "Не выявленные нарушения" },
                { "HaveViolation", "Признак выявлено или невыявлено нарушение" },
                { "PersonsWhoHaveViolated", "Сведения о лицах, допустивших нарушения" },
                { "OfficialsGuiltyActions", "Сведения, свидетельствующие, что нарушения допущены в результате виновных действий (бездействия) должностных лиц и (или) работников проверяемого лица" },
            };
            return result;
        }
    }
}