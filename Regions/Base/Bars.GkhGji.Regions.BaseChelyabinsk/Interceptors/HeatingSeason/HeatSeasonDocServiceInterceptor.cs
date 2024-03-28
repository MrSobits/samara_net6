
namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.HeatingSeason
{
    using System.Collections.Generic;

    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class HeatSeasonDocServiceInterceptor : Bars.GkhGji.Interceptors.HeatSeasonDocServiceInterceptor<HeatSeasonDoc>
    {
        /// <summary>
        /// Данные типы документов необходимы для того тчобы можно было сформирвоать документ пАспорт готовности
        /// </summary>
        public override List<HeatSeasonDocType> GetListTypesForCentralizedPasport()
        {
            return new List<HeatSeasonDocType>
                    {
                        HeatSeasonDocType.ActFlushingHeatingSystem,
                        HeatSeasonDocType.ActPressingHeatingSystem,
                        HeatSeasonDocType.ActReadyHeatingDevices
                    };
        }
    }

}