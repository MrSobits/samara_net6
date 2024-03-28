namespace Bars.GisIntegration.Base.Tasks.PrepareData.Bills
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.BillsAsync;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.GisIntegration.Base.Utils;

    /// <summary>
    /// Задача подготовки данных сведений о платежных документах
    /// </summary>
    public partial class PaymentDocumentPrepareDataTask : BasePrepareDataTask<importPaymentDocumentRequest>
    {
        private const int portionSize = 1000;

        private IList<RisPaymentDocument> paymentDocuments;
        private IDictionary<Tuple<short, int>, List<RisPaymentDocument>> documentsByPeriods;

        private Dictionary<RisPaymentDocument, List<RisCapitalRepairDebt>>
           capitalRepairDebtsByPaymentDocument;

        private Dictionary<RisPaymentDocument, List<RisCapitalRepairCharge>>
         capitalRepairChargesByPaymentDocument;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var risPaymentDocumentExtractor = this.Container.Resolve<IDataExtractor<RisPaymentDocument>>("RisPaymentDocumentDataExtractor");
            var risCapitalRepairChargeDataExtractor = this.Container.Resolve<IDataExtractor<RisCapitalRepairCharge>>("RisCapitalRepairChargeDataExtractor");
            var risCapitalRepairDebtDataExtractor = this.Container.Resolve<IDataExtractor<RisCapitalRepairDebt>>("RisCapitalRepairDebtDataExtractor");

            try
            {
                this.paymentDocuments = this.RunExtractor(risPaymentDocumentExtractor, parameters);
                parameters.Add("extractedPayDocs", this.paymentDocuments);

                this.documentsByPeriods = this.paymentDocuments
                    .GroupBy(x => new Tuple<short, int>(x.PeriodYear, x.PeriodMonth))
                    .ToDictionary(x => x.Key, x => x.ToList());

                this.capitalRepairChargesByPaymentDocument = this.RunExtractor(risCapitalRepairChargeDataExtractor, parameters)
                  .GroupBy(x => x.PaymentDocument)
                  .ToDictionary(x => x.Key, x => x.ToList());

                this.capitalRepairDebtsByPaymentDocument = this.RunExtractor(risCapitalRepairDebtDataExtractor, parameters)
                    .GroupBy(x => x.PaymentDocument)
                    .ToDictionary(x => x.Key, x => x.ToList());
            }
            finally
            {
                this.Container.Release(risPaymentDocumentExtractor);
                this.Container.Release(risCapitalRepairDebtDataExtractor);
                this.Container.Release(risCapitalRepairChargeDataExtractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            result.AddRange(PaymentDocumentPrepareDataTask.ValidateObjectList(this.paymentDocuments, this.ValidatePaymentDocument));
            result.AddRange(PaymentDocumentPrepareDataTask.ValidateObjectDicts(this.capitalRepairDebtsByPaymentDocument, this.ValidateCapitalRepairDebt));
            result.AddRange(PaymentDocumentPrepareDataTask.ValidateObjectDicts(this.capitalRepairChargesByPaymentDocument, this.ValidateCapitalRepairCharge));

            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importPaymentDocumentRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importPaymentDocumentRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var documentsByPeriod in this.documentsByPeriods)
            {
                var period = documentsByPeriod.Key;
                var docments = documentsByPeriod.Value;

                var transportGuids = new Dictionary<string, long>();

                foreach (var paymentDocumentPortion in docments.Section(PaymentDocumentPrepareDataTask.portionSize))
                {
                    var payments = this.PreparePaymentsRequest(paymentDocumentPortion, transportGuids);

                    var itemsField = new List<object> { period.Item2, period.Item1 };
                    itemsField.AddRange(payments.Item2);
                    itemsField.AddRange(payments.Item1);
                    
                    var request = new importPaymentDocumentRequest
                    {
                        Id = Guid.NewGuid().ToString(),
                        Items = itemsField.ToArray()
                    };

                    result.Add(request, new Dictionary<Type, Dictionary<string, long>>
                    {
                        { typeof(RisPaymentDocument), transportGuids }
                    });
                }
            }

            return result;
        }
    }
}
