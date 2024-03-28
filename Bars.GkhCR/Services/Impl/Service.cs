namespace Bars.GkhCr.Services.Impl
{
    using Bars.GkhCr.Services.ServiceContracts;

    using Castle.Windsor;

    // TODO wcf
    // [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        public IWindsorContainer Container { get; set; }
    }
}