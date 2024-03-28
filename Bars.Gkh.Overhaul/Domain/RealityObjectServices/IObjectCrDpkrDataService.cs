namespace Bars.Gkh.Overhaul.Domain.RealityObjectServices
{
    using System.Linq;
    using Gkh.Entities;
    using GkhCr.Entities;
    using ProxyEntity;

    /// <summary>
    /// Интерфейс для получения записей из программ кап. ремонта
    /// </summary>
    public interface IObjectCrDpkrDataService
    {
        /// <summary>
        /// Получение записей долгосрочной программы, которые попадают в краткосрочную
        /// </summary>
        /// <param name="programCr">Программа кап ремонта</param>
        /// <param name="municipality">Муниципальное образование</param>
        /// <returns>Список записей ДПКР, попадающие к краткосрочную</returns>
        IQueryable<ObjectCrDpkrProxy> GetShortProgramRecordsProgramAndMunicipality(ProgramCr programCr = null, Municipality municipality = null);
    }
}