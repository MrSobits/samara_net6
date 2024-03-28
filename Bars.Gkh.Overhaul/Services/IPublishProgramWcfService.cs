namespace Bars.Gkh.Overhaul.DomainService
{
    using Bars.Gkh.Overhaul.Services.DataContracts;

    public interface IPublishProgramWcfService
    {
        PublishProgRecWcfProxy[] GetPublishProgramRecs(long muId);
    }
}