namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using B4;
    using Entities;
    using GkhCr.DomainService;
    using GkhCr.Services;
    using System.Linq;


    /// <summary>
    /// Сервис получение работ ДПКР (РТ)
    /// </summary>
    public class GetProgramVersionService : IGetProgramVersionService
    {
        /// <summary>
        /// Домен-сервиса работ ДПКР (РТ)
        /// </summary>
        public IDomainService<VersionRecord> VersionRecordService { get; set; }

        /// <summary>
        /// Получение работ ДПКР (РТ)
        /// </summary>
        /// <param name="municipalityId">МО</param>
        /// <param name="housesId">Жилой дом</param>
        /// <returns>Маcсив работ ДПКР (РТ)</returns>
        public DPKR[] GetProgramVersion(long municipalityId, long housesId)
        {
            return this.VersionRecordService.GetAll()
                .Where(x => x.ProgramVersion.Municipality.Id == municipalityId)
                .Where(x => x.ProgramVersion.IsMain)
                .Where(x => x.RealityObject.Id == housesId)
                .Select(
                    x => new DPKR
                    {
                        Id = x.Id,
                        Year = x.Year,
                        CorrectYear = x.CorrectYear,
                        Sum = x.Sum,
                        CommonEstateObject = x.CommonEstateObjects
                    })
                .ToArray();
        }
    }
}
