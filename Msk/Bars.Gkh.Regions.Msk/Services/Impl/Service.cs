namespace Bars.Gkh.Regions.Msk.Services.Impl
{
    using System.ServiceModel.Activation;
    using Castle.Windsor;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        public IWindsorContainer Container { get; set; }
    }
}