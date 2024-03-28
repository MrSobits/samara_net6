namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Создатель слепков данных
    /// </summary>
    [System.Obsolete("use class SnapshotCreator")]
    internal class InvoiceAndActDataSnapshotCreator //: InvoiceInfoSnapshotCreatorBase<InvoiceAndActInfo>
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="cache">Кэш данных</param>
        /// <param name="configProv">Провайдер конфигурации</param>
        /// <param name="period">Перод сборки</param>
        /*public InvoiceAndActDataSnapshotCreator(
            IWindsorContainer container,
            GkhCache cache,
            IGkhConfigProvider configProv,
            ChargePeriod period) : base(container, cache, configProv, period)
        {
        }

        /// <summary>
        /// Создание слепка данных
        /// </summary>
        /// <param name="isZeroPayment">Допускаются нулевые платежи</param>
        /// <param name="personalAccountOwnerType">Тип абонента</param>
        public override void CreateSnapshots(bool isZeroPayment, PersonalAccountOwnerType personalAccountOwnerType)
        {
            this.WarmCache();

            var formatter = new ReportFormatter();

            var accountOwnersDict = this.Cache.GetCache<LegalAccountOwner>().GetEntities()
                .Select(x => new
                {
                    x.Id,
                    ContragentName = x.Contragent.With(c => c.Name),
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    AddressName = x.Contragent.With(c => c.FiasJuridicalAddress).With(c => c.AddressName),
                    FiasMailingAddress = x.Contragent.With(c => c.FiasMailingAddress),
                    ContragentId = x.Contragent.Return(z => z.Id),
                    x.PrintAct
                })
                .ToDictionary(x => x.Id);

            var contragentContactsDict = this.Cache.GetCache<ContragentContact>().GetEntities()
                .Select(x => new {x.Contragent.Id, x.FullName, PositionName = x.Position.Name})
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    x => x.GroupBy(y => y.PositionName.ToUpper())
                        .ToDictionary(y => y.Key, y => y.First().FullName));

            var contacts = this.Cache.GetCache<ContragentContact>().GetEntities()
                .Where(x => RegOperator.Contragent.Id == x.Contragent.Id)
                .GroupBy(x => x.Position.Name.Trim().ToUpper())
                .ToDictionary(x => x.Key,
                    y => y.Return(x =>
                    {
                        var rec = y.First();
                        return string.Format(
                            "{0} {1}. {2}.",
                            rec.Surname,
                            rec.Name.ElementAtOrDefault(0),
                            rec.Patronymic.ElementAtOrDefault(0));
                    }));

            var contragentIdList = accountOwnersDict.Select(x => x.Value.ContragentId).ToArray();
            var contragentBankCreditOrgDomain = Cache.GetCache<ContragentBankCreditOrg>();
            var contragentBankCreditOrgsDict = contragentBankCreditOrgDomain.GetEntities()
                .Where(x => contragentIdList.Contains(x.Contragent.Id))
                .Select(
                    x => new
                    {
                        ContragentId = x.Contragent.Id,
                        x.Name,
                        x.Okpo,
                        x.Okonh,
                        x.SettlementAccount
                    })
                .AsEnumerable()
                .GroupBy(x => x.ContragentId)
                .ToDictionary(x => x.Key, x => x.First());

            var commonData = this.GetCommonData(
                isZeroPayment,
                record =>
                {
                    if (!accountOwnersDict.ContainsKey(record.OwnerId)) return;

                    var accountOwner = accountOwnersDict[record.OwnerId];

                    record.ПечататьАкт = accountOwner.PrintAct;

                    record.Плательщик = accountOwner.ContragentName;
                    record.ИННСобственника = accountOwner.Inn;
                    record.КППСобственника = accountOwner.Kpp;
                    record.РуководительПолучателя = contacts.Get("РУКОВОДИТЕЛЬ");
                    record.ГлБухПолучателя = contacts.Get("ГЛАВНЫЙ БУХГАЛТЕР");
                    record.НаименованиеПолучателяКраткое = RegOperator != null
                        ? RegOperator.Contragent.ShortName
                        : string.Empty;
                    record.АдресКонтрагента = accountOwner.AddressName;
                    record.ПериодНачислений = isZeroPayment
                        ? string.Empty
                        : "за период с {0} года по {1} года".FormatUsing(
                            record.ДатаНачалаПериода,
                            record.ДатаОкончанияПериода);

                    var contragentCreditOrg = contragentBankCreditOrgsDict.Get(accountOwner.ContragentId);
                    if (contragentCreditOrg != null)
                    {
                        record.Наименование = contragentCreditOrg.Name;
                        record.РСчетБанка = contragentCreditOrg.SettlementAccount;
                        record.ОКПО = contragentCreditOrg.Okpo;
                        record.ОКОНХ = contragentCreditOrg.Okonh;
                    }

                    this.ApplyAddressInfo(record, accountOwner.FiasMailingAddress);

                    if (!isZeroPayment)
                    {
                        record.Итого = record.Начислено;
                        СуммаВсего = record.Итого + record.Пени + record.Перерасчет;

                        record.СуммаСтрокой = formatter.Format(
                            "СуммаСтрокой",
                            record.СуммаВсего,
                            CultureInfo.InvariantCulture.NumberFormat);
                    }

                    if (contragentContactsDict.ContainsKey(accountOwner.ContragentId))
                    {
                        var regopContact = contragentContactsDict[accountOwner.ContragentId];

                        record.Руководитель = regopContact.Get("РУКОВОДИТЕЛЬ") ?? string.Empty;
                        record.ГлавБух = regopContact.Get("ГЛАВНЫЙ БУХГАЛТЕР") ?? string.Empty;
                    }

                    record.Номер = this.GetDocNumber(record.AccountId);
                    record.ПорядковыйНомерВГоду = this.GetNextSerialNumber();
                });

            this.CreateSnapshots(commonData, PaymentDocumentData.OwnerholderType, true, personalAccountOwnerType);
        }*/
    }
}