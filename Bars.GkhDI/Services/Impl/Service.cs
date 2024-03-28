namespace Bars.GkhDi.Services.Impl
{
    // using System.ServiceModel.Activation;
    using Castle.Windsor;

    // TODO: wcf
    // [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        public IWindsorContainer Container { get; set; }
    }
}
