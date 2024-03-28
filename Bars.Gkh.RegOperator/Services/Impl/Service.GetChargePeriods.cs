namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts;

    public partial class Service
    {
        /// <summary>
        /// Период начислений
        /// </summary>
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }

        /// <summary>
        /// Метод получения периодов начисления 
        /// </summary>
        /// <returns></returns>
        public GetChargePeriodsResponse GetChargePeriods()
        {
            var chargePeriods = this.ChargePeriodDomain.GetAll()
                .Where(x => x.IsClosed)
                .Select(x => new ChargePeriodProxy
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Id)
                .ToArray();

            var result = chargePeriods.Length == 0 ? Result.DataNotFound : Result.NoErrors;

            return new GetChargePeriodsResponse { ChargePeriods = chargePeriods, Result = result };
        }
        
        public GetChargePeriodsResponse GetChargePeriodsAll()
        {
            var chargePeriods = this.ChargePeriodDomain.GetAll()
                .Select(x => new ChargePeriodProxy
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Id)
                .ToArray();

            var result = chargePeriods.Length == 0 ? Result.DataNotFound : Result.NoErrors;

            return new GetChargePeriodsResponse { ChargePeriods = chargePeriods, Result = result };
        }
    }
}