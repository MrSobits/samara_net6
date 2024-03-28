namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис категория документаций 
    /// </summary>
    public interface IInstructionGroupService
    {
        /// <summary>
        /// Список категорий по ролю
        /// </summary>
        /// <returns></returns>
        IDataResult ListByRole(BaseParams baseParams);
    }
}