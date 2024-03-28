namespace Bars.Gkh.RegOperator.DomainService.PaymentDocument.Impl
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Castle.Windsor;
    using System.Linq;

    public class SberbankPaymentDocService : ISberbankPaymentDocService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<SberbankPaymentDoc> PayDocDomain { get; set; }
        public IDomainService<BasePersonalAccount> PersAccDomain { get; set; }
        public IDomainService<PaymentDocumentSnapshot> SnapshotDomain { get; set; }
        public IDomainService<ChargePeriod> PeriodDomain { get; set; }

        /// <summary>
        /// Сформировать реестр
        /// </summary>
        public IDataResult CreateReestr(BaseParams baseParams)
        {
            const string query =
                @"truncate SBERBANK_PAYMENT_DOC cascade;
                  insert into SBERBANK_PAYMENT_DOC (object_version, object_create_date, object_edit_date, period, account, last_date, count, guid)
                      select 1, 
                  		  current_date, 
                  		  current_date, 
                  		  (select id from REGOP_PERIOD where CIS_CLOSED = true order by id desc limit 1), 
                  		  HOLDER_ID, 
                  		  current_date,
                  		  0,
                  		  uuid_generate_v1()
                  	  from REGOP_PAYMENT_DOC_SNAPSHOT 
                  	  where DOC_TYPE = 0 
                  		  and HOLDER_TYPE = 'PersonalAccount' 
                  		  and PERIOD_ID = (select id from REGOP_PERIOD where CIS_CLOSED = true order by id desc limit 1)
                  		  and HOLDER_ID in (select id from REGOP_PERS_ACC where STATE_ID = 402);";

            PayDocDomain = Container.ResolveDomain<SberbankPaymentDoc>();
            PersAccDomain = Container.ResolveDomain<BasePersonalAccount>();
            SnapshotDomain = Container.ResolveDomain<PaymentDocumentSnapshot>();
            PeriodDomain = Container.ResolveDomain<ChargePeriod>();

            var lastPeriod = PeriodDomain.GetAll()
                .OrderByDescending(x => x.Id)
                .FirstOrDefault(x => x.IsClosed);

            var previousDoc = PayDocDomain.GetAll().FirstOrDefault();

            if (previousDoc == null || (previousDoc != null && previousDoc.Period.Id != lastPeriod.Id))
            {
                var accWithSnapshotIds = SnapshotDomain.GetAll()
                .Where(x => x.PaymentDocumentType == Enums.PaymentDocumentType.Individual)
                .Where(x => x.HolderType == PaymentDocumentData.AccountHolderType)
                .Where(x => x.Period.Id == lastPeriod.Id)
                .Select(x => x.HolderId);

                var openAccs = PersAccDomain.GetAll()
                    .Where(x => accWithSnapshotIds.Contains(x.Id))
                    .Where(x => x.State.Code == "1");

                if (openAccs != null && openAccs.Count() > 0)
                {
                    Container.Resolve<ISessionProvider>()
                        .GetCurrentSession()
                        .CreateSQLQuery(query)
                        .ExecuteUpdate();

                    return new BaseDataResult(true, "Реестр сформирован");
                }
                else
                {
                    return new BaseDataResult(false, "В последнем закрытом периоде нет открытых лицевых счетов с историей платежных документов");
                }
            }
            else
            {
                return new BaseDataResult(false, "Реестр по последнему закрытому периоду уже сформирован");
            }
        }
    }
}