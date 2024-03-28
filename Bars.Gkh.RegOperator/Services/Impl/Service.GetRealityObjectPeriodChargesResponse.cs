namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using DataContracts;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using Gkh.Services.DataContracts;
    using Gkh.Utils;

    public partial class Service
    {
        public GetRealityObjectPeriodChargesResponse GetRealityObjectPeriodCharges(string houseId, string periodId)
        {
            var roId = houseId.ToLong();
            var chargePeriodId = periodId.ToLong();

            var periodDomain = Container.ResolveDomain<ChargePeriod>();

            var period = periodDomain.Get(chargePeriodId);

            if (period == null || roId < 1)
            {
                return new GetRealityObjectPeriodChargesResponse { Result = Result.DataNotFound };
            }

            return GetCharges(period, roId);
        }

        private GetRealityObjectPeriodChargesResponse GetCharges(ChargePeriod period, long roId)
        {
            var sums = new List<ChargedAndPaidSum>();

            var roomDomain = Container.ResolveDomain<Room>();
            var summaryDomain = Container.ResolveDomain<PersonalAccountPeriodSummary>();

            var rooms = roomDomain.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    x.RoomNum
                })
                .ToArray();

            var summaries = summaryDomain.GetAll()
                .Where(x => x.PersonalAccount.Room.RealityObject.Id == roId)
                .Where(x => x.Period.Id <= period.Id)
                .Select(x => new
                {
                    RoomId = x.PersonalAccount.Room.Id,
                    x.PersonalAccount.PersonalAccountNum,
                    Summary = x
                })
                .AsEnumerable()
                .GroupBy(x => x.RoomId)
                .ToDictionary(x => x.Key, z => z.ToArray());

            foreach (var room in rooms.OrderBy(x => x.RoomNum, new NumericComparer()))
            {
                var sum = new ChargedAndPaidSum
                {
                    FlatNum = room.RoomNum
                };

                var roomSummaries = summaries.Get(room.Id);

                if (!roomSummaries.IsEmpty())
                {
                    sum.ChargedSum = roomSummaries.Sum(x => x.Summary.GetTotalCharge() + x.Summary.GetTotalChange() - x.Summary.GetTotalPerformedWorkCharge()).RegopRoundDecimal(2);
                    sum.PaidSum = roomSummaries.Sum(x => x.Summary.GetTotalPayment()).RegopRoundDecimal(2);

                    sum.PersonalAccountNum = roomSummaries
                        .Select(x => x.PersonalAccountNum)
                        .Distinct()
                        .AggregateWithSeparator(", ");
                }

                sums.Add(sum);
            }

            var currPeriodSummaries = summaries.Values.SelectMany(x => x)
                .Select(x => x.Summary);

            var totalCharge = currPeriodSummaries
                .SafeSum(x => x.GetTotalCharge()).RegopRoundDecimal(2);

            var totalPaid = currPeriodSummaries
                .SafeSum(x => x.GetTotalPayment()).RegopRoundDecimal(2);
            
            return new GetRealityObjectPeriodChargesResponse
            {
                ChargedAndPaidSums = sums,
                ChargeSum = new ChargeProxy
                {
                    Sum = summaries.Values.SelectMany(x => x)
                        .Where(x => x.Summary.Period.Id == period.Id)
                        .SafeSum(x => x.Summary.GetTotalCharge()).ToStr()
                },
                TotalChargedSum = totalCharge,
                TotalPaidSum = totalPaid,
                Result = Result.NoErrors
            };
        }
    }
}