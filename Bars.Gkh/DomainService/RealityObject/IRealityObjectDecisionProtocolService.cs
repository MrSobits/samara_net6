namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
	/// Сервис для работы с "Протоколы и решения" (proxy к Bars.Gkh.Decisions.Nso)
	/// </summary>
	public interface IRealityObjectDecisionProtocolProxyService : ITypeOfFormingCrProvider
    {
        /// <summary>
        /// Получить прокси протокола или решения
        /// </summary>
        /// <param name="realityObject">Жилой дом</param>
        /// <returns>Прокси протокола или решения</returns>
        RealityObjectDecisionProtocolProxy GetBothProtocolProxy(RealityObject realityObject);

        /// <summary>
        /// Получить прокси протокола или решения
        /// </summary>
        /// <param name="roId">Идентификатор жилого дома</param>
        /// <returns>Прокси протокола или решения</returns>
        RealityObjectDecisionProtocolProxy GetBothProtocolProxy(long roId);

        /// <summary>
        /// Получить актуальные протоколы по домам
        /// </summary>
        Dictionary<long, RealityObjectDecisionProtocolProxy> GetBothProtocolProxy(IQueryable<RealityObject> realityObjects);

        /// <summary>
        /// Получить все протоколы по домам
        /// </summary>
        Dictionary<long, List<RealityObjectDecisionProtocolProxy>> GetAllBothProtocolProxy(IQueryable<RealityObject> realityObjects);

        /// <summary>
        /// Получить все протоколы по домам
        /// </summary>
        Dictionary<long, List<RealityObjectDecisionProtocolProxy>> GetAllBothProtocolByFormat(IQueryable<RealityObject> realityObjects);

        /// <summary>
        /// Получить размер взноса на КР (либо из протокола, либо из справочника взносов)
        /// </summary>
        /// <param name="protocolId">Идентификатор протокола</param>
        /// <param name="roId">Идентификатор жилого дома</param>
        /// <param name="yearStart">Дата начала периода</param>
        /// <returns>Размер взноса на КР</returns>
        decimal? GetPaysize(long protocolId, long roId, DateTime? yearStart);

        /// <summary>
        /// Проверить на наличие протокола и заполненность обязательных полей
        /// </summary>
        /// <param name="ro"></param>
        IDataResult CheckDecisionProtocol(RealityObject ro);
    }

    /// <summary>
    /// Прокси класс для "Протоколы и решения"
    /// </summary>
    public class RealityObjectDecisionProtocolProxy
    {
        /// <summary>
        /// Идентификатор протокола
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор протокола
        /// </summary>
        public long ExportId { get; set; }

        /// <summary>
        /// Идентификатор жилого дома
        /// </summary>
        public long RealityObjectId { get; set; }

        /// <summary>
        /// Дата протокола
        /// </summary>
        public DateTime ProtocolDate { get; set; }

        /// <summary>
        /// Номер протокола
        /// </summary>
        public string ProtocolNumber { get; set; }

        /// <summary>
        /// Наименование владельца специального счета
        /// </summary>
        public string RegOpContragentName { get; set; }

        /// <summary>
        /// ИНН владельца специального счета
        /// </summary>
        public string RegOpContragentInn { get; set; }

        /// <summary>
        /// Размер взноса на капитальный ремонт
        /// </summary>
        public decimal? CrPaySize { get; set; }

        /// <summary>
        /// Тип протокола решения
        /// </summary>
        public CoreDecisionType? DecisionType { get; set; }
    }
}