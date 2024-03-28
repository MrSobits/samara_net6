namespace Bars.GisIntegration.Base.Tasks.PrepareData.Bills
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.GisIntegration.Base.BillsAsync;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Методы подготовки запроса
    /// </summary>
    public partial class PaymentDocumentPrepareDataTask
    {
        private Tuple<IEnumerable<importPaymentDocumentRequestPaymentDocument>, IEnumerable<importPaymentDocumentRequestPaymentInformation>> PreparePaymentsRequest(
            IEnumerable<RisPaymentDocument> documents,
            IDictionary<string, long> transportGuids)
        {
            var documentsResult = new List<importPaymentDocumentRequestPaymentDocument>();
            var informationsResult = new List<importPaymentDocumentRequestPaymentInformation>();

            foreach (var paymentDocument in documents)
            {
                var transportGuid = Guid.NewGuid().ToString();
                paymentDocument.PaymentInformation.Guid = Guid.NewGuid().ToString();
                //Сведения о платежных реквизитах организации
                var informationRequest = this.PreparePaymentInformationRequest(paymentDocument, transportGuid);
                informationsResult.Add(informationRequest);

                //Платежный документ
                var documentRequest = this.PreparePaymentDocumentRequest(paymentDocument, transportGuid);
                documentsResult.Add(documentRequest);

                transportGuids.Add(transportGuid, paymentDocument.Id);
            }

            return new Tuple<IEnumerable<importPaymentDocumentRequestPaymentDocument>, IEnumerable<importPaymentDocumentRequestPaymentInformation>>(documentsResult, informationsResult);
        }

        private importPaymentDocumentRequestPaymentInformation PreparePaymentInformationRequest(RisPaymentDocument paymentDocument, string guid)
        {
            var paymentInformation = paymentDocument.PaymentInformation;

            return new importPaymentDocumentRequestPaymentInformation
            {
                BankBIK = paymentInformation.BankBik,
                TransportGUID = guid,
                operatingAccountNumber = paymentInformation.OperatingAccountNumber
            };
        }

        private importPaymentDocumentRequestPaymentDocument PreparePaymentDocumentRequest(RisPaymentDocument paymentDocument, string guid)
        {
            var chargeInfoList = new List<object>();

            var addressInfo = new PaymentDocumentTypeAddressInfo
            {
                LivingPersonsNumber = (sbyte)paymentDocument.AddressInfo.LivingPersonsNumber.GetValueOrDefault(),
                ResidentialSquare = decimal.Round(paymentDocument.AddressInfo.ResidentialSquare.GetValueOrDefault(), 2),
                HeatedArea = decimal.Round(paymentDocument.AddressInfo.HeatedArea.GetValueOrDefault(), 2),
                TotalSquare = decimal.Round(paymentDocument.AddressInfo.TotalSquare, 2)
            };

            if (paymentDocument.Operation == RisEntityOperation.Create)
            {
                chargeInfoList.AddRange(this.capitalRepairChargesByPaymentDocument[paymentDocument]
                        .Select(this.PrepareCapitalRepairChargeRequest));
            }
            else if (paymentDocument.Operation == RisEntityOperation.Update)
            {
                chargeInfoList.AddRange(this.capitalRepairDebtsByPaymentDocument[paymentDocument]
                        .Select(this.PrepareCapitalRepairDebtRequest));
            }

            return new importPaymentDocumentRequestPaymentDocument
            {
                AccountGuid = paymentDocument.Account.Guid,
                AddressInfo = addressInfo,
                Items = chargeInfoList.ToArray(),
                //указываем состояние документа, от этого зависит, нужно ли Guid передавать
                ItemElementName = paymentDocument.State == PaymentDocumentState.Expose
                    ? ItemChoiceType.Expose
                    : ItemChoiceType.Withdraw,
                Item = true,

                totalPiecemealPaymentSum = decimal.Round(paymentDocument.TotalPiecemealPaymentSum, 2),
                TransportGUID = Guid.NewGuid().ToString(),
                PaymentInformationKey = guid,
                // передаём guid, если документ отозванный
                //          PaymentDocumentID = paymentDocument.State == PaymentDocumentState.Expose ? paymentDocument.Guid : null,
                PaymentDocumentNumber = paymentDocument.PaymentDocumentNumber   //пока нету в нашей системе, поэтому будет всегда null
            };
        }

        private CapitalRepairType PrepareCapitalRepairChargeRequest(RisCapitalRepairCharge capitalRepairCharge)
        {
            return new CapitalRepairType
            {
                MoneyDiscount = decimal.Round(capitalRepairCharge.MoneyDiscount.GetValueOrDefault(), 2),
                AccountingPeriodTotal = decimal.Round(capitalRepairCharge.AccountingPeriodTotal.GetValueOrDefault(), 2),
                Contribution = decimal.Round(capitalRepairCharge.Contribution.GetValueOrDefault(), 2),
                MoneyRecalculation = decimal.Round(capitalRepairCharge.MoneyRecalculation.GetValueOrDefault(), 2),
                TotalPayable = decimal.Round(capitalRepairCharge.TotalPayable.GetValueOrDefault(), 2)
            };
        }

        private DebtType PrepareCapitalRepairDebtRequest(RisCapitalRepairDebt capitalRepairDebt)
        {
            return new DebtType
            {
                Month = capitalRepairDebt.Month,
                Year = (short)capitalRepairDebt.Year,
                TotalPayable = decimal.Round(capitalRepairDebt.TotalPayable.GetValueOrDefault(), 2)
            };
        }
    }
}