namespace Bars.GkhGji.StateChange
{
    using B4.DataAccess;
    using B4.Modules.States;
    using Castle.Windsor;
    using Gkh.Entities;

    public class ManOrgLicenseRemoveDateStateRule : IRuleChangeStatus
    {
        public virtual IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "ManOrgLicenseRemoveDateStateRule"; }
        }

        public string Name { get { return "Удаление значение поля \"Дата получения лицензии\""; } }
        public string TypeId { get { return "gkh_manorg_license"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет удалено значение поля \"Дата получения лицензии\" в карточке Контрагента";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var license = statefulEntity as ManOrgLicense;

            var contragentDomain = Container.ResolveDomain<Contragent>();

            try
            {
                if (license == null)
                {
                    return ValidateResult.No("Невозможно определить лицензию!");
                }


                var contragent = license.Contragent;

                contragent.LicenseDateReceipt = null;

                contragentDomain.Update(contragent);
            }
            finally 
            {
                Container.Release(contragentDomain);
            }
                                      
            return ValidateResult.Yes();
        }

        
    }
}
