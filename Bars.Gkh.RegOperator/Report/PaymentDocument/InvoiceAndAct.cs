namespace Bars.Gkh.RegOperator.Report.PaymentDocument
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.Gkh.Overhaul.Entities;

    using Gkh.Entities;
    using Entities;
    using Castle.Windsor;
    using Bars.B4;

    [Obsolete("use class SnapshotCreator")]
    public class InvoiceAndAct : Receipt
    {
        public bool withoutAct;

        public InvoiceAndAct(IWindsorContainer container) : base(container)
        {
        }

        public override Stream GetTemplate()
        {
            return withoutAct
                ? new ReportTemplateBinary(Properties.Resources.PaymentDocumentInvoice).GetTemplate()
                : new ReportTemplateBinary(Properties.Resources.PaymentDocumentInvoiceAndAct).GetTemplate();
        }

        protected override object GetData(ChargePeriod period)
        {
            var data = GetCommonData<ExtendedReportRecord>(period);

            var ownerIds = data.Select(x => x.OwnerId).Distinct().ToList();
            
            var formatter = new ReportFormatter();

            var accountOwnersDict = Cache.GetCache<LegalAccountOwner>().GetEntities()//Container.ResolveDomain<LegalAccountOwner>().GetAll()
                .Where(x => ownerIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    ContragentName = x.Contragent.With(c => c.Name),
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    AddressName = x.Contragent.With(c => c.FiasJuridicalAddress).With(c => c.AddressName),
                    FiasMailingAddress = x.Contragent.With(c => c.FiasMailingAddress),
                    ContragentId = (long?)x.Contragent.Id,
                    x.PrintAct
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First());

            var contragentIdList = accountOwnersDict.Select(x => x.Value.ContragentId).Where(x => x.HasValue).ToList();

            var contragentContractDomain = Cache.GetCache<ContragentContact>().GetEntities();//Container.ResolveDomain<ContragentContact>();

            var contragentContactsDict = contragentContractDomain//.GetAll()
                .Where(x => contragentIdList.Contains(x.Contragent.Id))
                .Where(x => x.DateStartWork == null || x.DateStartWork <= period.EndDate)
                .Where(x => x.DateEndWork == null || x.DateEndWork >= period.StartDate)
                .Select(x => new { x.Contragent.Id, x.FullName, PositionName = x.Position.Name })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.PositionName.ToLower()).ToDictionary(y => y.Key, y => y.First().FullName));

            var contacts = contragentContractDomain//.GetAll()
                    .Where(x => RegOperator.Contragent.Id == x.Contragent.Id)
                    .AsEnumerable()
                    .GroupBy(x => x.Position.Name.Trim().ToUpper())
                    .ToDictionary(x => x.Key, y => y.Return(x =>
                    {
                        var rec = y.First();
                        return string.Format("{0} {1}. {2}.", rec.Surname, rec.Name.FirstOrDefault(), rec.Patronymic.FirstOrDefault()); 
                    }));

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

			withoutAct = false;

            foreach (var account in data)
            {
                if (accountOwnersDict.ContainsKey(account.OwnerId))
                {
                    var accountOwner = accountOwnersDict[account.OwnerId];

                    withoutAct = !accountOwner.PrintAct;

                    account.Плательщик = accountOwner.ContragentName;
                    account.ИННСобственника = accountOwner.Inn;
                    account.КППСобственника = accountOwner.Kpp;
                    account.РуководительПолучателя = contacts.Get("РУКОВОДИТЕЛЬ");
                    account.ГлБухПолучателя = contacts.Get("ГЛАВНЫЙ БУХГАЛТЕР");
                    account.НаименованиеПолучателяКраткое = RegOperator != null ? RegOperator.Contragent.ShortName : string.Empty;
                    account.АдресКонтрагента = accountOwner.AddressName;

	                if (accountOwner.ContragentId != null)
	                {
		                var contragentCreditOrg = contragentBankCreditOrgsDict.Get(accountOwner.ContragentId.Value);
		                if (contragentCreditOrg != null)
		                {
			                account.Наименование = contragentCreditOrg.Name;
			                account.РСчетБанка = contragentCreditOrg.SettlementAccount;
			                account.ОКПО = contragentCreditOrg.Okpo;
			                account.ОКОНХ = contragentCreditOrg.Okonh;
		                }
	                }

	                if (accountOwner.FiasMailingAddress != null)
                    {
                        account.ПочтовыйИндексКонтрагента = accountOwner.FiasMailingAddress.PostCode;

                        // По задаче 44880 адрес должен выводится не так: Камчатский край, Карагинский р-н, с. Карага, ул. Лукашевского, д. 14
                        // а вот так: ул. Лукашевского, д. 14, с. Карага, Карагинский р-н, Камчатский край
                        var mailingAdress = accountOwner.FiasMailingAddress.AddressName;

                        var streetAdderss = accountOwner.FiasMailingAddress.StreetName.IsNotEmpty() ? 
                            mailingAdress.Substring(mailingAdress.IndexOf(accountOwner.FiasMailingAddress.StreetName, System.StringComparison.Ordinal)) : string.Empty;

                        var other = streetAdderss.IsNotEmpty() ? mailingAdress.Replace(streetAdderss, string.Empty) : mailingAdress;

                        account.ПочтовыйАдресКонтрагента = streetAdderss;

                        var splitOther = other.Split(',');

                        var idx = splitOther.Length - 1;
                        var item = string.Empty;

                        // теперь оставшуюся част ьадреса пишем в обратнубю сторону чтобы получилось так
                        // ул. Лукашевского, д. 14, с. Карага, Карагинский р-н, Камчатский край
                        while (idx >= 0)
                        {
                            item = splitOther[idx];

                            if (item != null && !string.IsNullOrEmpty(item.Trim()))
                            {
                                account.ПочтовыйАдресКонтрагента += string.Format(", {0}", item.Trim());
                            }

                            idx--;
                        }
                    }
                    else if (accountOwner.ContragentId != null)
                    {
                        var contragentAccount = this.Container.Resolve<IDomainService<Contragent>>().Get(accountOwner.ContragentId);
                        account.ПочтовыйИндексКонтрагента = contragentAccount.FiasJuridicalAddress.PostCode;
                    }

                    if (!IsZeroPaymentDoc)
                    {
                        account.Итого = account.Начислено;
                        account.СуммаВсего = account.Итого + account.Пени + account.Перерасчет;

                        account.СуммаСтрокой = formatter.Format("СуммаСтрокой", account.СуммаВсего,
                            CultureInfo.InvariantCulture.NumberFormat);
                    }

                    if (accountOwner.ContragentId.HasValue
                        && contragentContactsDict.ContainsKey(accountOwner.ContragentId.Value))
                    {
                        var contragentContacts = contragentContactsDict[accountOwner.ContragentId.Value];

                        account.Руководитель = contragentContacts.Get("руководитель") ?? string.Empty;
                        account.ГлавБух = contragentContacts.Get("главный бухгалтер") ?? string.Empty;
                    }

                    account.Номер = GetDocNumber(account.Id, period.Id);
                }
            }
            
            return data;
        }

        private class ExtendedReportRecord : ReportRecord
        {
            public string ИННСобственника { get; set; }

            public string КППСобственника { get; set; }

            public string Плательщик { get; set; }

            public string Руководитель { get; set; }

            public string СуммаСтрокой { get; set; }

            public string ГлавБух { get; set; }

            public int Номер { get; set; }

            public string РуководительПолучателя { get; set; }

            public string ГлБухПолучателя { get; set; }

            public string НаименованиеПолучателяКраткое { get; set; }

            public string АдресКонтрагента { get; set; }

            public string ПочтовыйАдресКонтрагента { get; set; }

            public string ПочтовыйИндексКонтрагента { get; set; }

            public bool ПечататьАкт { get; set; }

			public string Наименование { get; set; }

			public string РСчетБанка { get; set; }

			public string ОКПО { get; set; }

			public string ОКОНХ { get; set; }
		}
    }
}