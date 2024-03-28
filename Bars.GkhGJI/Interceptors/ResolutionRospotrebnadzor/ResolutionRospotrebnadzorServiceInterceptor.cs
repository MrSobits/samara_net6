namespace Bars.GkhGji.Interceptors.ResolutionRospotrebnadzor
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Interceptor Постановление Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorServiceInterceptor: ResolutionRospotrebnadzorServiceInterceptor<ResolutionRospotrebnadzor>
    {
    }

    /// <summary>
    /// Generic Interceptor Постановление Роспотребнадзора
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResolutionRospotrebnadzorServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : ResolutionRospotrebnadzor
    {
        /// <summary>
        /// Проверка правильности номера документа ГЖИ
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param> 
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            if (!string.IsNullOrEmpty(entity.DocumentNumber) && entity.DocumentDate.HasValue)
            {
                if (service.GetAll()
                    .Any(x => x.DocumentNumber == entity.DocumentNumber
                        && x.DocumentDate.Value.Year == entity.DocumentDate.Value.Year
                        && x.Id != entity.Id))
                {
                    return Failure("Найдено постановление Роспотребнадзора с совпадающим номером");
                }
            }

            this.Container.UsingForResolved<IDomainService<ResolutionRospotrebnadzorPayFine>>((container, payService) =>
            {
                // Если во вкладке "Оплаты штрафов" поле "Итого" равно или больше значения "Сумма штрафа" во вкладке "Реквизиты",
                // то Поле "Штраф оплачен" должно принимать значение "Да"
                var resolutionPayFineSum = payService.GetAll()
                    .Where(x => x.Resolution.Id == entity.Id)
                    .Sum(x => x.Amount)
                    .ToDecimal();

                if (entity.PenaltyAmount.HasValue && resolutionPayFineSum >= entity.PenaltyAmount)
                {
                    entity.Paided = YesNoNotSet.Yes;
                }
            });

            return base.BeforeUpdateAction(service, entity);
        }

        /// <summary>
        /// Удаление засимых сущностей
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            if (!result.Success)
            {
                return Failure(result.Message);
            }

            var annexService = this.Container.ResolveDomain<ResolutionRospotrebnadzorAnnex>();
            var defService = this.Container.ResolveDomain<ResolutionRospotrebnadzorDefinition>();
            var disputeService = this.Container.ResolveDomain<ResolutionRospotrebnadzorDispute>();
            var payService = this.Container.ResolveDomain<ResolutionRospotrebnadzorPayFine>();
            var violationService = this.Container.ResolveDomain<ResolutionRospotrebnadzorViolation>();
            var articleLawService = this.Container.ResolveDomain<ResolutionRospotrebnadzorArticleLaw>();
            try
            {
                annexService.GetAll()
                    .Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => annexService.Delete(x));

                defService.GetAll()
                    .Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => defService.Delete(x));

                disputeService.GetAll()
                    .Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => disputeService.Delete(x));

                payService.GetAll()
                    .Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => payService.Delete(x));

                violationService.GetAll()
                    .Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => violationService.Delete(x));

                articleLawService.GetAll()
                    .Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => articleLawService.Delete(x));

                return Success();
            }
            finally
            {
                this.Container.Release(annexService);
                this.Container.Release(defService);
                this.Container.Release(disputeService);
                this.Container.Release(payService);
                this.Container.Release(violationService);
                this.Container.Release(articleLawService);
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
                { "DocumentReason", "Документ-основание" },
                { "DeliveryDate", "Дата вручения" },
                { "RevocationReason", "Причина аннулирования" },
                { "TypeInitiativeOrg", "Тип инициативного органа (кем вынесено)" },
                { "FineMunicipality", "Муниципальное образование получателя штрафа" },
                { "Official", "Должностное лицо" },
                { "LocationMunicipality", "Местонахождение" },
                { "ExpireReason", "Основание прекращения" },
                { "Sanction", "Вид санкции" },
                { "PenaltyAmount", "Сумма штрафов" },
                { "SspDocumentNum", "Номер документа (Санкция)" },
                { "Paided", "Штраф оплачен" },
                { "TransferToSspDate", "Дата передачи в ССП" },
                { "Executant", "Тип исполнителя документа" },
                { "PhysicalPerson", "Физическое лицо" },
                { "PhysicalPersonInfo", "Реквизиты физ.лица" },
                { "Contragent", "Контрагент" }
            };
            return result;
        }
    }
}