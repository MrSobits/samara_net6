namespace Bars.GkhGji.Regions.Smolensk.NumberRule
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Contracts;
    using Castle.Windsor;

    public class AppealCitsNumberRuleEmpty : IAppealCitsNumberRule
    {
        public IWindsorContainer Container { get; set; }

        public void SetNumber(IEntity entity)
        {
            #warning заглушка для Смоленска, т.к. у них другой механизм проставления номера
        }
    }
}