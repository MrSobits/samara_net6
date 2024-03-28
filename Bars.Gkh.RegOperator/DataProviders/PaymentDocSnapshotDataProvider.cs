namespace Bars.Gkh.RegOperator.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Analytics.Data;
    using B4.Modules.Analytics.Enums;
    using B4.Utils;

    using Bars.Gkh.Entities;

    using Castle.Windsor;
    using Entities;
    using Entities.PersonalAccount.PayDoc;
    using Meta;

    internal class PaymentDocSnapshotDataProvider :
        BaseCollectionDataProvider<PaymentDocumentSnapshotInfo>
    {
        public PaymentDocSnapshotDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        public override string Description
        {
            get { return this.Name; }
        }

        public override string Name
        {
            get { return "Документы на оплату"; }
        }

        public override IEnumerable<DataProviderParam> Params
        {
            get
            {
                return new[]
                {
                    new DataProviderParam
                    {
                        Name = string.Format("{0}ChargePeriod", this.Key),
                        ParamType = ParamType.Catalog,
                        Label = "Период",
                        Additional = "ChargePeriod",
                        Required = true
                    },
                    new DataProviderParam
                    {
                        Name = string.Format("{0}DeliveryAgent", this.Key),
                        ParamType = ParamType.Catalog,
                        Label = "Агент доставки",
                        Additional = "DeliveryAgent",
                        Required = true
                    }
                };
            }
        }

        protected override IQueryable<PaymentDocumentSnapshotInfo> GetDataInternal(BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAs<long>(string.Format("{0}ChargePeriod", this.Key));
            var deliveryAgentId = baseParams.Params.GetAs<long>(string.Format("{0}DeliveryAgent", this.Key));

            var snapshotDomain = this.Container.ResolveDomain<AccountPaymentInfoSnapshot>();
            var deliveryRealObjDomain = this.Container.ResolveDomain<DeliveryAgentRealObj>();
            var periodDomain = this.Container.ResolveDomain<ChargePeriod>();
            var personalAccountDomain = this.Container.ResolveDomain<BasePersonalAccount>();

            try
            {
                var period = periodDomain.GetAll().FirstOrDefault(x => x.Id == periodId);

                var roIds =
                    deliveryRealObjDomain.GetAll()
                        .Where(x => x.Id == deliveryAgentId && (!x.DateEnd.HasValue || x.DateEnd > period.StartDate))
                        .Select(x => x.RealityObject.Id)
                        .ToList();

                var persIds =
                    personalAccountDomain.GetAll()
                        .Where(x => roIds.Contains(x.Room.RealityObject.Id))
                        .Select(x => x.Id)
                        .ToList();

                var invoiceInfo = snapshotDomain.GetAll()
                    .Where(x => x.Snapshot.Period.Id == periodId)
                    .Where(x => persIds.Contains(x.AccountId))
                    .Select(x => x.Snapshot.ConvertTo<InvoiceInfo>());

                var result = invoiceInfo
                    .Select(x =>
                        new PaymentDocumentSnapshotInfo
                        {
                            НомерЛС = x.ЛицевойСчет,
                            ТипАбонента = "Счет физ. лица",
                            ФИО = x.ФИОСобственника,
                            Индекс = x.Индекс,
                            АдресАбонента = x.АдресКвартиры,
                            МуниципальныйРайон = x.Municipality,
                            МуниципальноеОбразование = x.Settlement,
                            Получатель = x.НаименованиеПолучателя,
                            ИннПолучателя = x.ИннПолучателя,
                            КппПолучателя = x.КппПолучателя,
                            АдресПолучателя = x.АдресПолучателя,
                            РсПолучателя = x.РсчетПолучателя,
                            Тариф = x.Тариф ?? 0,
                            ОбщаяПлощадь = x.ОбщаяПлощадь.ToDecimal(),
                            КОплатеЗаМесяц = x.ИтогоКОплате ?? 0,
                            ВсегоКОплате = (x.ИтогоКОплате ?? 0).RoundDecimal(),
                            Период = x.НаименованиеПериода
                        });

                foreach (var res in result)
                {
                    if (res.Тариф == 0)
                    {
                        res.СообщниеОбОшибкке = "Тариф не должен быть равен нулю";
                    }

                    if (res.ОбщаяПлощадь == 0)
                    {
                        res.СообщниеОбОшибкке = "Площадь помещения не должна быть равна нулю";
                    }

                    if (res.КОплатеЗаМесяц == 0)
                    {
                        res.СообщниеОбОшибкке = "Cумма не должна быть равна нулю";
                    }
                }

                return result;
            }
            finally
            {
                this.Container.Release(snapshotDomain);
                this.Container.Release(deliveryRealObjDomain);
                this.Container.Release(periodDomain);
            }
        }
    }
}