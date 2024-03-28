namespace Bars.GkhCr.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using Hmao.DomainService;

    public class FinanceSourceResourceService : IFinanceSourceResourceService
    {
        public  IWindsorContainer Container { get; set; }

        public IDomainService<FinanceSourceResource> FinSourceDomain { get; set; }
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }
        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        public IDataResult AddFinSources(BaseParams baseParams)
        {
            var years = baseParams.Params.GetAs<int[]>("years");
            var record = baseParams.Params.GetAs<FinanceSourceResource>("record");
            var typeWorkId = baseParams.Params.GetAs<long>("typeWorkId");
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId");

            var existRecs = FinSourceDomain.GetAll()
                .Where(x => x.TypeWorkCr.Id == typeWorkId)
                .AsEnumerable()
                .Where(x => years.Contains(x.Year.ToInt()))
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, y => y.First());

            var listToSave = new List<FinanceSourceResource>();

            if (years == null)
            {
                record.ObjectCr = ObjectCrDomain.Load(objectCrId);
                record.TypeWorkCr = TypeWorkCrDomain.Load(typeWorkId);

                FinSourceDomain.Save(record);
            }
            else
            {
                foreach (var year in years)
                {
                    var newRec = new FinanceSourceResource
                    {
                        ObjectCr = ObjectCrDomain.Load(objectCrId),
                        TypeWorkCr = TypeWorkCrDomain.Load(typeWorkId),
                        Year = year
                    };

                    if (existRecs.ContainsKey(year))
                    {
                        newRec = existRecs.Get(year);
                    }

                    newRec.BudgetMu = record.BudgetMu;
                    newRec.BudgetSubject = record.BudgetSubject;
                    newRec.OwnerResource = record.OwnerResource;
                    newRec.FundResource = record.FundResource;
                    newRec.BudgetMuIncome = record.BudgetMuIncome;
                    newRec.BudgetSubjectIncome = record.BudgetSubjectIncome;
                    newRec.FundResourceIncome = record.FundResourceIncome;

                    listToSave.Add(newRec);
                }

                TransactionHelper.InsertInManyTransactions(Container, listToSave);
            }

            return new BaseDataResult();
        }
    }
}