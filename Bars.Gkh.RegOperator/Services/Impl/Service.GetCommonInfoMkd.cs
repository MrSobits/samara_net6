namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System.Linq;
    using Gkh.Enums;
    using GkhCr.Entities;
    using B4;
    using Entities;
    using DataContracts;
    using Gkh.Services.DataContracts;
    using System.Collections.Generic;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Utils;

    public partial class Service
    {
        /// <summary>
        /// DomainService Муниципальное образование
        /// </summary>
        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        /// <summary>
        /// DomainService Муниципальное образование
        /// </summary>
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        /// <summary>
        /// DomainService Счет начислений дома
        /// </summary>
        public IDomainService<RealityObjectChargeAccount> RealityObjectChargeAccountDomain { get; set; }

        /// <summary>
        /// DomainService Вид работы КР
        /// </summary>
        public IDomainService<TypeWorkCr> TypeWorkCrDomainService { get; set; }

        /// <summary>
        /// GetCommonInfoMkd
        /// </summary>
        /// <param name="sYear"></param>
        /// <returns></returns>
        public CommonInfoMkd GetCommonInfoMkd(string sYear)
        {

            var roInPrograms = this.Container.Resolve<IRealityObjectsInPrograms>();
            if (roInPrograms == null)
            {
                return new CommonInfoMkd { Result = Result.DataNotFound };
            }

            var result = Result.NoErrors;

            var year = sYear.ToInt();

            var dataMo = this.MunicipalityDomain.GetAll()
                .Select(x => new {x.Id, x.Name})
                .AsEnumerable();

            var munInions = new List<MunInion>();
            foreach (var moItem in dataMo)
            {
                var mo = moItem;
                var munInion = new MunInion {Name = mo.Name};

                var mkdData = this.RealityObjectDomain.GetAll()
                    .Where(x => x.Municipality.Id == mo.Id)
                    .Where(x => x.TypeHouse == TypeHouse.ManyApartments)
                    .Select(x => new
                    {
                        x.Id,
                        x.AreaMkd,
                        x.ConditionHouse,
                        x.NumberLiving
                    }).ToArray();

                var mkdIds = mkdData.Select(x => x.Id).ToArray();

                var mkd = new Mkd {
                    TotalCount = mkdData.Count(),
                    TotalArea = mkdData.SafeSum(x => x.AreaMkd.ToDecimal()).RegopRoundDecimal(2)
                };

                var emergData = mkdData.Where(x => x.ConditionHouse == ConditionHouse.Emergency).ToArray();
                var emergMkd = new Mkd
                {
                    TotalCount = emergData.Count(),
                    TotalArea = emergData.SafeSum(x => x.AreaMkd.ToDecimal()).RegopRoundDecimal(2)
                };

                var inhabitants = new Count
                {
                    TotalCount = mkdData.SafeSum(x => x.NumberLiving.ToDecimal())
                };

                var lprobj = roInPrograms.GetInPublishedProgram();
                var dictLongPrObj = lprobj
                    .Where(x => mkdIds.Any() && mkdIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .Distinct()
                    .ToArray();

                var prInclData = mkdData
                    .Where(x => dictLongPrObj.Contains(x.Id))
                    .Select(x => new {x.AreaMkd, x.NumberLiving})
                    .ToArray();

                var programIncludedMkd = new Mkd
                {
                    TotalCount = prInclData.Count(),
                    TotalArea = prInclData.SafeSum(x => x.AreaMkd.ToDecimal()).RegopRoundDecimal(2)
                };

                var programIncludedInhabitants = new Count
                {
                    TotalCount = prInclData.SafeSum(x => x.NumberLiving.ToDecimal())
                };

                var chargeAccount = this.RealityObjectChargeAccountDomain.GetAll()
                    .Where(x => mkdIds.Any() && mkdIds.Contains(x.RealityObject.Id))
                    .Select(x => new
                    {
                        ChargeTotal = x.Operations.Sum(y => y.ChargedTotal),
                        x.PaidTotal
                    });

                var creditPayments = new SumType
                {
                    Sum = chargeAccount.SafeSum(x => x.ChargeTotal)
                };

                var paidPayments = new SumType
                {
                    Sum = chargeAccount.SafeSum(x => x.PaidTotal)
                };

                var plannedMkdKrIds = roInPrograms.GetInShortProgram(year)
                    .Where(x => mkdIds.Any() && mkdIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .Distinct()
                    .ToArray();

                var plannedMkdKrArea = mkdData
                    .Where(x => plannedMkdKrIds.Contains(x.Id))
                    .Select(x => x.AreaMkd)
                    .ToArray();

                var totalCost = roInPrograms.GetShortProgramRecordData()
                    .Where(x => plannedMkdKrIds.Contains(x.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.TotalCost).Sum());

                var plannedMkdKr = new MkdKr
                {
                    TotalCount = plannedMkdKrArea.Count(),
                    TotalArea = plannedMkdKrArea.Sum().ToDecimal().RegopRoundDecimal(2),
                    TotalCost = totalCost.Values.Sum().ToDecimal().RegopRoundDecimal(2)
                };

                var factMkdKrData1 = this.TypeWorkCrDomainService.GetAll()
                    .Where(x => plannedMkdKrIds.Contains(x.ObjectCr.RealityObject.Id))
                    .Select(x => new {x.ObjectCr.RealityObject.Id, x.PercentOfCompletion, x.Sum})
                    .AsEnumerable()
                    .GroupBy(x => x.Id);

                var factMkdKrData = factMkdKrData1.AsEnumerable()
                    .Where(x => x.Count()*100 == x.Select(z => z.PercentOfCompletion).Sum())
                    .ToDictionary(x => x.Key, y => y.Select(z => z.Sum).Sum());
                    
                var factMkdKr = new MkdKr
                {
                    TotalCount = factMkdKrData.Count(),
                    TotalArea = mkdData.Where(x => factMkdKrData.ContainsKey(x.Id)).Select(x => x.AreaMkd).Sum().ToDecimal().RegopRoundDecimal(2),
                    TotalCost = factMkdKrData.Values.Sum().ToDecimal().RegopRoundDecimal(2)
                };

                munInion.Mkd = mkd;
                munInion.EmergencyMkd = emergMkd;
                munInion.Inhabitants = inhabitants;
                munInion.ProgramIncludedMkd = programIncludedMkd;
                munInion.ProgramIncludedInhabitants = programIncludedInhabitants;
                munInion.CreditPayments = creditPayments;
                munInion.PaidPayments = paidPayments;
                munInion.PlannedMkdKKr = plannedMkdKr;
                munInion.FactMkdKr = factMkdKr;

                munInions.Add(munInion);
            }

            return new CommonInfoMkd { MunInion = munInions.ToArray(), Result = result };
        }
    }
}