namespace Bars.GkhCr.Services.Impl
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Services.DataContracts;
    using Bars.GkhCr.Services.DataContracts.KRInfo;

    /// <summary>
    /// Реализация метода KRInfo
    /// </summary>
    public partial class Service
    {
        /// <summary>
        /// Метод получения данных по программе капитального ремонта
        /// </summary>
        /// <param name="houseId">Id дома</param>
        /// <param name="houseofRegoperator">Выбирать или нет только те дома, у котроых "Способ формирования фонда" = "Счет рег. оператора"</param>
        /// <returns></returns>
        public KRInfoResponse KRContracts(string houseId, string houseofRegoperator = null)
        {
            long id;
            var programsKr = new ProgramKR[] { };

            if (long.TryParse(houseId, out id))
            {
                this.CacheContractorOrg(id, true);
                this.CacheWork(id, true);

                programsKr = this.ObjectCrService.GetAll()
                        .WhereIf(houseofRegoperator.ToBool(), x => x.RealityObject.AccountFormationVariant == CrFundFormationType.RegOpAccount)
                        .Where(x => x.RealityObject.Id == id)
                        .Where(x => x.ProgramCr.UsedInExport)
                        .Where(x => x.ProgramCr.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                        .Select(x => new ObjectCr(
                                        x.RealityObject, 
                                        new ProgramCr
                                        {
                                            Id = x.ProgramCr.Id,
                                            Name = x.ProgramCr.Name,
                                            Period = new Period { DateEnd = x.ProgramCr.Period.DateEnd }
                                        })
                                    {
                                        Id = x.Id
                                    })
                        .AsEnumerable()
                        .Select(this.GetProgramInfo)
                        .ToArray();

                this.CleanCache();
            }

            var result = programsKr.Length == 0 ? Result.DataNotFound : Result.NoErrors;

            return new KRInfoResponse { ProgramsKR = programsKr, Result = result };
        }
    }
}