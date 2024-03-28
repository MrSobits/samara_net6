namespace Bars.GkhRf.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhCr.Entities;
    using Bars.GkhRf.Entities;

    public class TransferFundsRfInterceptor : EmptyDomainInterceptor<TransferFundsRf>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<TransferFundsRf> service, TransferFundsRf entity)
        {
            var reqTransferRf = entity.RequestTransferRf;
            var realObj = entity.RealityObject;

            var serviceObjectCr = Container.Resolve<IRepository<ObjectCr>>();
            var serviceObjCrFinSources = Container.Resolve<IDomainService<FinanceSourceResource>>();

            var anotherRequests =
                Container.Resolve<IDomainService<TransferFundsRf>>().GetAll()
                         .Where(x => x.RealityObject.Id == realObj.Id && x.Id != entity.Id)
                         .Where(x => x.RequestTransferRf.TypeProgramRequest == reqTransferRf.TypeProgramRequest)
                         .Where(x =>
                             x.RequestTransferRf.ProgramCr.Id == entity.RequestTransferRf.ProgramCr.Id
                             && x.RequestTransferRf.DateFrom.Value.Year == DateTime.Now.Year)
                         .Select(x => new { Doc = x.RequestTransferRf.DocumentNum, x.Sum })
                         .AsEnumerable()
                         .Aggregate(new { Docs = string.Empty, Sum = 0M }, (x, y) => new { Docs = string.Format("{0}, {1}", x.Docs, y.Doc), Sum = x.Sum + (y.Sum.HasValue ? y.Sum.Value : 0M) });

            var requestFinSources =
                Container.Resolve<IDomainService<LimitCheckFinSource>>().GetAll()
                         .Where(x => x.LimitCheck.TypeProgram == reqTransferRf.TypeProgramRequest)
                         .Select(x => x.FinanceSource.Id)
                         .Distinct()
                         .ToList();

            var objCr = serviceObjectCr.GetAll().FirstOrDefault(x => x.RealityObject.Id == realObj.Id && x.ProgramCr.Id == reqTransferRf.ProgramCr.Id);

            if (objCr == null)
            {
                throw new ValidationException(string.Format("{0} не включена в указанную программу КР", realObj.Address));
            }

            var finSource = serviceObjCrFinSources.GetAll().Where(x => requestFinSources.Contains(x.FinanceSource.Id));

            var finSourceResource =
                finSource.Where(x => x.ObjectCr.Id == objCr.Id)
                         .Select(x => x.OwnerResource)
                         .Sum()
                         .GetValueOrDefault(0);

            var hasMessage = false;

            var res = string.Empty;
            if (finSource.Any() && finSourceResource < entity.Sum + anotherRequests.Sum)
            {
                hasMessage = true;
                res = string.Format(
                    "{0} - заявка № {1}; превышает лимит по программе КР", realObj.Address, reqTransferRf.DocumentNum + anotherRequests.Docs);
            }

            if (hasMessage)
            {
                throw new ValidationException(res);
            }

            return Success();
        }
    }
}
