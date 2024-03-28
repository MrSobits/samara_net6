namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolProsServiceInterceptor : ResolProsServiceInterceptor<ResolPros>
    {
    }

    public class ResolProsServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : ResolPros
    {
        private long inspectionId { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var domainServiceInspection = Container.Resolve<IDomainService<BaseProsResol>>();
            var domainStage = Container.Resolve<IDomainService<InspectionGjiStage>>();

            try
            {
                // Также перед созданием мы создаем Проверку Поскольку Документы ГЖИ без основания немогут быть
                // И дерево Этапов также настроено на inspectionGJI поэтому скрыто от глаз пользователей создаем основание
                // Проверки с типом постановление прокуратуры
                var newInspection = new BaseProsResol
                {
                    TypeBase = TypeBase.ProsecutorsResolution
                };

                domainServiceInspection.Save(newInspection);

                // также перед созданием мы создаем этап 
                var newStage = new InspectionGjiStage
                {
                    Inspection = newInspection,
                    TypeStage = TypeStage.ResolutionProsecutor,
                    Position = 0
                };

                domainStage.Save(newStage);

                entity.Inspection = newInspection;
                entity.Stage = newStage;

                return base.BeforeCreateAction(service, entity);
            }
            finally
            {
                Container.Release(domainServiceInspection);
                Container.Release(domainStage);
            }

        }

        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            // перед обновлением сохраняем или удаляем связь с Актом проверки который передан в прокуратуруы
            var domainDocumentRef = Container.Resolve<IDomainService<DocumentGjiReference>>();

            try
            {
                var docActCheck = domainDocumentRef.GetAll()
                        .FirstOrDefault(x => x.TypeReference == TypeDocumentReferenceGji.ActCheckToProsecutor
                            && (x.Document1.Id == entity.Id || x.Document2.Id == entity.Id));

                if (docActCheck == null && entity.ActCheck != null)
                {
                    // Если связи небыло, в обновляемом объекте есть документ акта проверки, то создаем связь
                    var newRef = new DocumentGjiReference
                    {
                        TypeReference = TypeDocumentReferenceGji.ActCheckToProsecutor,
                        Document1 = entity,
                        Document2 = entity.ActCheck
                    };

                    domainDocumentRef.Save(newRef);
                }
                else if (docActCheck != null && entity.ActCheck != null)
                {
                    // Если связь уже существует и также пришел документ в обновляемой сущности то перезаписываем
                    docActCheck.Document1 = entity;
                    docActCheck.Document2 = entity.ActCheck;

                    domainDocumentRef.Update(docActCheck);
                }

                return base.BeforeUpdateAction(service, entity);
            }
            finally
            {
                Container.Release(domainDocumentRef);
            }

        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var annexService = this.Container.Resolve<IDomainService<ResolProsAnnex>>();
            var lawService = this.Container.Resolve<IDomainService<ResolProsArticleLaw>>();
            var resolProsRealityObjectService = Container.Resolve<IDomainService<ResolProsRealityObject>>();

            try
            {

                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return Failure(result.Message);
                }

                var refFuncs = new List<Func<long, string>>
                               {
                                  id => annexService.GetAll().Any(x => x.ResolPros.Id == id) ? "Приложения" : null,
                                  id => lawService.GetAll().Any(x => x.ResolPros.Id == id) ? "Статьи закона" : null
                               };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }

                var roIds = resolProsRealityObjectService.GetAll().Where(x => x.ResolPros.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in roIds)
                {
                    resolProsRealityObjectService.Delete(value);
                }

                // Короче поскольку над оудалит ьдокумент то Основание проверки данного документа также адо будет удалить
                // Следовательно запоминаем Id оснвоания чтобы в AfterDelete совершить данный акт удаления
                if (entity.Inspection != null)
                {
                    inspectionId = entity.Inspection.Id;
                }

                return result;
            }
            finally
            {
                Container.Release(annexService);
                Container.Release(lawService);
                Container.Release(resolProsRealityObjectService);
            }

        }

        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            var baseService = Container.Resolve<IDomainService<BaseProsResol>>();
            var documentService = Container.Resolve<IDomainService<DocumentGji>>();
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                // Сначала вызываем удаление базового потомучто там произходит зачистка Stage 
                var result = base.AfterDeleteAction(service, entity);

                if (inspectionId > 0)
                {
                    if (!documentService.GetAll().Any(x => x.Inspection.Id == inspectionId))
                    {
                        // Если нет ниодного документа у основания то удаляем основание
                        baseService.Delete(inspectionId);
                    }
                }
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
                return result;
            }
            finally
            {
                Container.Release(baseService);
                Container.Release(documentService);
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

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "Inspection", "Проверка ГЖИ" },
                { "Stage", "Этап проверки" },
                { "TypeDocumentGji", "Тип документа ГЖИ" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNumber", "Номер документа" },
                { "DocumentSubNum", "Дополнительный номер документа (порядковый номер если документов одного типа несколько)" },
                { "State", "Статус" },
                { "Executant", "Тип исполнителя документа" },
                { "Contragent", "Контрагент" },
                { "PhysicalPerson", "Физическое лицо" },
                { "PhysicalPersonPosition", "Должность физ.лица" },
                { "PhysicalPersonInfo", "Реквизиты физ.лица" },
                { "Municipality", "Муниципальное образование (Орган прокуратуры, вынесший постановление)" },
                { "DateSupply", "Дата поступления в ГЖИ" },
                { "UIN", "УИН" },
                { "IssuedByPosition", "Должность прокурора" },
                { "IssuedByRank", "Звание прокурора" },
                { "IssuedByFio", "Фио прокурора" },
                { "DateResolPros", "Дата рассмотрения постановления прокуратуры" }
            };
            return result;
        }
    }
}