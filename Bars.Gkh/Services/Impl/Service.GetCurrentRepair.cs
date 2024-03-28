namespace Bars.Gkh.Services.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.CurrentRepair;

    public partial class Service
    {
        public GetCurrentRepairResponse GetCurrentRepair(string houseId)
        {
            int id;
            var currentRepairs = new CurrentRepair[] { };

            if (int.TryParse(houseId, out id))
            {
                currentRepairs =
                    Container.Resolve<IDomainService<RealityObjectCurentRepair>>()
                             .GetAll()
                             .Where(x => x.RealityObject.Id == id)
                             .Select(
                                 x =>
                                 new CurrentRepair
                                     {
                                         Id = x.Id,
                                         Name = x.WorkKind.Name,
                                         FactDate =
                                             x.FactDate.HasValue
                                                 ? x.FactDate.Value.ToShortDateString()
                                                 : null,
                                         FactSize = x.FactWork.GetValueOrDefault(),
                                         FactSum = x.FactSum.GetValueOrDefault(),
                                         Measure = x.UnitMeasure,
                                         PlanDate =
                                             x.PlanDate.HasValue
                                                 ? x.PlanDate.Value.ToShortDateString()
                                                 : null,
                                         PlanSize = x.PlanWork.GetValueOrDefault(),
                                         PlanSum = x.PlanSum.GetValueOrDefault()
                                     })
                             .ToArray();
            }

            var result = currentRepairs.Length == 0 ? Result.DataNotFound : Result.NoErrors;

            return new GetCurrentRepairResponse { CurrentRepairs = currentRepairs, Result = result };
        }
    }
}