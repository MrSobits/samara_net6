namespace Bars.Gkh.RegOperator.LogMap
{
    using B4;
    using B4.Application;
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Decisions.Nso.Entities.Decisions;
    using Gkh.Entities;

    public class GovDecisionLogMap : AuditLogMap<GovDecision>
    {
        public GovDecisionLogMap()
        {
            Name("Протоколы решений ОГВ");

            MapProperty(x => x.ProtocolNumber, "ProtocolNumber", "Номер");
            MapProperty(x => x.ProtocolDate, "ProtocolDate", "Дата протокола");
            MapProperty(x => x.DateStart, "DateStart", "Дата вступления в силу");
            MapProperty(x => x.RealtyManagement, "RealtyManagement", "Управление домом");
            MapProperty(x => x.AuthorizedPerson, "AuthorizedPerson", "Уполномоченное лицо");
            MapProperty(x => x.AuthorizedPersonPhone, "AuthorizedPersonPhone", "Телефон уполномоченного лица");
            MapProperty(x => x.ProtocolFile, "ProtocolFile", "Файл протокола", x => x.Return(y => y.FullName));
            MapProperty(x => x.FundFormationByRegop, "FundFormationByRegop", "На счету регопа");
            MapProperty(x => x.Destroy, "Destroy", "Снос МКД");
            MapProperty(x => x.DestroyDate, "DestroyDate", "Дата сноса МКД");
            MapProperty(x => x.Reconstruction, "Reconstruction", "Реконструкция МКД");
            MapProperty(x => x.ReconstructionStart, "ReconstructionStart", "Дата начала реконструкции");
            MapProperty(x => x.ReconstructionEnd, "ReconstructionEnd", "Дата окончания реконструкции");
            MapProperty(x => x.TakeLandForGov, "TakeLandForGov", "Изъятие для государственных или муниципальных нужд");
            MapProperty(x => x.TakeLandForGovDate, "TakeLandForGovDate", "Дата изъятия земельного участка");
            MapProperty(x => x.TakeApartsForGov, "TakeApartsForGov", "Изъятие каждого жилого помещения в доме");
            MapProperty(x => x.TakeApartsForGovDate, "TakeApartsForGovDate", "Дата изъятия жилых помещений");
            MapProperty(x => x.MaxFund, "MaxFund", "Максимальный размер фонда");
            MapProperty(x => x.State, "State", "Статус", x => x.Return(y => y.Description.Or(y.Name)));

            // Адрес
            Description(x =>
            {
                var domain = ApplicationContext.Current.Container.Resolve<IDomainService<RealityObject>>();
                try
                {
                    var rObject = domain.Load(x.RealityObject.Id);
                    return rObject.Address;
                }
                catch
                {
                    return string.Empty;
                }
                finally
                {
                    ApplicationContext.Current.Container.Release(domain);
                }
            });
        }
    }
}