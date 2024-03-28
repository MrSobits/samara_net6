namespace Bars.Gkh.RegOperator.ViewModels
{
    using B4;

    /// <summary>
    /// Вью-модель лицевых счетов для распределений
    /// </summary>
    public interface IPersonalAccountDistributionViewModel
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="baseParams"></param>
        ListDataResult List(BaseParams baseParams);
    }
}