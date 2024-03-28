namespace Bars.Gkh.Regions.Perm.StateChanges
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    public class TerminationLicenseStateRule : IRuleChangeStatus
    {
        public virtual IWindsorContainer Container { get; set; }

        public string Id => "TerminationLicenseStateRule";

        public string Name => "Прекращение деятельности";

        public string TypeId => "gkh_manorg_license";

        public string Description => "Прекращение деятельности";

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

            var manOrgRoService = this.Container.Resolve<IManagingOrgRealityObjectService>();
            var manOrgContractOwnersDomain = this.Container.ResolveDomain<ManOrgContractOwners>();

            try
            {
                var manOrgContractsRo = manOrgRoService.GetAllActive()
                    .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == license.Contragent.Id)
                    .Select(x => x.ManOrgContract.Id)
                    .ToList();

                var manOrgContractOwners = manOrgContractOwnersDomain.GetAll()
                    .Where(x => manOrgContractsRo.Contains(x.Id))
                    .Where(x => x.DateLicenceDelete == null)
                    .ToList();

                foreach (var manOrgContractOwner in manOrgContractOwners)
                {
                    manOrgContractOwner.DateLicenceDelete = license.DateTermination;
                    manOrgContractOwner.ContractStopReason = ContractStopReasonEnum.is_excluded_decision;

                    manOrgContractOwnersDomain.Update(manOrgContractOwner);
                }

                return ValidateResult.Yes();
            }
            finally
            {
                this.Container.Release(manOrgRoService);
                this.Container.Release(manOrgContractOwnersDomain);
            }
        }
    }
}
