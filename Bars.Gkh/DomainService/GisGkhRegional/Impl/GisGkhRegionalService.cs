namespace Bars.Gkh.DomainService.GisGkhRegional.Impl
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис для получения региональных ранных при работе с ГИС ЖКХ
    /// </summary>
    public class GisGkhRegionalService : IGisGkhRegionalService
    {
        public virtual int GetCitizenSuggestionRubric(string code)
        {
            return -1;
        }

        public virtual bool UserIsCr()
        {

            return false;
        }

        public virtual bool UserIsGji()
        {
            return false;
        }
    }
}