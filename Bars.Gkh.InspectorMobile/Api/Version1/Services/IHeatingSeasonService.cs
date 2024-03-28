namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using System.Collections.Generic;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.HeatingSeason;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Интерфейс сервиса для работы с <see cref="HeatSeason"/>
    /// </summary>
    public interface IHeatingSeasonService : IBaseApiService<object, HeatingSeasonObjectUpdate>
    {
        /// <summary>
        /// Получить объект по указанному периоду и дому
        /// </summary>
        /// <param name="heatingSeasonPeriodId">Уникальный идентификатор периода по отопительному сезону</param>
        /// <param name="addressId">Уникальный идентификатор дома</param>
        HeatingSeasonObjectGet Get(long heatingSeasonPeriodId, long addressId);

        /// <summary>
        /// Получить список объектов по указанному периоду и домам
        /// </summary>
        /// <param name="heatingSeasonPeriodId">Уникальный идентификатор периода по отопительному сезону</param>
        /// <param name="addressIds">Перечень уникальных идентификаторов домов</param>
        IEnumerable<HeatingSeasonObjectGet> GetList(long heatingSeasonPeriodId, long[] addressIds);
    }
}