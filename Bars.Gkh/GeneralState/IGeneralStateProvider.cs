namespace Bars.Gkh.GeneralState
{
    using System.Collections.Generic;

    /// <summary>
    /// Провайдер для хранения описателей обобщенных состояний
    /// </summary>
    public interface IGeneralStateProvider
    {
        /// <summary>
        /// Собрать все описатели состояний
        /// </summary>
        void Init();

        /// <summary>
        /// Получить все описатели состояний
        /// </summary>
        IEnumerable<KeyValuePair<string, GeneralStatefulEntityInfo>> GetStatefulInfos();
    }
}