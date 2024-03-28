namespace Bars.GkhRf.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils; 
    using Bars.GkhRf.Entities;
    using Castle.Windsor;

    public class TransferRfService : ITransferRfService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Копирование заявки на перечисление средств
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult Copy(BaseParams baseParams)
        {
            var transferRfRecordId = baseParams.Params.GetAs<long>("transferRfRecordId", 0);
            var transferRfService = Container.Resolve<IDomainService<RequestTransferRf>>();
            var transferFundService = Container.Resolve<IDomainService<TransferFundsRf>>();

            var rec = transferRfService.Get(transferRfRecordId);

            if (transferRfRecordId != 0 && rec != null)
            {
                var newRec = new RequestTransferRf
                    {
                       ContractRf = rec.ContractRf,
                       ContragentBank = rec.ContragentBank,
                       DateFrom = rec.DateFrom,
                       DocumentName = rec.DocumentName,
                       DocumentNum = 0, // присваивается по порядку
                       ManagingOrganization = rec.ManagingOrganization,
                       File = rec.File,
                       Perfomer = rec.Perfomer,
                       ExternalId = rec.ExternalId,
                       ProgramCr = rec.ProgramCr,
                       State = rec.State,
                       TypeProgramRequest = rec.TypeProgramRequest
                    };

                transferRfService.Save(newRec);

                // Дома, включенные в заявку
                var transferFundsRf = transferFundService.GetAll().Where(x => x.RequestTransferRf.Id == rec.Id);

                foreach (var transferFundRf in transferFundsRf)
                {
                    transferFundService.Save(new TransferFundsRf
                        {
                            PayAllocate = transferFundRf.PayAllocate,
                            PersonalAccount = transferFundRf.PersonalAccount,
                            RealityObject = transferFundRf.RealityObject,
                            RequestTransferRf = newRec,
                            WorkKind = transferFundRf.WorkKind
                        });
                }

                Container.Release(transferRfService);
                Container.Release(transferFundService);

                return new BaseDataResult(newRec);
            }
          
            return new BaseDataResult(false, string.Format("Не найдена запись с id ={0}", transferRfRecordId));
        }
    }
}
