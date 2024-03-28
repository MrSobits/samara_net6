namespace Bars.GkhGji.Regions.BaseChelyabinsk.NumberRule.Impl
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Contracts;

    using Castle.Windsor;

    public class AppealCitsNumberRuleChelyabinsk : IAppealCitsNumberRule
    {
        public IWindsorContainer Container { get; set; }

        public void SetNumber(IEntity entity)
        {
            var numberRule = this.Container.Resolve<INumberRuleChelyabinsk>();
            if (numberRule != null)
            {
                numberRule.SetNumber(entity);
            }
        }
    }
}
