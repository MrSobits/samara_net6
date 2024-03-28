namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IJurPersonService
    {
        /// <summary>
        ///     Возвращает типы контрагентов
        /// </summary>
        /// <param name="baseParams">
        ///     Параметры.
        /// </param>
        /// <returns>Типы контрагентов</returns>
        IDataResult GetJurPersonTypes(BaseParams baseParams);
    }
}