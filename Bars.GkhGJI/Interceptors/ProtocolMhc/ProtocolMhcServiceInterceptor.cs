namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ProtocolMhcServiceInterceptor : ProtocolMhcServiceInterceptor<ProtocolMhc>
    {
        // доработки вносить в Genetric
    }

    public class ProtocolMhcServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : ProtocolMhc
    {

        private long inspectionId { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var domainServiceInspection = Container.Resolve<IDomainService<BaseProtocolMhc>>();
            var domainStage = Container.Resolve<IDomainService<InspectionGjiStage>>();

            try
            {
                // Также перед созданием мы создаем Проверку Поскольку Документы ГЖИ без основания немогут быть
                // И дерево Этапов также настроено на inspectionGJI поэтому скрыто от глаз пользователей создаем основание
                // Проверки с типом постановление прокуратуры
                var newInspection = new BaseProtocolMhc()
                {
                    TypeBase = TypeBase.ProtocolMhc
                };

                domainServiceInspection.Save(newInspection);

                // также перед созданием, создаем этап 
                var newStage = new InspectionGjiStage
                {
                    Inspection = newInspection,
                    TypeStage = TypeStage.ProtocolMhc,
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

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var annexService = this.Container.Resolve<IDomainService<ProtocolMhcAnnex>>();
            var lawService = this.Container.Resolve<IDomainService<ProtocolMhcArticleLaw>>();
            var definitionService = this.Container.Resolve<IDomainService<ProtocolMhcDefinition>>();
            var roService = Container.Resolve<IDomainService<ProtocolMhcRealityObject>>();

            try
            {

                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return Failure(result.Message);
                }

                var refFuncs = new List<Func<long, string>>
                               {
                                  id => annexService.GetAll().Any(x => x.ProtocolMhc.Id == id) ? "Приложения" : null,
                                  id => lawService.GetAll().Any(x => x.ProtocolMhc.Id == id) ? "Статьи закона" : null,
                                  id => definitionService.GetAll().Any(x => x.ProtocolMhc.Id == id) ? "Определения" : null
                               };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }

                var roIds = roService.GetAll().Where(x => x.ProtocolMhc.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in roIds)
                {
                    roService.Delete(value);
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
                Container.Release(definitionService);
                Container.Release(roService); 
            }
        }

        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            var baseService = Container.Resolve<IDomainService<BaseProtocolMhc>>();
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
            return this.Success();
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
            return this.Success();
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
                { "PhysicalPersonInfo", "Реквизиты физ.лица" },
                { "Municipality", "Муниципальное образование (Орган прокуратуры, вынесший постановление)" },
                { "DateSupply", "Дата поступления в ГЖИ" }
            };
            return result;
        }
    }
}