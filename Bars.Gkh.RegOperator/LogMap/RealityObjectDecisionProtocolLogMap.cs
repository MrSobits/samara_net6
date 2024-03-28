namespace Bars.Gkh.RegOperator.LogMap
{
    using System.Linq;

    using B4;
    using B4.Application;
    using B4.Modules.NHibernateChangeLog;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;

    using Decisions.Nso.Entities;

    public class RealityObjectDecisionProtocolLogMap : AuditLogMap<RealityObjectDecisionProtocol>
    {
        public RealityObjectDecisionProtocolLogMap()
        {
            this.Name("Протоколы решений собственников");

            this.MapProperty(x => x.DocumentNum, "DocumentNum", "Номер");
            this.MapProperty(x => x.ProtocolDate, "ProtocolDate", "Дата протокола");
            this.MapProperty(x => x.DateStart, "DateStart", "Дата вступления в силу");
            this.MapProperty(x => x.AuthorizedPerson, "AuthorizedPerson", "Уполномоченное лицо");
            this.MapProperty(x => x.PhoneAuthorizedPerson, "PhoneAuthorizedPerson", "Телефон уполномоченного лица");
            this.MapProperty(x => x.File, "File", "Протокол", x => x.Return(y => y.FullName));
            this.MapProperty(x => x.State, "State", "Статус", x => x.Return(y => y.Description.Or(y.Name)));
            
            // Адрес
            this.Description(x =>
            {
                var container = ApplicationContext.Current.Container;
                var sessionProvider = container.Resolve<ISessionProvider>();

                var result = x.RealityObject.Address;
                if (result == null)
                {
                    using (container.Using(sessionProvider))
                    using (var statelessSession = sessionProvider.OpenStatelessSession())
                    {
                        try
                        {
                            const string query = "SELECT ADDRESS FROM GKH_REALITY_OBJECT where id = :id";

                            result = statelessSession
                                .CreateSQLQuery(query)
                                .SetParameter("id", x.RealityObject.Id)
                                .List<string>()
                                .FirstOrDefault();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }

                return result ?? string.Empty;
            });
        }
    }
}