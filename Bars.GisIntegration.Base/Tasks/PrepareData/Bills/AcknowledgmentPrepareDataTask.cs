namespace Bars.GisIntegration.Base.Tasks.PrepareData.Bills
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.BillsAsync;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    /// <summary>
    /// Задача подготовки данных сведений о квитировании
    /// </summary>
    public class AcknowledgmentPrepareDataTask : BasePrepareDataTask<importAcknowledgmentRequest>
    {
        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 1000;

        private List<RisAcknowledgment> acknowledgmentsToExport;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var acknowledgmentDataExtractor = this.Container.Resolve<IDataExtractor<RisAcknowledgment>>("AcknowledgmentDataExtractor");

            try
            {
                this.acknowledgmentsToExport = this.RunExtractor(acknowledgmentDataExtractor, parameters);
            }
            finally
            {
                this.Container.Release(acknowledgmentDataExtractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            var itemsToRemove = new List<RisAcknowledgment>();

            foreach (var item in this.acknowledgmentsToExport)
            {
                var validateResult = this.CheckAcknowledgmentsListItem(item);

                if (validateResult.State != ObjectValidateState.Success)
                {
                    result.Add(validateResult);
                    itemsToRemove.Add(item);
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                this.acknowledgmentsToExport.Remove(itemToRemove);
            }

            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importAcknowledgmentRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importAcknowledgmentRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);
                request.Id = Guid.NewGuid().ToString();

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса.
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект запроса</returns>
        private importAcknowledgmentRequest GetRequestObject(
            IEnumerable<RisAcknowledgment> listForImport,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var acknowledgmentTransportGuidDictionary = new Dictionary<string, long>();

            transportGuidDictionary.Add(typeof(RisAcknowledgment), acknowledgmentTransportGuidDictionary);

            return new importAcknowledgmentRequest
            {
                AcknowledgmentRequestInfo = listForImport
                .Select(item => this.PrepareAcknowledgmentRequestInfo(item, acknowledgmentTransportGuidDictionary))
                .ToArray()
            };
        }

        private importAcknowledgmentRequestAcknowledgmentRequestInfo PrepareAcknowledgmentRequestInfo(
            RisAcknowledgment acknowledgment,
            Dictionary<string, long> transportGuidDictionary)
        {
            var transportGuid = Guid.NewGuid().ToString();
            transportGuidDictionary.Add(transportGuid, acknowledgment.Id);


            var capitalRepairChargeDomain = this.Container.ResolveDomain<RisCapitalRepairCharge>();

            using (this.Container.Using(capitalRepairChargeDomain))
            {
                object item;
                var capitalRepairChargePayable = capitalRepairChargeDomain.GetAll().Where(x => x.PaymentDocument == acknowledgment.PaymentDocument).Sum(x => x.TotalPayable);

                if (acknowledgment.Notification.RisPaymentDocument != null
                    && acknowledgment.PaymentDocument == acknowledgment.Notification.RisPaymentDocument
                    && capitalRepairChargePayable == acknowledgment.Notification.Amount)
                {
                    item = this.GetPaymentDocumentAckItem(acknowledgment);
                }
                else
                {
                    item = new AcknowledgmentRequestInfoTypeAckImpossible
                    {
                        PaymentDocumentID = acknowledgment.PaymentDocument?.Guid
                    };
                }

                return new importAcknowledgmentRequestAcknowledgmentRequestInfo
                {
                    NotificationsOfOrderExecutionGUID = acknowledgment.Notification.Guid,
                    Item = item,
                    TransportGUID = transportGuid
                };
            }
        }

        /// <summary>
        /// Получить Item для AcknowledgmentRequestInfo для платежного документа
        /// </summary>
        /// <param name="acknowledgment">Сведения о квитировании</param>
        /// <returns>Item для AcknowledgmentRequestInfo для платежного документа</returns>
        private AcknowledgmentRequestInfoTypePaymentDocumentAck GetPaymentDocumentAckItem(RisAcknowledgment acknowledgment)
        {
            string item;
            ItemChoiceType1 itemType;

            if (!acknowledgment.HSType.IsEmpty())
            {
                item = acknowledgment.HSType;
                itemType = ItemChoiceType1.HSType;
            }
            else if (!acknowledgment.MSType.IsEmpty())
            {
                item = acknowledgment.MSType;
                itemType = ItemChoiceType1.MSType;
            }
            else
            {
                item = acknowledgment.ASType;
                itemType = ItemChoiceType1.ASType;
            }

            return new AcknowledgmentRequestInfoTypePaymentDocumentAck
            {
                PaymentDocumentID = acknowledgment.PaymentDocument?.Guid,
                Amount = decimal.Round(acknowledgment.Amount, 2),
                Item = item,
                ItemElementName = itemType,
                PaymentDocumentNumber = acknowledgment.PaymentDocument?.PaymentDocumentNumber
            };
        }

        /// <summary>
        /// Проверка данных о квитировании перед импортом
        /// </summary>
        /// <param name="item">Данные о квитировании</param>
        /// <returns>Результат проверки</returns>
        private ValidateObjectResult CheckAcknowledgmentsListItem(RisAcknowledgment item)
        {
            StringBuilder messages = new StringBuilder();

            if (string.IsNullOrEmpty(item.Notification?.Guid))
            {
                messages.Append("OrderId(Notification.Guid) ");
            }

            if (string.IsNullOrEmpty(item.PaymentDocument?.Guid))
            {
                messages.Append("PaymentDocumentNumber(PaymentDocument.Guid) ");
            }

            var typeCount = 0;

            if (!string.IsNullOrEmpty(item.HSType))
            {
                typeCount++;
            }

            if (!string.IsNullOrEmpty(item.MSType))
            {
                typeCount++;
            }

            if (!string.IsNullOrEmpty(item.ASType))
            {
                typeCount++;
            }

            if (typeCount != 1)
            {
                messages.Append("HSType MSType ASType ");
            }

            if (decimal.Round(item.Amount, 2) < 1m)
            {
                messages.Append("Amount ");
            }

            return new ValidateObjectResult
            {
                Id = item.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Сведения о квитировании"
            };
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса.
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<RisAcknowledgment>> GetPortions()
        {
            List<IEnumerable<RisAcknowledgment>> result = new List<IEnumerable<RisAcknowledgment>>();

            if (this.acknowledgmentsToExport.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.acknowledgmentsToExport.Skip(startIndex).Take(AcknowledgmentPrepareDataTask.Portion));
                    startIndex += AcknowledgmentPrepareDataTask.Portion;
                }
                while (startIndex < this.acknowledgmentsToExport.Count);
            }

            return result;
        }
    }
}
