using Bars.GkhCr.Entities;
using Bars.GkhCr.Services;
using System.Collections.Generic;

namespace Bars.GkhCr.DomainService
{
    /// <summary>
    /// Сервис получение работ ДПКР (РТ)
    /// </summary>
    public interface IGetProgramVersionService
    {
        /// <summary>
        /// Получение работ ДПКР (РТ)
        /// </summary>
        /// <param name="municipalityId">МО</param>
        /// <param name="housesId">Жилой дом</param>
        /// <returns>Маcсив работ ДПКР (РТ)</returns>
        DPKR[] GetProgramVersion(long municipalityId, long housesId);
    }
}