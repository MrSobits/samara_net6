namespace Bars.GkhRf.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    public class TransferFundsRfService : ITransferFundsRfService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddTransferFundsObjects(BaseParams baseParams)
        {
            try
            {
                var requestTransferRfId = baseParams.Params["requestTransferRfId"].ToLong();
                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',');

                if (objectIds.Length > 0)
                {
                    var service = Container.Resolve<IDomainService<TransferFundsRf>>();

                    // получаем у контроллера дома что бы не добавлять их повторно
                    var exsistingRequestTransferRfObjects = service.GetAll().Where(x => x.RequestTransferRf.Id == requestTransferRfId).Select(x => x.RealityObject.Id).ToList();

                    foreach (var id in objectIds)
                    {
                        if (exsistingRequestTransferRfObjects.Contains(id.ToLong()))
                        {
                            continue;
                        }

                        var newId = id.ToLong();

                        var newTransferFundsRf = new TransferFundsRf
                        {
                            RealityObject = new RealityObject { Id = newId },
                            RequestTransferRf = new RequestTransferRf { Id = requestTransferRfId },
                        };

                        service.Save(newTransferFundsRf);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        public IDataResult ListPersonalAccount(BaseParams baseParams)
        {
            var loadParams = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);

            var transferFundId = baseParams.Params.ContainsKey("transferFundId")
                       ? baseParams.Params["transferFundId"].ToLong()
                       : 0;

            TransferFundsRf transferFundsRf = null;

            if (transferFundId != 0)
            {
                transferFundsRf = Container.Resolve<IDomainService<TransferFundsRf>>().Load(transferFundId);
            }
            else
            {
                 var emptyList = new List<PersonalAccount>();
                 return new ListDataResult(emptyList, 0);
            }

            TypeFinanceGroup financeGroup = 0;
            if (transferFundsRf.RequestTransferRf.TypeProgramRequest == TypeProgramRequest.Primary)
            {
                financeGroup = TypeFinanceGroup.ProgramCr;
            }

            if (transferFundsRf.RequestTransferRf.TypeProgramRequest == TypeProgramRequest.Additional
                || transferFundsRf.RequestTransferRf.TypeProgramRequest == TypeProgramRequest.Another)
            {
                financeGroup = TypeFinanceGroup.Other;
            }

            var servicePersonalAccount = Container.Resolve<IDomainService<PersonalAccount>>();
            var data = servicePersonalAccount.GetAll()
               .WhereIf(transferFundId != 0, x => x.ObjectCr.ProgramCr.Id == transferFundsRf.RequestTransferRf.ProgramCr.Id && x.FinanceGroup == financeGroup
                   && x.ObjectCr.RealityObject.Id == transferFundsRf.RealityObject.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Account,
                    FinGroupDisplay = this.GetDisplayEnum(x.FinanceGroup)
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        private string GetDisplayEnum(TypeFinanceGroup enumItem)
        {
            object[] attribs = typeof(TypeFinanceGroup)
                .GetField(enumItem.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attribs.Length > 0)
            {
                return ((DisplayAttribute)attribs[attribs.Length - 1]).Value.ToStr();
            }

            return string.Empty;
        }
    }
}
