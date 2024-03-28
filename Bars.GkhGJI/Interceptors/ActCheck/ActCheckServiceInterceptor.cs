namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActCheckServiceInterceptor : ActCheckServiceInterceptor<ActCheck>
    {
        // Внимание !!! override делать в Generic классе
    }
    
    public class ActCheckServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : ActCheck
    {
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            var serviceNumRule = this.Container.Resolve<IDomainService<DocNumValidationRule>>();
            var domainDocumentRef = this.Container.Resolve<IDomainService<DocumentGjiReference>>();
            var domainDocumentGji = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var actRoDomain = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();

            using (this.Container.Using(serviceNumRule, domainDocumentGji, domainDocumentRef, actRoDomain, roDomain))
            {
                //проверяем чтоб дата акта была не меньше даты распоряжения по которому создан данный акт
                var parentDisposal = domainDocumentGji
                    .GetAll()
                    .Where(x => x.Children.Id == entity.Id)
                    .Select(x => new { x.Parent.Id, x.Parent.DocumentDate })
                    .ToList();

                if (parentDisposal.Any(item => item.DocumentDate.HasValue && entity.DocumentDate.HasValue && item.DocumentDate.Value > entity.DocumentDate.Value))
                {
                    return this.Failure(
                        $"Дата акта проверки должна быть больше или равна дате родительского {this.Container.Resolve<IDisposalText>().GenetiveCase.ToLower()}");
                }

                // перед обновлением сохраняем связь с Постановлением прокуратуры

                if (entity.ResolutionProsecutor != null)
                {
                    var docResolutionProsecutor = domainDocumentRef.GetAll()
                        .FirstOrDefault(x => x.TypeReference == TypeDocumentReferenceGji.ActCheckToProsecutor
                            && (x.Document1.Id == entity.Id || x.Document2.Id == entity.Id));

                    if (docResolutionProsecutor == null)
                    {
                        // Если связи небыло, в обновляемом объекте есть документ прокуратуры, то создаем связб
                        var newRef = new DocumentGjiReference
                        {
                            TypeReference = TypeDocumentReferenceGji.ActCheckToProsecutor,
                            Document1 = entity,
                            Document2 = entity.ResolutionProsecutor
                        };

                        domainDocumentRef.Save(newRef);
                    }
                    else
                    {
                        // Если связь уже существует и также пришел документ в обновляемой сущности то перезаписываем
                        docResolutionProsecutor.Document1 = entity;
                        docResolutionProsecutor.Document2 = entity.ResolutionProsecutor;

                        domainDocumentRef.Update(docResolutionProsecutor);
                    }
                }

                // проверяем изменилась ли площадь по указанным домам
                var roIds =
                    actRoDomain.GetAll()
                               .Where(x => x.ActCheck.Id == entity.Id)
                               .Where(x => x.RealityObject != null)
                               .Select(x => x.RealityObject.Id)
                               .Distinct()
                               .ToList();

                decimal? area = 0m;
                
                if (roIds.Any())
                {
                    area = roDomain.GetAll().Where(x => roIds.Contains(x.Id)).Sum(x => x.AreaMkd);
                }

                if (entity.Area != area && area > 0)
                {
                    entity.Area = area;
                }

                return base.BeforeUpdateAction(service, entity);
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var annexService = this.Container.Resolve<IDomainService<ActCheckAnnex>>();
            var definitionService = this.Container.Resolve<IDomainService<ActCheckDefinition>>();
            var partService = this.Container.Resolve<IDomainService<ActCheckInspectedPart>>();
            var witnessService = this.Container.Resolve<IDomainService<ActCheckWitness>>();
            var domainChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var domainServiceActRemoval = this.Container.Resolve<IDomainService<ActRemoval>>();
            var domainServiceObject = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var actPeriodService = this.Container.Resolve<IDomainService<ActCheckPeriod>>();
            var actCheckProvidedDocDomainService = this.Container.Resolve<IDomainService<ActCheckProvidedDoc>>();

            using (this.Container.Using(annexService,
                       definitionService,
                       partService,
                       witnessService,
                       domainChildren,
                       domainServiceActRemoval,
                       domainServiceObject,
                       actPeriodService,
                       actCheckProvidedDocDomainService))
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return this.Failure(result.Message);
                }

                var refFuncs = new List<Func<long, string>>
                {
                    id => annexService.GetAll().Any(x => x.ActCheck.Id == id) ? "Приложения" : null,
                    id => definitionService.GetAll().Any(x => x.ActCheck.Id == id) ? "Определение" : null,
                    id => partService.GetAll().Any(x => x.ActCheck.Id == id) ? "Инспектируемые части" : null,
                    id => witnessService.GetAll().Any(x => x.ActCheck.Id == id) ? "Лица, присутсвующие в акте" : null
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + $" {str}; ");
                    message = $"Существуют связанные записи в следующих таблицах: {message}";
                    return this.Failure(message);
                }

                // удаляем дочерние акты
                domainChildren.GetAll()
                        .Where(x => x.Parent.Id == entity.Id && x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                        .Select(x => x.Children.Id)
                        .ForEach(x => domainServiceActRemoval.Delete(x));

                // Удаляем все дочерние Дома акта
                domainServiceObject.GetAll()
                    .Where(x => x.ActCheck.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => domainServiceObject.Delete(x));

                //удаляем даты/время проведения проверки
                actPeriodService.GetAll()
                    .Where(x => x.ActCheck.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => actPeriodService.Delete(x));

                //Удаление сущности "Предоставленные документы акта проверки ГЖИ"
                actCheckProvidedDocDomainService.GetAll()
                    .Where(x => x.ActCheck.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => actCheckProvidedDocDomainService.Delete(x));

                return result;
            }
        }

        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.Id.ToString());
            }
            catch
            {

            }

            return base.AfterCreateAction(service, entity);
        }

        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }

            return base.AfterUpdateAction(service, entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }

            return base.AfterDeleteAction(service, entity);
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "Inspection", "Проверка ГЖИ" },
                { "Stage", "Этап проверки" },
                { "TypeDocumentGji", "Тип документа ГЖИ" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNumber", "Номер документа" },
                { "LiteralNum", "Буквенный подномер" },
                { "DocumentYear", "Год документа" },
                { "State", "Статус" },
                { "Area", "Проверяемая площадь" },
                { "DateToProsecutor", "Дата передачи" },
                { "TypeActCheck", "Тип акта" },
                { "TypeCheckAct", "Тип акта 2" },
                { "Flat", "Квартира" },
                { "ActToPres", "Акт направлен в прокуратуру" },
                { "Unavaliable", "Невозможно провести проверку" },
                { "UnavaliableComment", "Причина невозможности проведения проверки" },
                { "DocumentPlace", "Место составления" },
                { "DocumentTime", "Время составления акта" },
                { "AcquaintState", "Статус ознакомления с результатами проверки" },
                { "RefusedToAcquaintPerson", "ФИО должностного лица, отказавшегося от ознакомления с актом проверки" },
                { "AcquaintedPerson", "ФИО должностного лица, ознакомившегося с актом проверки" },
                { "AcquaintedDate", "Дата ознакомления" },
                { "SignatoryInspector", "Инспектор подписавший акт проверки" }
            };
            return result;
        }
    }
}