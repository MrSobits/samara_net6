namespace Bars.Gkh.RegOperator.Domain
{
    using B4.DataAccess;
    using NHibernate.Cfg;

    public class NhConfigModifier : INhibernateConfigModifier
    {
        public void Apply(Configuration configuration)
        {
            configuration.Properties["adonet.batch_size"] = "500";
        }
    }
}