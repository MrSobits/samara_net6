namespace Bars.Gkh.RegOperator.Services.Impl
{
    using Bars.Gkh.RegOperator.Services.ServiceContracts;

    using Castle.Windsor;

    // TODO: WCF
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        public IWindsorContainer Container { get; set; }
    }
}