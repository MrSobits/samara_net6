using Bars.B4;

namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActCheck
{
    /// <summary>
    /// Сервис для работы с нарушениями действия акта проверки
    /// </summary>
    public interface IActCheckActionViolationService
    {
        /// <summary>
        /// Добавить нарушения
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult AddViolations(BaseParams baseParams);
    }
}
