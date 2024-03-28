namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolutionServiceInterceptor: ResolutionServiceInterceptor<Resolution>
    {
    }

    public class ResolutionServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : Resolution
    {
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            var payService = Container.Resolve<IDomainService<ResolutionPayFine>>();

            try
            {
                // Если во вкладке "Оплаты штрафов" поле "Итого" равно или больше значения "Сумма штрафа" во вкладке "Реквизиты", 
                // то Поле "Штраф оплачен" должно принимать значение "Да"
                    var resolutionPayFineSum = payService
                             .GetAll()
                             .Where(x => x.Resolution.Id == entity.Id)
                             .Sum(x => x.Amount)
                             .ToDecimal();
                if (entity.PenaltyAmount.HasValue && entity.Payded50Percent && resolutionPayFineSum >= entity.PenaltyAmount/2)
                {
                    entity.Paided = YesNoNotSet.Yes;
                }
                if (entity.PenaltyAmount.HasValue && resolutionPayFineSum >= entity.PenaltyAmount)
                {
                    entity.Paided = YesNoNotSet.Yes;
                }
               
                if (entity.DeliveryDate.HasValue)
                {
                    if (!entity.InLawDate.HasValue)
                    {
                        entity.InLawDate = entity.DeliveryDate.Value.AddDays(11);
                    }
                    if (entity.PenaltyAmount > 0)
                    {
                        entity.DueDate = entity.DeliveryDate.Value.AddDays(60);
                    }
                }

                return base.BeforeUpdateAction(service, entity);
            }
            finally
            {
                Container.Release(payService);
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            if (!result.Success)
            {
                return Failure(result.Message);
            }

            var annexService = this.Container.Resolve<IDomainService<ResolutionAnnex>>();
            var defService = this.Container.Resolve<IDomainService<ResolutionDefinition>>();
            var disputeService = this.Container.Resolve<IDomainService<ResolutionDispute>>();
            var payService = this.Container.Resolve<IDomainService<ResolutionPayFine>>();

            try
            {
                annexService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => annexService.Delete(x));

                defService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => defService.Delete(x));

                disputeService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => disputeService.Delete(x));

                payService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => payService.Delete(x));

                return Success();
            }
            finally
            {
                Container.Release(annexService);
                Container.Release(defService);
                Container.Release(disputeService);
                Container.Release(payService);
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
                { "Executant", "Тип исполнителя документа" },
                { "Municipality", "Муниципальное образование" },
                { "Contragent", "Контрагент" },
                { "Sanction", "Вид санкции" },
                { "Official", "Должностное лицо" },
                { "Surname", "Фамилия" },
                { "FirstName", "Имя" },
                { "Patronymic", "Отчество" },
                { "Position", "Должность" },
                { "PhysicalPerson", "Физическое лицо" },
                { "PhysicalPersonInfo", "Реквизиты физ. лица" },
                { "PhysicalPersonDocType", "Тип документа физ лица" },
                { "PhysicalPersonDocumentNumber", "Номер документа физлица" },
                { "PhysicalPersonDocumentSerial", "Серия документа физлица" },
                { "DeliveryDate", "Дата вручения" },
                { "TypeInitiativeOrg", "Тип инициативного органа" },
                { "SectorNumber", "Номер участка" },
                { "PenaltyAmount", "Сумма штрафов" },
                { "Paided", "Штраф оплачен" },
                { "PayStatus", "Штраф оплачен (подробно)" },
                { "DateTransferSsp", "Дата передачи в ССП" },
                { "DocumentNumSsp", "Номер документа, передача в ССП" },
                { "Description", "Примечание" },
                { "DocumentTime", "Время составления документа" },
                { "DateWriteOut", "Выписка из ЕГРЮЛ" },
                { "BecameLegal", "Вступило в законную силу" },
                { "FineMunicipality", "МО получателя штрафа" }
            };
            return result;
        }
    }
}