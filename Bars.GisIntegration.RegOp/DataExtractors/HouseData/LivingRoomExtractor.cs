namespace Bars.GisIntegration.RegOp.DataExtractors.HouseData
{
    using System.Collections.Generic;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Base.Entities.HouseManagement;

    /// <summary>
    /// Экстрактор данных по комнатам
    /// </summary>
    public class LivingRoomExtractor : BaseSlimDataExtractor<LivingRoom>
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<LivingRoom> Extract(DynamicDictionary parameters)
        {
            return null;
        }
    }
}