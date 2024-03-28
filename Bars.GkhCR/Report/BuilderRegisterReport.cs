namespace Bars.GkhCr.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class BuilderRegisterReport : BasePrintForm
    {
        private long programCrId;
        private long[] municipalityIds;

        public BuilderRegisterReport()
            : base(new ReportTemplateBinary(Properties.Resources.BuilderRegister))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.BuilderRegister";
            }
        }

        public override string Name 
        { 
            get { return "Реестр подрядчиков";  }
        }

        public override string Desciption 
        { 
            get { return "Реестр подрядчиков";  }
        }

        public override string GroupName
        { 
            get { return "Отчеты ГЖИ";  }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.BuilderRegister"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();

            var municipalityIdsList = baseParams.Params.ContainsKey("municipalityIds")
                      ? baseParams.Params["municipalityIds"].ToString()
                      : string.Empty;

            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["DateActuality"] = DateTime.Today.ToShortDateString();

            var builderContract = Container.Resolve<IDomainService<BuildContract>>().GetAll()
                            .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                            .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id));

            var dataBuilderContract = builderContract
                .Select(x => new 
                            {
                                id = x.Id,
                                BuilderId = (long?)x.Builder.Id,
                                BuilderContrId = (long?)x.Builder.Contragent.Id,
                                
                                BuilderContrName = x.Builder.Contragent.Name ?? string.Empty,
                                Inn = x.Builder.Contragent.Inn ?? string.Empty,
                                JuridicalAddress = x.Builder.Contragent.JuridicalAddress ?? string.Empty,
                                AddressOutsideSubject = x.Builder.Contragent.AddressOutsideSubject ?? string.Empty,

                                FiasJuridicalAddress = (long?)x.Builder.Contragent.FiasJuridicalAddress.Id,
                                FiasJurPlaceName = x.Builder.Contragent.FiasJuridicalAddress.PlaceName ?? string.Empty,
                                FiasJurStreetName = x.Builder.Contragent.FiasJuridicalAddress.StreetName ?? string.Empty,
                                FiasJurHouse = x.Builder.Contragent.FiasJuridicalAddress.House ?? string.Empty,

                                FiasOutsideSubjectAddress = (long?)x.Builder.Contragent.FiasOutsideSubjectAddress.Id,
                                FiasOutsPlaceName = x.Builder.Contragent.FiasOutsideSubjectAddress.PlaceName ?? string.Empty,
                                FiasOutsStreetName = x.Builder.Contragent.FiasOutsideSubjectAddress.StreetName ?? string.Empty,
                                FiasOutsHouse = x.Builder.Contragent.FiasOutsideSubjectAddress.House ?? string.Empty,

                                MunicipalityName = x.ObjectCr.RealityObject.Municipality.Name ?? string.Empty,
                                
                                RoId = x.ObjectCr.Id
                            })
                .OrderBy(x => x.MunicipalityName)
                .ThenBy(x => x.BuilderContrName)
                .ToList();

            var contragentsQuery = builderContract.Select(x => x.Builder.Contragent.Id);

            var requisites = Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                .Where(x => contragentsQuery.Contains(x.Contragent.Id))
                .Where(q => q.Position.Code == "1")
                .Select(v => new
                {
                    v.DateEndWork,
                    v.Contragent.Id,
                    v.FullName,
                    v.Phone
                })
                .ToList()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, 
                    x => 
                    x.Select(y => new
                        {
                            y.FullName,
                            y.Phone,
                            y.DateEndWork
                        })
                    .FirstOrDefault());
            
            var groupedByMUandBuilder = dataBuilderContract
                .Where(y => y.BuilderId != null)
                .GroupBy(y => new { y.MunicipalityName, y.BuilderId })
                .ToDictionary(z => z.Key, 
                    z => 
                    z.Select(x =>
                        {
                            var builderContrId = x.BuilderContrId ?? -1;
                            var addressFias = x.FiasJuridicalAddress != null
                                                  ? x.FiasJurPlaceName + ", " + x.FiasJurStreetName + ", " + x.FiasJurHouse
                                                  : string.Empty;

                            var addressOutsideFias = x.FiasOutsideSubjectAddress != null
                                                         ? x.FiasOutsPlaceName + ", " + x.FiasOutsStreetName + ", " + x.FiasOutsHouse
                                                         : string.Empty;

                            var dogovorCount = z.Count();

                            var countHouses = z.Select(y => y.RoId).Distinct().Count();
                            
                            return new
                                {
                                    BuilderName = x.BuilderContrName,
                                    BuilderContrId = builderContrId,
                                    BuilderINN = x.Inn,
                                    address = x.JuridicalAddress,
                                    addressFias,
                                    addressOutside = x.AddressOutsideSubject,
                                    addressOutsideFias,
                                    z.Key.MunicipalityName,
                                    dogovorCount,
                                    countHouses
                                };
                        })
                    .FirstOrDefault());
                
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("row");

            int i1 = 0, i2 = 0;
            var uniqMunicipality = string.Empty;

            foreach (var row in groupedByMUandBuilder)
            {
                section.ДобавитьСтроку();
                
                if (uniqMunicipality == row.Key.MunicipalityName)
                {
                    ++i2;
                }
                else
                {
                    i2 = 1;
                    uniqMunicipality = row.Key.MunicipalityName;
                }

                section["NumberPP"] = ++i1;
                section["NumberMO"] = i2;
                section["MO"] = row.Key.MunicipalityName;
                section["BuilderName"] = row.Value.BuilderName;
               
                var address = string.IsNullOrEmpty(row.Value.addressFias) ? row.Value.addressOutsideFias : row.Value.addressFias; // если не указан юридический, то за прелеами субъекта (ФИАС)
                if (string.IsNullOrEmpty(address)) address = string.IsNullOrEmpty(row.Value.address) ? row.Value.addressOutside : row.Value.address; // если не указаны предыдущие варинаты, то обычные адреса

                if (!requisites.ContainsKey(row.Value.BuilderContrId.ToInt()))
                {
                    section["BuilderInfo"] = string.Empty;
                }
                else
                {
                    var director = requisites[row.Value.BuilderContrId.ToInt()];
                    var currentDate = DateTime.Today;

                    if (director.DateEndWork >= currentDate || director.DateEndWork == null)
                    {
                        section["BuilderInfo"] = string.Format("{0}; {1}; {2}", address, director.FullName, requisites[row.Value.BuilderContrId.ToInt()].Phone);
                    }
                }

                section["BuilderInn"] = row.Value.BuilderINN;
                section["CountAgrements"] = row.Value.dogovorCount;
                section["CountHouses"] = row.Value.countHouses;
            }
        }
    }
}