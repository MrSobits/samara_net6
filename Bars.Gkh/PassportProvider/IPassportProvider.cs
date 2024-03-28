namespace Bars.Gkh.PassportProvider
{
    using System.Collections.Generic;

    using Bars.B4;
    using Enums.BasePassport;

    /// <summary>
    /// интерфейс для работы с паспортами
    /// </summary>
    public interface IPassportProvider
    {
        /// <summary>
        /// Наименование паспорта
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Тип источника
        /// </summary>
        string TypeDataSource { get; }

        IList<MenuItem> GetMenu();

        object GetFormWithData(string formId, IDictionary<string, Dictionary<string, string>> data);

        bool UpdateForm(long realityObjectId, string formId, List<SerializePassportValue> requestVal, bool fromSync = false);

        /// <summary>
        /// получить описание элементов редактирования
        /// </summary>
        /// <returns></returns>
        object GetEditors();

        /// <summary>
        /// получить нужные описания элементов редактирования по идентификатору формы
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        object GetEditors(string formId);

        /// <summary>
        /// получить editor по formId, componentId, code
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="componentId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        TypeEditor GetEditorByFormAndComponentAndCode(string formId, string componentId, string code);

        /// <summary>
        /// Возвращает true, если данная ячейка редактируемая
        /// </summary>
        /// <param name="componentCode">код компонента</param>
        /// <param name="cellCode">код ячейки</param>
        /// <returns></returns>
        bool IsEditableCell(string componentCode, string cellCode);

        /// <summary>
        /// Возвращает true, если данный компонент динамический грид (грид с возможностью добавлять строки)
        /// </summary>
        /// <param name="componentCode">код компонента</param>
        /// <returns></returns>
        bool IsInlineGrid(string componentCode);

        /// <summary>
        /// Возвращает ячейки в описании техпаспорта
        /// </summary>
        /// <param name="componentCode">идентификатор компонента</param>
        /// <returns>key=код, value=значение ячейки</returns>
        Dictionary<string, string> GetCells(string componentCode);

        /// <summary>
        /// Получаем коды компонентов на форме
        /// </summary>
        /// <param name="formId">код формы</param>
        /// <returns></returns>
        List<string> GetComponentCodes(string formId);

        /// <summary>
        /// Возвращает текстовое значение ячейки
        /// </summary>
        /// <param name="componentCode">код компонента</param>
        /// <param name="cellCode">код ячейки</param>
        /// <param name="value">значение ячейки</param>
        /// <returns></returns>
        string GetTextForCellValue(string componentCode, string cellCode, string value);

        /// <summary>
        /// Возвращает значение ячейки в зависимости от <see cref="TypeEditor"/>
        /// </summary>
        /// <param name="componentCode">код компонента</param>
        /// <param name="cellCode">код ячейки</param>
        /// <param name="value">значение ячейки</param>
        /// <returns></returns>
        string GetCellValue(string componentCode, string cellCode, string value);

        /// <summary>
        /// Возвращает название элемента
        /// </summary>
        /// <param name="componentCode">код компонента</param>
        /// <param name="elementCode">код элемента</param>
        /// <returns></returns>
        string GetLabelForFormElement(string componentCode, string elementCode);

        /// <summary>
        /// Получить значения техпаспорта дома
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома</param>
        /// <returns>Значения техпаспорта</returns>
        IList<SerializePassportValue> GetPassportValuesByRo(long realityObjectId);

        /// <summary>
        /// Получить компонент по ИД формы и ИД компонента
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="componentId"></param>
        /// <returns>ComponentTechPassport либо NULL</returns>
        object GetComponentBy(string formId, string componentId);

        /// <summary>
        /// Массовое обновление техпаспорта
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="fromSync"></param>
        /// <returns></returns>
        bool UpdateForms(Dictionary<long, Dictionary<string, List<SerializePassportValue>>> dict, bool fromSync = false);
    }
}
