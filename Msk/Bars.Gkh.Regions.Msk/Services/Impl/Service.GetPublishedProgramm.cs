namespace Bars.Gkh.Regions.Msk.Services.Impl
{
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Services.DataContracts;
    using Bars.Gkh.Regions.Msk.Entities;

    public partial class Service
    {
        public GetPublishedProgrammResponse GetPublishedProgramm()
        {
            var roInfoDomain = Container.ResolveDomain<RealityObjectInfo>();

            try
            {
                var data = roInfoDomain.GetAll()
                    .OrderBy(x => x.Okrug)
                    .ThenBy(x => x.Raion)
                    .ThenBy(x => x.Address)
                    .Select(x => new 
                    {
                        x.Uid,
                        x.Okrug,
                        x.Raion,
                        x.Address,
                        x.BuildingYear,
                        x.TotalArea,
                        x.EsPeriod,
                        x.GasPeriod,
                        x.HvsPeriod,
                        x.HvsmPeriod,
                        x.GvsPeriod,
                        x.GvsmPeriod,
                        x.KanPeriod,
                        x.KanmPeriod,
                        x.OtopPeriod,
                        x.OtopmPeriod,
                        x.MusPeriod,
                        x.PpiaduPeriod,
                        x.PvPeriod,
                        x.FasPeriod,
                        x.KrovPeriod,
                        x.VdskPeriod,
                        x.LiftPeriod
                    })
                    .ToList()
                    .Select(x => new GetPublishedProgrammRecord
                    {
                        Uid = x.Uid,
                        Okrug = x.Okrug,
                        Raion = x.Raion,
                        Address = x.Address,
                        BuildingYear = x.BuildingYear,
                        TotalArea = x.TotalArea.RoundDecimal(2).ToString("G29"),
                        EsPeriod = x.EsPeriod,
                        GasPeriod = x.GasPeriod,
                        HvsPeriod = x.HvsPeriod,
                        HvsmPeriod = x.HvsmPeriod,
                        GvsPeriod = x.GvsPeriod,
                        GvsmPeriod = x.GvsmPeriod,
                        KanPeriod = x.KanPeriod,
                        KanmPeriod = x.KanmPeriod,
                        OtopPeriod = x.OtopPeriod,
                        OtopmPeriod = x.OtopmPeriod,
                        MusPeriod = x.MusPeriod,
                        PpiaduPeriod = x.PpiaduPeriod,
                        PvPeriod = x.PvPeriod,
                        FasPeriod = x.FasPeriod,
                        KrovPeriod = x.KrovPeriod,
                        VdskPeriod = x.VdskPeriod,
                        LiftPeriod = x.LiftPeriod
                    }).ToArray();

                return new GetPublishedProgrammResponse
                {
                    Records = data
                };
            }
            finally
            {
                Container.Release(roInfoDomain);
            }
        }
    }
}