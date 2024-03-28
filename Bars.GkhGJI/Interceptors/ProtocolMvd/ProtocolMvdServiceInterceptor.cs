namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.DomainService;

    public class ProtocolMvdServiceInterceptor : ProtocolMvdServiceInterceptor<ProtocolMvd>
    {
    }

    public class ProtocolMvdServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T: ProtocolMvd
    {
        /// <summary>
        /// Инспектор Id
        /// </summary>
        private long inspectionId { get; set; }

        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var domainServiceInspection = this.Container.Resolve<IDomainService<BaseProtocolMvd>>();
            var domainStage = this.Container.Resolve<IDomainService<InspectionGjiStage>>();


            try
            {
                // Также перед созданием мы создаем Проверку Поскольку Документы ГЖИ без основания немогут быть
                // И дерево Этапов также настроено на inspectionGJI поэтому скрыто от глаз пользователей создаем основание
                // Проверки с типом постановление прокуратуры
                var newInspection = new BaseProtocolMvd
                {
                    TypeBase = TypeBase.ProtocolMvd
                };

                domainServiceInspection.Save(newInspection);

                // также перед созданием мы создаем этап 
                var newStage = new InspectionGjiStage
                {
                    Inspection = newInspection,
                    TypeStage = TypeStage.ProtocolMvd,
                    Position = 0
                };

                domainStage.Save(newStage);

                entity.Inspection = newInspection;
                entity.Stage = newStage;

                if (entity.DocumentDate.HasValue)
                {
                    entity.DocumentYear = entity.DocumentDate.Value.Year;
                }

                return base.BeforeCreateAction(service, entity);
            }
            finally
            {
                this.Container.Release(domainServiceInspection);
                this.Container.Release(domainStage);
            }
        }

        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var annexService = this.Container.Resolve<IDomainService<ProtocolMvdAnnex>>();
            var lawService = this.Container.Resolve<IDomainService<ProtocolMvdArticleLaw>>();
            var protocolMvdRealityObjectService = this.Container.Resolve<IDomainService<ProtocolMvdRealityObject>>();

            try
            {

                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return this.Failure(result.Message);
                }

                var refFuncs = new List<Func<long, string>>
                               {
                                  id => annexService.GetAll().Any(x => x.ProtocolMvd.Id == id) ? "Приложения" : null,
                                  id => lawService.GetAll().Any(x => x.ProtocolMvd.Id == id) ? "Статьи закона" : null
                               };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return this.Failure(message);
                }

                var roIds = protocolMvdRealityObjectService.GetAll().Where(x => x.ProtocolMvd.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in roIds)
                {
                    protocolMvdRealityObjectService.Delete(value);
                }

                // Короче поскольку над оудалит ьдокумент то Основание проверки данного документа также адо будет удалить
                // Следовательно запоминаем Id оснвоания чтобы в AfterDelete совершить данный акт удаления
                if (entity.Inspection != null)
                {
                    this.inspectionId = entity.Inspection.Id;
                }

                return result;
            }
            finally 
            {
                this.Container.Release(annexService);
                this.Container.Release(lawService);
                this.Container.Release(protocolMvdRealityObjectService);
            }
        }

        /// <summary>
        /// Метод вызывается после удаления объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            var baseService = this.Container.Resolve<IDomainService<BaseProtocolMvd>>();
            var documentService = this.Container.Resolve<IDomainService<DocumentGji>>();
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                // Сначала вызываем удаление базового потомучто там произходит зачистка Stage 
                var result = base.AfterDeleteAction(service, entity);

                if (this.inspectionId > 0)
                {
                    if (!documentService.GetAll().Any(x => x.Inspection.Id == this.inspectionId))
                    {
                        // Если нет ниодного документа у основания то удаляем основание
                        baseService.Delete(this.inspectionId);
                    }
                }
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.Id.ToString());
                return result;
            }
            finally
            {
                this.Container.Release(baseService);
                this.Container.Release(documentService);
            }
        }

        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
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
                { "Municipality", "Муниципальное образование" },
                { "DateSupply", "Дата поступления в ГЖИ" },
                { "TypeExecutant", "Тип исполнителя документа" },
                { "PhysicalPerson", "Физическое лицо" },
                { "PhysicalPersonInfo", "Реквизиты физ. лица" },
                { "OrganMvd", "Орган МВД, оформивший протокол" },
                { "DateOffense", "Дата парвонарушения" },
                { "SerialAndNumber", "Серия и номер паспорта" },
                { "BirthDate", "Дата рождения" },
                { "IssueDate", "Дата выдачи" },
                { "BirthPlace", "Место рождения" },
                { "IssuingAuthority", "Кем выдан" },
                { "Company", "Место работы, должность" }
            };
            return result;
        }
    }
}