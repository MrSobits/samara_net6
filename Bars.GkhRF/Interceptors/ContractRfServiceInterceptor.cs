namespace Bars.GkhRf.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhRf.Entities;

    public class ContractRfServiceInterceptor : EmptyDomainInterceptor<ContractRf>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ContractRf> service, ContractRf entity)
        {
            var contrRfObjServ = Container.Resolve<IDomainService<ContractRfObject>>();
            var transfRfObjServ = Container.Resolve<IDomainService<TransferRf>>();
            var reqTransRfObjServ = Container.Resolve<IDomainService<RequestTransferRf>>();

            try
            {
                contrRfObjServ.GetAll().Where(x => x.ContractRf.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => contrRfObjServ.Delete(x));

                transfRfObjServ.GetAll().Where(x => x.ContractRf.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => transfRfObjServ.Delete(x));

                reqTransRfObjServ.GetAll().Where(x => x.ContractRf.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => reqTransRfObjServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(contrRfObjServ);
                Container.Release(transfRfObjServ);
                Container.Release(reqTransRfObjServ);
            }
        }
    }
}
