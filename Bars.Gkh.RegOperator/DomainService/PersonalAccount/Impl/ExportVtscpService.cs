namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Linq;

    using B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.RegOperator.Domain.ImportExport;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Enums;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Mapping;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// ExportVtscpService
    /// </summary>
    public class ExportVtscpService : IExportVtscpService
    {
        /// <summary>
        /// Container
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult Export(BaseParams baseParams)
        {
            var importExportDataProvider = this.Container.Resolve<ImportExportDataProvider>();
            var persAccFilterService = this.Container.Resolve<IPersonalAccountFilterService>();
            var realObjectTariffService = this.Container.Resolve<IRealityObjectTariffService>();
            var persAccDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var realObjDomain = this.Container.ResolveDomain<RealityObject>();
            var roChargeAccountDomain = this.Container.ResolveDomain<RealityObjectChargeAccount>();
            var fileManager = this.Container.Resolve<IFileManager>();

            try
            {
                var mapper = new VtscpExportRecordDbfImportMap();
                var now = DateTime.Now;

                var accountIds = baseParams.Params.GetAs<string>("accountIds").ToLongArray();
                var queryByFilters = persAccFilterService.GetQueryableByFilters(baseParams, persAccDomain.GetAll())
                    .WhereIf(accountIds.Length > 0, x => accountIds.Contains(x.Id));

                var roQuery = realObjDomain.GetAll()
                    .Where(x => queryByFilters.Any(y => y.RoId == x.Id));

                var tarifInfoDict = realObjectTariffService.FillRealityObjectTariffInfoDictionary(roQuery);

                var realOnjHasCharge = roChargeAccountDomain.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .Where(x => x.Operations.Sum(y => y.ChargedTotal) > 0)
                    .Select(x => x.RealityObject.Id)
                    .ToHashSet();

                var data = roQuery.Select(x => new { x.Id, x.VtscpCode, x.FiasAddress.StreetName, x.FiasAddress.House, x.FiasAddress.Housing, x.RealEstateType.Code }).ToArray().Select(
                    x =>
                        {
                            var vtskpCodeParts = !x.VtscpCode.IsEmpty() ? x.VtscpCode.Split(":") : new string[2];
                            var rec = new VtscpExportRecord
                                          {
                                              Date = now,
                                              RegionCode = vtskpCodeParts[0] ?? string.Empty,
                                              Street = x.StreetName,
                                              House = x.House,
                                              Housing = x.Housing,
                                              VtscpCode = vtskpCodeParts[1] ?? string.Empty,
                                              Category = x.Code
                                          };

                            if (tarifInfoDict.ContainsKey(x.Id))
                            {
                                var tarifInfo = tarifInfoDict.Get(x.Id);

                                rec.Tarif = tarifInfo.Tarif;
                                rec.TarifStartDate = tarifInfo.DateStart.HasValue ? tarifInfo.DateStart.Value.ToShortDateString() : string.Empty;
                                rec.TarifEndDate = tarifInfo.DateEnd.HasValue ? tarifInfo.DateEnd.Value.ToShortDateString() : string.Empty;
                                rec.TarifType = tarifInfo.IsFromDecision ? "2" : "1";


                                var startDate = rec.TarifStartDate.ToDateTime();

                                if (startDate.Month == now.Month && startDate.Year == now.Year)
                                {
                                    rec.TarifSign = "2";
                                }
                            }

                            if (realOnjHasCharge.Contains(x.Id))
                            {
                                rec.TarifSign = "1";
                            }


                            return rec;
                        }).ToList();

                var result = importExportDataProvider.Serialize(data, mapper);

                var file = fileManager.SaveFile(result, "export.dbf");

                return new BaseDataResult(file.Id);
            }
            finally
            {
                this.Container.Release(importExportDataProvider);
                this.Container.Release(persAccFilterService);
                this.Container.Release(persAccDomain);
                this.Container.Release(realObjDomain);
                this.Container.Release(fileManager);
                this.Container.Release(roChargeAccountDomain);
                this.Container.Release(realObjectTariffService);
            }
        }
    }

    /// <summary>
    /// VtscpExportRecord
    /// </summary>
    public class VtscpExportRecord
    {
        public DateTime Date { get; set; }
        public string RegionCode { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string VtscpCode { get; set; }
        public string Housing { get; set; }
        public string Category { get; set; }
        public decimal Tarif { get; set; }
        public string TarifType { get; set; }
        public string TarifStartDate { get; set; }
        public string TarifEndDate { get; set; }
        public string TarifSign { get; set; }
        public string Dok { get; set; }
        public string Res { get; set; }
    }

    /// <summary>
    /// VtscpExportRecordDbfImportMap
    /// </summary>
    public class VtscpExportRecordDbfImportMap : AbstractImportMap<VtscpExportRecord>
    {
        public override string ProviderCode
        {
            get { return "default"; }
        }

        public override string ProviderName
        {
            get { return "Выгрузка ВЦКП"; }
        }

        public override string Format
        {
            get { return "dbf"; }
        }

        public override ImportExportType Direction
        {
            get { return ImportExportType.Import; }
        }

        /// <summary>
        /// мап VtscpExportRecordDbfImportMap
        /// </summary>
        public VtscpExportRecordDbfImportMap()
        {
             this.Map(x => x.Date, x => x.SetLookuper("DATE_B"), 20);
             this.Map(x => x.RegionCode, x => x.SetLookuper("NAME_RN"), 20);
             this.Map(x => x.Street, x => x.SetLookuper("UL"), 100);
             this.Map(x => x.House, x => x.SetLookuper("DOM"), 10);
             this.Map(x => x.VtscpCode, x => x.SetLookuper("KODD"), 20);
             this.Map(x => x.Housing, x => x.SetLookuper("KOR"), 10);
             this.Map(x => x.Category, x => x.SetLookuper("KAT_D"), 10);
             this.Map(x => x.Tarif, x => x.SetLookuper("TARIF"), 20);
             this.Map(x => x.TarifType, x => x.SetLookuper("TIP_T"), 10);
             this.Map(x => x.TarifStartDate, x => x.SetLookuper("DATE_N"), 20);
             this.Map(x => x.TarifEndDate, x => x.SetLookuper("DATE_K"), 20);
             this.Map(x => x.TarifSign, x => x.SetLookuper("PK"), 10);
             this.Map(x => x.Dok, x => x.SetLookuper("DOK"), 10);
             this.Map(x => x.Res, x => x.SetLookuper("RES"), 10);
        }
    }
}