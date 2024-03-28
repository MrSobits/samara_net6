namespace Bars.Gkh.Overhaul.Services.Impl
{
    using Castle.Windsor;

    using IService = Bars.Gkh.Overhaul.Services.ServiceContracts.IService;

    // TODO wcf
    // [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        public IWindsorContainer Container { get; set; }
    }
}