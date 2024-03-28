namespace Bars.Gkh.Ris.Extractors.HouseManagement.MeteringDeviceData
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;

    /// <summary>
    /// Экстрактор данных по помещениям ПУ
    /// </summary>
    public class MeteringDeviceLivingRoomExtractor : BaseSlimDataExtractor<RisMeteringDeviceLivingRoom>
    {
        /// <summary>
        /// Получить сущности внутренней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности внутренней системы</returns>
        public override List<RisMeteringDeviceLivingRoom> Extract(DynamicDictionary parameters)
        {
            var selectedDevices = parameters.GetAs<List<RisMeteringDeviceData>>("selectedDevices");
            var risMeteringDeviceDataDomain = this.Container.ResolveDomain<RisMeteringDeviceLivingRoom>();

            try
            {
                var selectedDeviceIds = selectedDevices?.Select(x => x.ExternalSystemEntityId).ToArray() ?? new long[] { };

                return risMeteringDeviceDataDomain.GetAll()
                    .Where(x => x.MeteringDeviceData != null && selectedDeviceIds.Contains(x.MeteringDeviceData.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(risMeteringDeviceDataDomain);
            }
        }
    }
}
