namespace Bars.GisIntegration.Base.Tasks.PrepareData.Bills
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    /// <summary>
    /// Методы проверки данных
    /// </summary>
    public partial class PaymentDocumentPrepareDataTask
    {
        private static List<ValidateObjectResult> ValidateObjectDicts<TKey, T>(IDictionary<TKey, List<T>> objectDict, Func<T, ValidateObjectResult> validateObjectFunc)
            where T : BaseRisEntity
        {
            var result = new List<ValidateObjectResult>();
            var objectsToRemove = new List<KeyValuePair<TKey, T>>();

            foreach (var kvp in objectDict)
            {
                foreach (var elem in kvp.Value)
                {
                    var validateResult = validateObjectFunc(elem);

                    if (validateResult.State != ObjectValidateState.Success)
                    {
                        result.Add(validateResult);
                        objectsToRemove.Add(new KeyValuePair<TKey, T>(kvp.Key, elem));
                    }
                }
            }

            foreach (var objToRemove in objectsToRemove)
            {
                objectDict.Get(objToRemove.Key).Remove(objToRemove.Value);
            }

            return result;
        }

        private static List<ValidateObjectResult> ValidateObjectList<T>(ICollection<T> objectList, Func<T, ValidateObjectResult> validateObjectFunc)
            where T : BaseRisEntity
        {
            var result = new List<ValidateObjectResult>();
            var objectsToRemove = new List<T>();

            foreach (var obj in objectList)
            {
                var validateResult = validateObjectFunc(obj);

                if (validateResult.State != ObjectValidateState.Success)
                {
                    result.Add(validateResult);
                    objectsToRemove.Add(obj);
                }
            }

            foreach (var objToRemove in objectsToRemove)
            {
                objectList.Remove(objToRemove);
            }

            return result;
        }

        private ValidateObjectResult ValidatePaymentDocument(RisPaymentDocument document)
        {
            var messages = new StringBuilder();

            if (document.State == PaymentDocumentState.Withdraw && document.Guid.IsEmpty())
            {
                messages.Append("document.State == Withdraw && Guid = null ");
            }

            if (document.AddressInfo.IsNull())
            {
                messages.Append($"{nameof(document.AddressInfo)} ");
            }
            else
            {
                if (document.AddressInfo.TotalSquare < 0)
                {
                    messages.Append("AddressInfo.TotalSquare ");
                }
            }

            if (document.PaymentInformation.IsNull())
            {
                messages.Append($"{nameof(document.PaymentInformation)} ");
            }
            else
            {
                if (document.PaymentInformation.BankBik.IsEmpty())
                {
                    messages.Append("PaymentInformation.BankBik ");
                }

                if (document.PaymentInformation.BankName.IsEmpty())
                {
                    messages.Append("PaymentInformation.BankName ");
                }

                if (document.PaymentInformation.Recipient.IsEmpty())
                {
                    messages.Append("PaymentInformation.Recipient ");
                }

                if (document.PaymentInformation.RecipientInn.IsEmpty())
                {
                    messages.Append("PaymentInformation.RecipientInn ");
                }

                if (document.PaymentInformation.OperatingAccountNumber.IsEmpty())
                {
                    messages.Append("PaymentInformation.OperatingAccountNumber ");
                }

                if (document.PaymentInformation.CorrespondentBankAccount.IsEmpty())
                {
                    messages.Append("PaymentInformation.CorrespondentBankAccount ");
                }
            }

            return new ValidateObjectResult
            {
                Id = document.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Платежный документ"
            };
        }

        private ValidateObjectResult ValidateCapitalRepairDebt(RisCapitalRepairDebt capitalRepairDebt)
        {
            StringBuilder messages = new StringBuilder();

            if (capitalRepairDebt.Month == 0)
            {
                messages.Append($"{nameof(capitalRepairDebt.Month)} ");
            }

            if (capitalRepairDebt.Year == 0)
            {
                messages.Append($"{nameof(capitalRepairDebt.Year)} ");
            }

            return new ValidateObjectResult
            {
                Id = capitalRepairDebt.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Задолженность за капитальный ремонт"
            };
        }

        private ValidateObjectResult ValidateCapitalRepairCharge(RisCapitalRepairCharge capitalRepairCharge)
        {
            StringBuilder messages = new StringBuilder();

            return new ValidateObjectResult
            {
                Id = capitalRepairCharge.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Начисление за капитальный ремонт"
            };
        }
    }
}