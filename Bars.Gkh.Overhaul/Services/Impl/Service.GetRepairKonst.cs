namespace Bars.Gkh.Overhaul.Services.Impl
{
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts;

    public partial class Service
    {
        public IRepairKonstWcfService RepairKonstWcfService { get; set; }

        public GetRepairKonstResponse GetRepairKonst(string roId)
        {
            var repairKonst = RepairKonstWcfService.GetRepairKonstWcfService(roId.ToLong());

            return new GetRepairKonstResponse
                       {
                           RepairKonstProxy = repairKonst,
                           Result = repairKonst.Any() ? Result.NoErrors : Result.DataNotFound
                       };
        }
    }
}