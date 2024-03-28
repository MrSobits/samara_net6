namespace Bars.GkhGji.DomainService
{
    using System.Collections;

    using B4;

    using Bars.GkhGji.Entities;

    /// <summary>
    /// Интерфейс сервиса взаимодействия с <see cref="StatSubjectGji"/>
    /// </summary>
    public interface IStatSubjectGjiService
    {
        /// <summary>
        /// Добавить тематику
        /// </summary>
        IDataResult AddSubsubject(BaseParams baseParams);
        
        /// <summary>
        /// Получить список тематик для СОПР
        /// </summary>
        /// <returns></returns>
        IList ListForSopr();
    }
}