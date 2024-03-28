namespace Bars.Gkh.Overhaul.Services.Impl
{
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts;

    public partial class Service
    {
        public IPublishProgramWcfService PublishProgramWcfService { get; set; }

        public GetPublishProgramRecsResponse GetPublishProgramRecs(string muId)
        {
            var publishProgRecs = PublishProgramWcfService.GetPublishProgramRecs(muId.ToLong());

           return  new GetPublishProgramRecsResponse
                       {
                           PublishProgRecs = publishProgRecs,
                           Result = publishProgRecs.Any() ? Result.NoErrors : Result.DataNotFound
                       };
        }
    }
}