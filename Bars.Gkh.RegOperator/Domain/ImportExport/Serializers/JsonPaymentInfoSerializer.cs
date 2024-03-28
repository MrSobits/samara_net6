namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Mapping;
    using Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount;
    using Castle.Windsor;
    using Newtonsoft.Json;

    /// <summary>
    /// Сериализатор информации об оплатах платежных агентов для формата JSON.
    /// Пример десериализуемого файла:
    /// {
    ///    "agent_id":"77384631",
    ///    "reg_num": 4321,
    ///    "reg_date":"01.08.2014",
    ///    "reg_sum":12345.354,
    ///    "payments":[
    ///        { 
    ///            "paid_penalty":0,
    ///            "paid_sum":500,
    ///            "month":"Октябрь",
    ///            "payment_date":"15.10.2014",
    ///            "payment_type":10,
    ///            "personal_acc":"130000005",
    ///            "year":2014
    ///        }
    ///    ]
    ///}
    /// </summary>
    public class JsonPaymentInfoSerializer : DefaultImportExportSerializer<PersonalAccountPaymentInfoIn>
    {
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="container">Контейнер</param>
        public JsonPaymentInfoSerializer(IWindsorContainer container)
            : base(container)
        {
        }

		/// <summary>
		/// Десериализовать
		/// </summary>
		/// <param name="data">Дата</param>
		/// <param name="format">Формат</param>
		/// <param name="fileName">Имя файла</param>
		/// <param name="extraParams">Экстра параметры</param>
		/// <returns>Результат импорта</returns>
        public override ImportResult<PersonalAccountPaymentInfoIn> Deserialize(Stream data, IImportMap format,
            string fileName = null, DynamicDictionary extraParams = null)
        {
            using (var reader = new StreamReader(data))
            {
                var text = reader.ReadToEnd();
                var proxy = JsonConvert.DeserializeObject<JsonPaymentsInProxy>(text);
                var result = new ImportResult<PersonalAccountPaymentInfoIn>();

                result.GeneralData["AgentId"] = proxy.AgentId;

                var paymentAgentDomain = Container.ResolveDomain<PaymentAgent>();
                using (Container.Using(paymentAgentDomain))
                {
                    result.GeneralData["AgentName"] = paymentAgentDomain.GetAll()
                            .Where(x => x.Code == proxy.AgentId)
                            .Select(x => x.Contragent.Name)
                            .FirstOrDefault();
                }

                result.GeneralData["RegNum"] = proxy.RegNum;
                result.GeneralData["RegDate"] = proxy.RegDate;
                result.GeneralData["RegSum"] = proxy.RegSum;

                result.FileDate = proxy.RegDate.ToDateTime();
                result.FileNumber = proxy.RegNum;

                var resultRows = new List<ImportRow<PersonalAccountPaymentInfoIn>>();
                proxy.Payments.ForEach(x => resultRows.Add(new ImportRow<PersonalAccountPaymentInfoIn>
                {
                    Value = x
                }));

                result.Rows = resultRows;
                return result;
            }
        }

		/// <summary>
		/// Прокси-класс
		/// </summary>
        private class JsonPaymentsInProxy
        {
            public JsonPaymentsInProxy()
            {
                Payments = new JsonPaymentInRecord[0];
            }

            [JsonProperty(PropertyName = "agent_id")]
            public string AgentId { get; set; }

            [JsonProperty(PropertyName = "reg_num")]
            public string RegNum { get; set; }

            [JsonProperty(PropertyName = "reg_date")]
            public string RegDate { get; set; }

            [JsonProperty(PropertyName = "reg_sum")]
            public decimal RegSum { get; set; }

            [JsonProperty(PropertyName = "payments")]
            public JsonPaymentInRecord[] Payments { get; set; }
        }

		/// <summary>
		/// Прокси-класс
		/// </summary>
        public class JsonPaymentInRecord : PersonalAccountPaymentInfoIn
        {
			/// <summary>
			/// Тип абонента
			/// </summary>
            public override AccountType OwnerType
            {
                get { return AccountType.Personal; }
                set { }
            }

			/// <summary>
			/// Списанная пеня
			/// </summary>
            [JsonProperty(PropertyName = "paid_penalty")]
            public decimal? charged_penalty
            {
                get { return PenaltyPaid; }
                set { PenaltyPaid = value.GetValueOrDefault(); }
            }

			/// <summary>
			/// Списанная сумма
			/// </summary>
			[JsonProperty(PropertyName = "paid_sum")]
            public decimal? charged_sum
            {
                get { return TargetPaid; }
                set { TargetPaid = value.GetValueOrDefault(); }
            }

			/// <summary>
			/// Месяц
			/// </summary>
			[JsonProperty(PropertyName = "month")]
            public string month { get; set; }

			/// <summary>
			/// Дата платежа
			/// </summary>
			[JsonProperty(PropertyName = "payment_date")]
            public string payment_date
            {
                get { return PaymentDate.ToShortDateString(); }
                set
                {
                    var date = DateTime.MinValue;

                    if (!string.IsNullOrEmpty(value))
                    {
                        DateTime.TryParse(value, out date);
                    }

                    PaymentDate = date;
                }
            }

			/// <summary>
			/// Тип платежа
			/// </summary>
			[JsonProperty(PropertyName = "payment_type")]
            public string payment_type { get; set; }

			/// <summary>
			/// Лицевой счет
			/// </summary>
			[JsonProperty(PropertyName = "personal_acc")]
            public string personal_acc
            {
                get { return AccountNumber; }
                set { AccountNumber = value; }
            }

			/// <summary>
			/// Год
			/// </summary>
			[JsonProperty(PropertyName = "year")]
            public string year { get; set; }
        }
    }
}
