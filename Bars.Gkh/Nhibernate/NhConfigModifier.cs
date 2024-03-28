namespace Bars.Gkh.Nhibernate
{
    using Bars.B4.DataAccess;

    using NHibernate.Cfg;

    /// <summary>
    /// Конфигуратор NHibernate
    /// </summary>
    public class NhConfigModifier : INhibernateConfigModifier
    {
        /// <inheritdoc />
        public void Apply(Configuration configuration)
        {
            configuration.LinqToHqlGeneratorsRegistry<NhRegistry>();
        }
    }
}