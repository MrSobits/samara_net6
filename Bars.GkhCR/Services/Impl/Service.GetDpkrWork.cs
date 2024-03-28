namespace Bars.GkhCr.Services.Impl
{
    using B4;

    using System;

    using Bars.B4.Utils;

    using DomainService;

    using Bars.GkhCr.Services.DataContracts;

    /// <summary>
    /// Сервис для получение Информации о работе ДПКР (РТ)
    /// </summary>
    public partial class Service
    {
        /// <summary>
        /// Сервис получение работ ДПКР (РТ)
        /// </summary>
        public IGetProgramVersionService GetProgramVersionService { get; set; }

        /// <summary>
        /// Информация о работах ДПКР (РТ)
        /// </summary>
        /// <param name="municipalityId">МО</param>
        /// <param name="housesId">Уникальный номер дома</param>
        /// <returns>Информация о работе ДПКР и результат</returns>
        public GetDpkrWorkResponse GetDpkrWork(string municipalityId, string housesId)
        {
            if (this.GetProgramVersionService != null)
            {
                var DPKR = this.GetProgramVersionService.GetProgramVersion(municipalityId.ToLong(), housesId.ToLong());

                var result = DPKR.Length == 0 ? Result.DataNotFound : Result.NoErrors;

                return new GetDpkrWorkResponse {DPKR = DPKR, Result = result};
            }

            return new GetDpkrWorkResponse { DPKR = null, Result = Result.DataNotFound  };
        }
    }
}
