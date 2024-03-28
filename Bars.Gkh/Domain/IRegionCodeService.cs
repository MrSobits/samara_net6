namespace Bars.Gkh.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Сервис для работы с кодами субъекта РФ
    /// </summary>
    public interface IRegionCodeService
    {
        /// <summary>
        /// Код справочника
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Название справочника
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Получить наименование субъекта РФ по коду
        /// </summary>
        /// <param name="code">Код субъекта</param>
        string GetRegionName(string code);

        /// <summary>
        /// Получить код текущего региона
        /// </summary>
        string GetRegionCode();

        /// <summary>
        /// Получить информацию обо всех регионах
        /// </summary>
        IDictionary<string, string> GetAll();
    }
}