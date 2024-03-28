namespace Bars.Gkh.Gis.DomainService.Helper.HqlHelper
{
    using B4;

    public interface IHqlHelper
    {
        /// <summary>
        /// Получить HQL запрос сортировки данных по входным параметрам
        /// </summary>
        /// <param name="loadParam">Заполняется свойство Order</param>
        /// <returns></returns>
        string GetOrderHqlQuery(LoadParam loadParam);

        /// <summary>
        /// Получить HQL запрос фильтрации данных по ComplexFilter'у
        /// </summary>
        /// <param name="loadParam">Заполняется свойство ComplexFilter</param>
        /// <returns></returns>
        string GetComplexFilterHqlQuery(LoadParam loadParam);
    }
}
