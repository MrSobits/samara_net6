namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    internal class TransferCtrViewModel : BaseViewModel<TransferCtr>
    {
        /// <summary>
        /// Сервис для работы с сущностью "Заявка на перечисление средств подрядчикам"
        /// </summary>
        public ITransferCtrService TransferCtrService { get; set; }

        /// <summary>
        /// Получение списка заявок
        /// </summary>
        /// <param name="domainService">Домен-сервис заявки</param>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public override IDataResult List(IDomainService<TransferCtr> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = this.TransferCtrService.List(baseParams);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Получение Заявки для редактирования
        /// </summary>
        /// <param name="domainService">Домен-сервис заявки</param>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public override IDataResult Get(IDomainService<TransferCtr> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId());

            return new BaseDataResult(
                new
                {
                    obj.Id,
                    obj.File,
                    obj.FinSource,
                    obj.BudgetMu,
                    obj.BudgetSubject,
                    Builder = new
                    {
                        obj.Builder.Id,
                        obj.Builder.Contragent,
                        ContragentName = obj.Builder.Contragent.Name,
                        ContragentId = obj.Builder.Contragent.Id,
                        obj.Builder.Contragent.Inn,
                        obj.Builder.Contragent.Kpp,
                        obj.Builder.Contragent.Phone,
                    },
                    Contract = new
                    {
                        obj.Contract.Id,
                        Text = string.Format("№ {0} от {1}", obj.Contract.DocumentNum, obj.Contract.DocumentDateFrom.ToDateString())
                    },
                    ContragentBank = new
                    {
                        obj.ContragentBank.Id,
                        obj.ContragentBank.Name,
                        obj.ContragentBank.CorrAccount,
                        obj.ContragentBank.SettlementAccount,
                        obj.ContragentBank.Contragent.Inn,
                        obj.ContragentBank.Contragent.Kpp,
                        obj.ContragentBank.Bik
                    },
                    ContragentInn = obj.Builder.Contragent.Inn,
                    ContragentKpp = obj.Builder.Contragent.Kpp,
                    ContragentPhone = obj.Builder.Contragent.Phone,
                    obj.DateFrom,
                    obj.DocumentName,
                    obj.DocumentNum,
                    obj.DocumentNumPp,
                    obj.DateFromPp,
                    ObjectCr = new
                    {
                        obj.ObjectCr.Id,
                        RealityObjName = obj.ObjectCr.RealityObject.Address
                    },
                    obj.PaymentType,
                    obj.Perfomer,
                    obj.ProgramCr,
                    obj.TypeProgramRequest,
                    obj.State,
                    TypeWorkCr = obj.TypeWorkCr == null
                        ? null
                        : new {obj.TypeWorkCr.Id, WorkName = obj.TypeWorkCr.Work.Name},
                    obj.PaymentPurposeDescription,
                    RegOperator = obj.RegOperator == null
                        ? null
                        : new
                        {
                            obj.RegOperator.Id,
                            Contragent = obj.RegOperator.Contragent.Name,
                            ContragentId = obj.RegOperator.Contragent.Id
                        },
                    obj.RegopCalcAccount,
                    obj.ProgramCrType,
                    obj.Comment,
                    obj.KindPayment,
                    obj.Sum,
                    obj.PaidSum,
                    obj.PaymentDate,
                    obj.TransferGuid,
                    obj.IsEditPurpose,
                    obj.PaymentPriority,
                    obj.TypeCalculationNds
                });
        }
    }
}