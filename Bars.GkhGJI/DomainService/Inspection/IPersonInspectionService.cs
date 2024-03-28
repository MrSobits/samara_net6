namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IPersonInspectionService
    {
        /// <summary>
        ///     Возвращает типы объектов проверки
        /// </summary>
        /// <param name="baseParams">
        ///     Параметры.
        /// </param>
        /// <returns>Типы объектов проверки</returns>
        IDataResult GetPersonInspectionTypes(BaseParams baseParams);
    }
}