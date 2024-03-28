namespace Bars.GkhDi.Services.Impl
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;

    using Gkh.Services.DataContracts;

    public partial class Service
    {
        /// <summary>
        /// Репозиторий для <see cref="RealityObject"/>
        /// </summary>
        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="PeriodDi"/>
        /// </summary>
        public IRepository<PeriodDi> PeriodDiRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="DisclosureInfoRealityObj"/>
        /// </summary>
        public IRepository<DisclosureInfoRealityObj> DisclosureInfoRealityObjRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="InfoAboutUseCommonFacilities"/>
        /// </summary>
        public IRepository<InfoAboutUseCommonFacilities> InfoAboutUseCommonFacilitiesRepository { get; set; }

        /// <summary>
        /// Получить сведения о договоре пользования общим имуществом МКД
        /// </summary>
        /// <param name="roId">Идентификатор жилого дома</param>
        /// <param name="periodId">Идентификатор периода раскрытия информации</param>
        /// <returns>Результат выполнения запроса</returns>
        public GetDoiContractResponse GetDoiContract(string roId, string periodId)
        {
            var result = Result.NoErrors;

            var realityObject = this.RealityObjectRepository.Get(roId.ToLong());
            if (realityObject == null)
            {
                result = Result.DataNotFound;
                result.Message = "По введенному параметру дом не найден";

                return new GetDoiContractResponse {Result = result};
            }

            var periodDi = this.PeriodDiRepository.Get(periodId.ToLong());
            if (periodDi == null)
            {
                result = Result.DataNotFound;
                result.Message = "По введенному параметру период раскрытия информации не найден";

                return new GetDoiContractResponse {Result = result};
            }

            var disclosureInfoRo = this.DisclosureInfoRealityObjRepository
                .GetAll()
                .Where(x => x.RealityObject.Id == roId.ToLong())
                .FirstOrDefault(x => x.PeriodDi.Id == periodId.ToLong());

            if (disclosureInfoRo == null)
            {
                result = Result.DataNotFound;
                result.Message = string.Format(
                    "Объект недвижимости \"{0}\" в периоде раскрытия информации \"{1}\" не найден",
                    realityObject.Address,
                    periodDi.Name);

                return new GetDoiContractResponse {Result = result};
            }

            return new GetDoiContractResponse
            {
                Result = result,
                CommonFacilitiesInfo = this.GetCommonFacilitiesInfo(disclosureInfoRo.Id)
            };
        }

        private CommonFacilityInfo[] GetCommonFacilitiesInfo(long disclosureInfoRoId)
        {
            var commonFacilitiesInfo = this.InfoAboutUseCommonFacilitiesRepository.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRoId)
                .Select(x => new
                    {
                        x.Id,
                        x.ContractNumber,
                        x.ContractDate,
                        x.CostContract,
                        x.CostByContractInMonth,
                        x.KindCommomFacilities,
                        x.Lessee,
                        x.DateStart,
                        x.DateEnd
                    })
                .AsEnumerable()
                .Select(x => new CommonFacilityInfo
                    {
                        Id = x.Id,
                        ContractNumber = x.ContractNumber,
                        ContractDate = x.ContractDate != null ? x.ContractDate.Value.ToShortDateString() : string.Empty,
                        CostContract = x.CostByContractInMonth.ToDecimal(),
                        KindCommomFacilities = x.KindCommomFacilities,
                        Lessee = x.Lessee,
                        DateStart = x.DateStart != null ? x.DateStart.Value.ToShortDateString() : string.Empty,
                        DateEnd = x.DateEnd != null ? x.DateEnd.Value.ToShortDateString() : string.Empty
                    })
                .ToArray();

            return commonFacilitiesInfo;
        }
    }
}
