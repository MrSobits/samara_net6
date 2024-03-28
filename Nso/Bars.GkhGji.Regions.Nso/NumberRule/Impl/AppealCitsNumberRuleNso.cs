namespace Bars.GkhGji.Regions.Nso.NumberRule
{
    using B4.DataAccess;
    using Contracts;
    using Castle.Windsor;

    public class AppealCitsNumberRuleNso : IAppealCitsNumberRule
    {
        public IWindsorContainer Container { get; set; }

        public void SetNumber(IEntity entity)
        {
            var numberRule = Container.Resolve<INumberRuleNso>();
            if (numberRule != null)
            {
                numberRule.SetNumber(entity);
            }
        }
    }
}
