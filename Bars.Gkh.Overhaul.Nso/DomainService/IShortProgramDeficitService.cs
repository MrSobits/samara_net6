namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using Bars.B4;

    public interface IShortProgramDeficitService
    {
        /// <summary>
        /// Сохранение дефицитов
        /// </summary>
        IDataResult SaveDeficit(BaseParams baseParams);
        
        /// <summary>
        /// Сформирвоать долгосрочную программу
        /// </summary>
        IDataResult CreateShortProgram(BaseParams baseParams);
    }
}