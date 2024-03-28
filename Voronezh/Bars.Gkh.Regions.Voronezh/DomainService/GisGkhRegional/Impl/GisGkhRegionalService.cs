namespace Bars.Gkh.Regions.Voronezh.DomainService.GisGkhRegional.Impl
{
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using System.Collections.Generic;

    /// <summary>
    /// Сервис для получения региональных ранных при работе с ГИС ЖКХ
    /// </summary>
    public class GisGkhRegionalService : Bars.Gkh.DomainService.GisGkhRegional.Impl.GisGkhRegionalService
    {
        public IGkhUserManager UserManager { get; set; }
        public GisGkhRegionalService(IGkhUserManager _UserManager)
        {
            UserManager = _UserManager;
        }

        public override bool UserIsGji()
        {
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator.GisGkhContragent == null)
            {
                return false;
            }          
             return true;
          
        }

        public virtual bool UserIsCr()
        {
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator.GisGkhContragent == null)
            {
                return false;
            }
            if (thisOperator.GisGkhContragent.Name.Contains("капитального ремонта"))
            {
                return true;
            }
            return false;
        }


    }
}