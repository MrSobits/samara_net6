namespace Bars.Gkh.Regions.Chelyabinsk.DomainService.GisGkhRegional.Impl
{
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;

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
        public override int GetCitizenSuggestionRubric(string code)
        {
            switch (code.Split('.')[0])
            {
                case "3":
                    return 1;
                default:
                    return 2;
            }
        }

        public override bool UserIsCr()
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

        public override bool UserIsGji()
        {
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator.GisGkhContragent == null)
            {
                return false;
            }
            if (thisOperator.GisGkhContragent.Name.Contains("ЖИЛИЩНАЯ ИНСПЕКЦИЯ"))
            {
                return true;
            }
            return false;
        }
    }
}