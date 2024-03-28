namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    public class ActIsolatedInterceptor : DocumentGjiInterceptor<ActIsolated>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActIsolated> service, ActIsolated entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DocumentPlaceFias);

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ActIsolated> service, ActIsolated entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DocumentPlaceFias);

            return base.BeforeUpdateAction(service, entity);
        }
        
        public override IDataResult BeforeDeleteAction(IDomainService<ActIsolated> service, ActIsolated entity)
        {
            var annexService = this.Container.Resolve<IDomainService<ActIsolatedAnnex>>();
            var definitionService = this.Container.Resolve<IDomainService<ActIsolatedDefinition>>();
            var partService = this.Container.Resolve<IDomainService<ActIsolatedInspectedPart>>();
            var witnessService = this.Container.Resolve<IDomainService<ActIsolatedWitness>>();
            var providedDocService = this.Container.Resolve<IDomainService<ActIsolatedProvidedDoc>>();
            var domainChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var domainServiceObject = this.Container.Resolve<IDomainService<ActIsolatedRealObj>>();
            var actPeriodService = this.Container.Resolve<IDomainService<ActIsolatedPeriod>>();

            try
            {

                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return this.Failure(result.Message);
                }

                var refFuncs = new List<Func<long, string>>
                {
                    id => annexService.GetAll().Any(x => x.ActIsolated.Id == id) ? "Приложения" : null,
                    id => definitionService.GetAll().Any(x => x.ActIsolated.Id == id) ? "Определение" : null,
                    id => partService.GetAll().Any(x => x.ActIsolated.Id == id) ? "Инспектируемые части" : null,
                    id => witnessService.GetAll().Any(x => x.ActIsolated.Id == id) ? "Лица, присутсвующие припроверке" : null,
                    id => providedDocService.GetAll().Any(x => x.ActIsolated.Id == id) ? "Предоставленные документы" : null
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + $" {str}; ");
                    message = $"Существуют связанные записи в следующих таблицах: {message}";
                    return this.Failure(message);
                }

                // Удаляем все дочерние Дома акта
                var objectIds = domainServiceObject.GetAll()
                    .Where(x => x.ActIsolated.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                foreach (var objId in objectIds)
                {
                    domainServiceObject.Delete(objId);
                }

                //удаляем даты/время проведения проверки
                actPeriodService.GetAll()
                    .Where(x => x.ActIsolated.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => actPeriodService.Delete(x));

                return result;
            }
            finally 
            {
                this.Container.Release(annexService);
                this.Container.Release(definitionService);
                this.Container.Release(partService);
                this.Container.Release(witnessService); 
                this.Container.Release(providedDocService);
                this.Container.Release(domainChildren);
                this.Container.Release(domainServiceObject);
                this.Container.Release(actPeriodService);
            }
            
        }
    }
}