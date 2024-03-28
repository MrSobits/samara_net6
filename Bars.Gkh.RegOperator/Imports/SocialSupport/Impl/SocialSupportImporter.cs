namespace Bars.Gkh.RegOperator.Imports.SocialSupport.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using Newtonsoft.Json;

	/// <summary>
	/// Импортер социальной поддержки
	/// </summary>
    public class SocialSupportImporter : IBillingFileImporter
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Провайдер импорта документов из банка
		/// </summary>
		public IBankDocumentImportProvider BankDocumentImportProvider { get; set; }

		/// <summary>
		/// Имя файла
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		/// Порядок
		/// </summary>
        public int Order { get; private set; }

		/// <summary>
		/// Импортировать
		/// </summary>
        public void Import(Stream fileStream, string archiveName, ILogImport logger = null, IProgressIndicator indicator = null, long userId = 0, object param = null)
        {
            var chargePeriodRepo = this.Container.Resolve<IChargePeriodRepository>();
            try
            {
                var period = chargePeriodRepo.GetCurrentPeriod();
                var paymentDate = period.StartDate;

                using (var r = new StreamReader(fileStream, Encoding.UTF8))
                {
                    var json = r.ReadToEnd();
                    var paymentsData = JsonConvert.DeserializeObject<PaymentsData>(json);

                    if (paymentsData == null)
                    {
                        logger.Error("Ошибка", "Некорректный формат импортируемого файла");
                        return;
                    }

                    var data = new List<PersonalAccountPaymentInfoIn>();

                    foreach (var socialSupportData in paymentsData.Payments)
                    {
                        var persAccPaymInfoIn = new PersonalAccountPaymentInfoIn
                        {
                            OwnerType = PersonalAccountPaymentInfoIn.AccountType.Undefined,
                            AccountNumber = socialSupportData.PersonalNumber,
                            PaymentDate = paymentDate,
                            SocialSupport = socialSupportData.ChargedSum,
                            Fio = socialSupportData.Fio
                        };

                        data.Add(persAccPaymInfoIn);
                    }

                    this.BankDocumentImportProvider.CreateBankDocumentImport(data, BankDocumentImportType.SocialSupport, logger);
                }
            }
            catch
            {
                logger.Error("Ошибка", "Некорректный формат импортируемого файла");
                throw new Exception("Некорректный формат переданного файла");
            }
        }

        [JsonObject]
        private class PaymentsData
        {
            [JsonProperty("payments")]
            public List<SocialSupportData> Payments { get; set; }
        }

        private class SocialSupportData
        {
            [JsonProperty("account_num")]
            public string AccountNumber { get; set; }

            [JsonProperty("personal_acc")]
            public string PersonalNumber { get; set; }

            [JsonProperty("street")]
            public string Street { get; set; }

            [JsonProperty("house")]
            public string House { get; set; }

            [JsonProperty("corpus")]
            public string Housing { get; set; }

            [JsonProperty("flat")]
            public string Flat { get; set; }

            [JsonProperty("fio")]
            public string Fio { get; set; }

            [JsonProperty("charged_sum")]
            public decimal ChargedSum { get; set; }
        }
    }
}