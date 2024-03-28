namespace Bars.GkhCr.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;

    public class SpecialFinanceSourceResourceService : ISpecialFinanceSourceResourceService
    {
        public  IWindsorContainer Container { get; set; }

        public IDomainService<SpecialFinanceSourceResource> FinSourceDomain { get; set; }
        public IDomainService<SpecialTypeWorkCr> TypeWorkCrDomain { get; set; }
        public IDomainService<SpecialObjectCr> ObjectCrDomain { get; set; }

        public IDataResult AddFinSources(BaseParams baseParams)
        {
            var years = baseParams.Params.GetAs<int[]>("years");
            var record = baseParams.Params.GetAs<SpecialFinanceSourceResource>("record");
            var typeWorkId = baseParams.Params.GetAs<long>("typeWorkId");
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId");

            var existRecs = this.FinSourceDomain.GetAll()
                .Where(x => x.TypeWorkCr.Id == typeWorkId)
                .AsEnumerable()
                .Where(x => years.Contains(x.Year.ToInt()))
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, y => y.First());

            var listToSave = new List<SpecialFinanceSourceResource>();

            if (years == null)
            {
                record.ObjectCr = this.ObjectCrDomain.Load(objectCrId);
                record.TypeWorkCr = this.TypeWorkCrDomain.Load(typeWorkId);

                this.FinSourceDomain.Save(record);
            }
            else
            {
                foreach (var year in years)
                {
                    var newRec = new SpecialFinanceSourceResource
                    {
                        ObjectCr = this.ObjectCrDomain.Load(objectCrId),
                        TypeWorkCr = this.TypeWorkCrDomain.Load(typeWorkId),
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

                TransactionHelper.InsertInManyTransactions(this.Container, listToSave);
            }

            return new BaseDataResult();
        }
    }
}