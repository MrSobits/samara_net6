namespace Bars.Gkh.StateChanges
{
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.States;
    using Castle.Windsor;
    using Entities;

    public class LicenseRequestStateRule : IRuleChangeStatus
    {
        public virtual IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "LicenseRequestStateRule"; }
        }

        public string Name { get { return "Создание лицензии"; } }
        public string TypeId { get { return "gkh_manorg_license_request"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет создана Лицензия по Обращению";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var request = statefulEntity as ManOrgLicenseRequest;

            var licenseDomain = Container.ResolveDomain<ManOrgLicense>();
            var requestPersonDomain = Container.ResolveDomain<ManOrgRequestPerson>();

            try
            {
                if (request == null)
                {
                    return ValidateResult.No("Внутренняя ошибка.");
                }

                if (!newState.FinalState)
                {
                    return ValidateResult.No("Данное правило должно работать только на конечном статусе");
                }

                if (licenseDomain.GetAll().Any(x => x.Request.Id == request.Id))
                {
                   // return ValidateResult.No("По данному обращению уже создана лицензия");
                }

                if (!requestPersonDomain.GetAll().Any(x => x.LicRequest.Id == request.Id))
                {
                    return ValidateResult.No("По данному обращению отсутствуют должностные лица");
                }

                if (request.Contragent == null)
                {
                    return ValidateResult.No("Управляющая организация не указана.");
                }

                if (licenseDomain.GetAll().Any(x => x.Contragent.Id == request.Contragent.Id && !x.DateTermination.HasValue))
                {
                 //   return ValidateResult.No("У данной организации уже есть лицензия.");
                }

                var license = new ManOrgLicense
                {
                    Contragent = new Contragent {Id = request.Contragent.Id},
                    Request = new ManOrgLicenseRequest {Id = request.Id}
                };

                licenseDomain.Save(license);

            }
            finally 
            {
                Container.Release(licenseDomain);
                Container.Release(requestPersonDomain);
            }
                                      
            return ValidateResult.Yes();
        }

        
    }
}
