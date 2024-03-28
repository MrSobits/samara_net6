namespace Bars.Gkh1468.Wcf
{
   
    using Castle.Windsor;

    // TODO : WCF
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class PassportService : IPassportService
    {
        public IWindsorContainer Container { get; set; }
    }
}