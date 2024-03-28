namespace Bars.Gkh.Regions.Perm.StateChanges
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    public class RevocationLicenseStateRule : IRuleChangeStatus
    {

        public virtual IWindsorContainer Container { get; set; }

        public string Id => "RevocationLicenseStateRule";

        public string Name => "Аннулирование лицензии";

        public string TypeId => "gkh_manorg_license";

        public string Description => "При переводе статуса будет аннулирована Лицензия";

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

            var personPlaceWorkDomain = this.Container.ResolveDomain<PersonPlaceWork>();
            var personDisqualInfoDomain = this.Container.Resolve<IRepository<PersonDisqualificationInfo>>();
            var personQualCertDomain = this.Container.Resolve<IRepository<PersonQualificationCertificate>>();
            var manOrgRoService = this.Container.Resolve<IManagingOrgRealityObjectService>();
            var manOrgContractOwnersRepository = this.Container.ResolveRepository<ManOrgContractOwners>();

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

                var manOrgContractsRo = manOrgRoService.GetAllActive()
                    .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == license.Contragent.Id)
                    .Select(x => x.ManOrgContract.Id)
                    .ToList();

                var manOrgContractOwners = manOrgContractOwnersRepository.GetAll()
                    .Where(x => manOrgContractsRo.Contains(x.Id))
                    .Where(x => x.DateLicenceDelete == null)
                    .ToList();

                foreach (var manOrgContractOwner in manOrgContractOwners)
                {
                    manOrgContractOwner.DateLicenceDelete = license.DateTermination;
                    manOrgContractOwner.ContractStopReason = ContractStopReasonEnum.revocation_of_license;

                    manOrgContractOwnersRepository.Update(manOrgContractOwner);
                }

                return ValidateResult.Yes();
            }
            finally 
            {
                this.Container.Release(personPlaceWorkDomain);
                this.Container.Release(personDisqualInfoDomain);
                this.Container.Release(personQualCertDomain);
                this.Container.Release(manOrgRoService);
                this.Container.Release(manOrgContractOwnersRepository);
            }
        }
        
    }
}
