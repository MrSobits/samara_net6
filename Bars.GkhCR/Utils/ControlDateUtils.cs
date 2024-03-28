namespace Bars.GkhCr.Utils
{
    using System;
    using System.Collections.Generic;

    public static class ControlDateUtils
    {
        public static DateTime? GetControlDate(
            Dictionary<long, Dictionary<long, DateTime>> dictMunicipalityControlLimitDate,
            Dictionary<long, DateTime?> dictControlDate,
            long workId,
            long municipalityId)
        {
            if (dictMunicipalityControlLimitDate.ContainsKey(workId)
                && dictMunicipalityControlLimitDate[workId].ContainsKey(municipalityId))
            {
                return dictMunicipalityControlLimitDate[workId][municipalityId];
            }

            return dictControlDate.ContainsKey(workId)
                ? dictControlDate[workId]
                : null;
        }
    }
}
