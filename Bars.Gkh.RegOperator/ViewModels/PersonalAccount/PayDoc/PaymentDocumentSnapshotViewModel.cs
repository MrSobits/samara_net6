namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount.PayDoc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.Utils;

    using Enums;

    internal class PaymentDocumentSnapshotViewModel : BaseViewModel<PaymentDocumentSnapshot>
    {
        #region Overrides of BaseViewModel<PaymentDocumentSnapshot>

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<PaymentDocumentSnapshot> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            var periodId = baseParams.Params.GetAsId("periodId");
            var onlyLegal = baseParams.Params.GetAs<bool>("onlyLegal");
            var onlyBase = baseParams.Params.GetAs<bool>("onlyBase");

            var data = domainService.GetAll()
                .Where(x => x.Period.Id == periodId)
                .WhereIf(onlyLegal, x => x.OwnerType == PersonalAccountOwnerType.Legal)
                .WhereIf(onlyBase, x => x.IsBase)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.DocNumber,
                        x.HolderType,
                        x.HolderId,
                        x.DocDate,
                        x.Payer,
                        x.Municipality,
                        x.Settlement,
                        x.Address,
                        x.OwnerType,
                        x.PaymentReceiverAccount,
                        x.TotalCharge,
                        x.PaymentState,
                        IsBase = x.IsBase ? YesNo.Yes : YesNo.No,
                        x.AccountCount,
                        x.SendingEmailState,
                        x.HasEmail,
                        x.OwnerInn
                    })
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }

        #endregion
    }
}