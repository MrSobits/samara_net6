namespace Bars.Gkh.GeneralState
{
    using System.Collections.Generic;

    /// <summary>
    /// Манифест для объявления обобщенного состояния
    /// </summary>
    public interface IGeneralStatefulEntityManifest
    {
        /// <summary>
        /// Состояния объявляются в этом методе
        /// </summary>
        /// <returns>Перечисление описателей</returns>
        IEnumerable<GeneralStatefulEntityInfo> GetAllInfo();
    }
}