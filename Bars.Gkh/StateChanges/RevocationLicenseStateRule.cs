using Bars.Gkh.Entities;
using Castle.Windsor;

namespace Bars.Gkh.StateChanges
{
    using System.Linq;
    using System.Runtime.ConstrainedExecution;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;

    public class RevocationLicenseStateRule : IRuleChangeStatus
    {

        public virtual IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "RevocationLicenseStateRule"; }
        }

        public string Name { get { return "Аннулирование лицензии"; } }
        public string TypeId { get { return "gkh_manorg_license"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет аннулирована Лицензия";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var license = statefulEntity as ManOrgLicense;

            if (!newState.FinalState)
            {
                return ValidateResult.No("Данное правило должно работать только на кончном статусе");
            }

            if (license == null)
            {
                return ValidateResult.No("Внутренняя ошибка.");
            }

            if (license.TypeTermination == TypeManOrgTerminationLicense.NotSet || !license.DateTermination.HasValue)
            {
                return ValidateResult.No("Необходимо указать основание и дату аннулирования лицензии");
            }

            var personPlaceWorkDomain = Container.ResolveDomain<PersonPlaceWork>();
            var personDisqualInfoDomain = Container.Resolve<IRepository<PersonDisqualificationInfo>>();
            var personQualCertDomain = Container.Resolve<IRepository<PersonQualificationCertificate>>();

            try
            {
                var activePersonsQuery = personPlaceWorkDomain.GetAll()
                    .Where(x => x.Contragent.Id == license.Contragent.Id)
                    .Where(x => x.StartDate <= license.DateTermination &&
                                (!x.EndDate.HasValue || x.EndDate >= license.DateTermination));

                var personHasDisqualQuery = personDisqualInfoDomain.GetAll()
                    .Where(x => !x.EndDisqDate.HasValue || x.EndDisqDate >= license.DateTermination);

                var activePersons = activePersonsQuery
                    .Where(x => !personHasDisqualQuery.Any(y => y.Person.Id == x.Person.Id))
                    .Select(x => x.Person)
                    .AsEnumerable()
                    .Distinct()
                    .ToList();
               
                var qualCerts = personQualCertDomain.GetAll()
                    .Where(x => activePersonsQuery.Any(y => y.Person.Id == x.Person.Id))
                    .Where(x => x.IssuedDate < license.DateTermination && x.TypeCancelation == TypeCancelationQualCertificate.NotSet)
                    .ToList();

                foreach (var person in activePersons)
                {
                    personDisqualInfoDomain.Save(new PersonDisqualificationInfo
                    {
                        Person = person,
                        TypeDisqualification = TypePersonDisqualification.CancelationLicenze,
                        DisqDate = license.DateTermination
                    });
                }

                foreach (var cert in qualCerts)
                {
                    cert.TypeCancelation = TypeCancelationQualCertificate.Disqualification;
                    cert.CancelProtocolDate = license.DateTermination;
                    cert.EndDate = cert.CancelProtocolDate;
                    cert.HasCancelled = true;

                    personQualCertDomain.Update(cert);
                }

                return ValidateResult.Yes();
            }
            finally 
            {
                Container.Release(personPlaceWorkDomain);
                Container.Release(personDisqualInfoDomain);
                Container.Release(personQualCertDomain);
            }
        }
        
    }
}
