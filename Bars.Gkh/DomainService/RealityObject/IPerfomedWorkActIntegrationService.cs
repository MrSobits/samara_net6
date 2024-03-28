namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Сервис для получение данных из Акта выполненных работ
    /// </summary>
    public interface IPerfomedWorkActIntegrationService
    {
        /// <summary>
        /// Получение данных из Акта выполненных работ
        /// </summary>
        /// <param name="ids">Id жилых домов</param>
        /// <returns></returns>
        Dictionary<long, IEnumerable<PerfomedWorkActProxy>> GetPerfomedWorkActProxies(long[] ids);
    }

    /// <summary>
    /// Промежуточных класс 
    /// </summary>
    public class PerfomedWorkActProxy
    {
        /// <summary>
        /// Наименование работы последнего капитального ремонта
        /// </summary>
        public string TypesWorkOverhaul { get; set; }

        /// <summary>
        /// Дата фактического выполнения работы
        /// </summary>
        public DateTime? DatePerformance { get; set; }
    }
}