namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;

    using CollectionExtensions = Castle.Core.Internal.CollectionExtensions;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel;

    using NHibernate.Transform;

    public class DpkrService : IDpkrService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<LoadProgram> LoadProgramDomain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RealityObjectStructuralElementDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IGkhConfigProvider ConfigProvider { get; set; }
        
        /// <summary>
        /// Минимальное количество квартир
        /// </summary>
        private int MinCountApartments => this.ConfigProvider.Get<OverhaulHmaoConfig>().HouseAddInProgramConfig.MinimumCountApartments;
        
        public IDataResult GetYears(BaseParams baseParams)
        {
            // ToDo Активировать блок когда получение данных станет как раньше нормальным, а не через загрузку своих данных

            #region Old
            //var startYear = OverhaullParamProvider.GetOverhaulParam<int>("ProgrammPeriodStart", Container);
            //var endYear = OverhaullParamProvider.GetOverhaulParam<int>("ProgrammPeriodEnd", Container);

            //var years = new List<int>();

            //while (startYear <= endYear)
            //{
            //    years.Add(startYear);
            //    startYear++;
            //}

            //return new BaseDataResult(years);
            #endregion

            var yearList =
                this.LoadProgramDomain.GetAll().Select(x => x.PlanOverhaulYear).AsEnumerable().Distinct().OrderBy(x => x).ToList();

            return new BaseDataResult(yearList);
        }

        public IDataResult GetMunicipality(BaseParams baseParams)
        {
            // ToDo Активировать блок когда получение данных станет как раньше нормальным, а не через загрузку своих данных

            #region Old
            //var municipalityList = MunicipalityDomain.GetAll()
            //                    .Select(x => new { x.Id, x.Name })
            //                    .OrderBy(x => x.Name)
            //                    .ToList();

            //return new BaseDataResult(municipalityList);
            #endregion

            // так как в загружаемом файле нет МО, то фильтруем по населенному пункту 
            var municipalityList = this.LoadProgramDomain.GetAll()
                .Select(x => x.Locality)
                .AsEnumerable()
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .Select(x => new
                {
                    Id = x,
                    Name = x
                })
                .OrderBy(x => x.Name)
                .ToList();

            return new BaseDataResult(municipalityList);
        }

        public IDataResult GetRealityObjects(BaseParams baseParams)
        {
            // ToDo Активировать блок когда получение данных станет как раньше нормальным, а не через загрузку своих данных

            #region Old
            //var municipalityId = baseParams.Params.GetAs<long>("muId");
            //var address = baseParams.Params.GetAs("address", string.Empty);

            //if (!string.IsNullOrEmpty(address))
            //{
            //    address = address.Replace(" ", "").Trim().ToUpper();
            //}

            //var roList = RoDomain.GetAll()
            //                    .WhereIf(municipalityId > 0, x => x.Municipality.Id == municipalityId)
            //                    .WhereIf(!string.IsNullOrEmpty(address), x => x.FiasAddress.AddressName.Replace(" ", "").ToUpper().Contains(address))
            //                    .Select(x => new { x.Id, x.Address })
            //                    .OrderBy(x => x.Address)
            //                    .ToList();

            //return new BaseDataResult(roList);
            #endregion

            // так как в загружаемом файле нет МО, то фильтруем по населенному пункту 
            var locality = baseParams.Params.GetAs("muId", string.Empty);
            var address = baseParams.Params.GetAs("address", string.Empty);

            if (!string.IsNullOrEmpty(address))
            {
                address = address.Replace(" ", "").Trim().ToUpper();
            }

            var roList = this.LoadProgramDomain.GetAll()
                .WhereIf(!CollectionExtensions.IsNullOrEmpty(locality), x => x.Locality == locality)
                .WhereIf(!string.IsNullOrEmpty(address), x => x.Address.Replace(" ", "").ToUpper().Contains(address))
                .Select(x => x.Address)
                .AsEnumerable()
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .Select(x => new
                {
                    Id = x,
                    Address = x
                })
                .OrderBy(x => x.Address)
                .ToList();

            return new BaseDataResult(roList);
        }

        public ListDataResult GetRecords(BaseParams baseParams)
        {
            // ToDo Активировать блок когда получение данных станет как раньше нормальным, а не через загрузку своих данных

            #region Old
            //var loadParams = GetLoadParam(baseParams);

            //var year = baseParams.Params.GetAs("year", 0);
            //var municipalityId = baseParams.Params.GetAs<long>("muId");
            //var roId = baseParams.Params.GetAs<long>("roId");

            //var startYear = OverhaullParamProvider.GetOverhaulParam<int>("ProgrammPeriodStart", Container);

            //var data =
            //    DpkrCorrectionDomain.GetAll()
            //                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
            //                        .WhereIf(year > 0, x => x.PublicationYear == year)
            //                        .WhereIf(municipalityId > 0, x => x.RealityObject.Municipality.Id == municipalityId)
            //                        .WhereIf(roId > 0, x => x.RealityObject.Id == roId)
            //                        .Select(
            //                            x =>
            //                            new
            //                            {
            //                                Id = x.Id,
            //                                x.Stage2.Stage3Version.IndexNumber,
            //                                st2Id = x.Stage2.Id,
            //                                Address = x.RealityObject.FiasAddress.AddressName,
            //                                MunicipalityName = x.RealityObject.Municipality.Name,
            //                                AddrPlace = x.RealityObject.FiasAddress.PlaceName,
            //                                AddrStreet = x.RealityObject.FiasAddress.StreetName,
            //                                AddrHouse = x.RealityObject.FiasAddress.House,
            //                                AddrHousing = x.RealityObject.FiasAddress.Housing,
            //                                YearComissioning = x.RealityObject.BuildYear.HasValue ? x.RealityObject.BuildYear.Value : 0, // Просили поменять год Эксплуатации на год постройки
            //                                CeoName = x.Stage2.CommonEstateObject.Name,
            //                                YearPublication = x.PublicationYear
            //                            })
            //                        .OrderBy(x => x.IndexNumber);

            //var totalCount = data.Count();

            ////поулчаем отрезанные записи по Пейджингу
            //var list = data.Paging(loadParams).ToList();

            //// поулчаем только Id 2 этапа версии
            //var listIds = list.Select(x => x.st2Id).Distinct().ToList();

            //var st1DataList = versStage1Domain.GetAll()
            //                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
            //                    .Where(x => listIds.Contains(x.Stage2Version.Id))
            //                    .WhereIf(municipalityId > 0, x => x.RealityObject.Municipality.Id == municipalityId)
            //                    .WhereIf(roId > 0, x => x.RealityObject.Id == roId)
            //                    .Select(x => new { x.StructuralElement.Id, st2Id = x.Stage2Version.Id })
            //                    .ToList();

            //var listStrElIds = st1DataList.Select(x => x.Id).Distinct().ToList();

            //var st1Data = st1DataList
            //                    .GroupBy(x => x.st2Id)
            //                    .ToDictionary(x => x.Key, y => y.Select(x => x.Id).First());

            //var strElData = strElDomain.GetAll()
            //                           .Where(x => listStrElIds.Contains(x.Id))
            //                           .WhereIf(
            //                               municipalityId > 0, x => x.RealityObject.Municipality.Id == municipalityId)
            //                           .WhereIf(roId > 0, x => x.RealityObject.Id == roId)
            //               .Select(x => new { x.Id, x.LastOverhaulYear })
            //               .Where(x => x.LastOverhaulYear > 0)
            //               .AsEnumerable()
            //               .GroupBy(x => x.Id)
            //               .ToDictionary(x => x.Key, y => y.Select(x => x.LastOverhaulYear).First());

            //var result = list.Select(x => new
            //{
            //    x.Id,
            //    x.MunicipalityName,
            //    x.AddrPlace,
            //    x.AddrStreet,
            //    x.AddrHouse,
            //    x.AddrHousing,
            //    x.YearComissioning,
            //    x.CeoName,
            //    YearLastOverhaul = st1Data.ContainsKey(x.st2Id) && strElData.ContainsKey(st1Data[x.st2Id]) && strElData[st1Data[x.st2Id]] < startYear
            //                ? (strElData[st1Data[x.st2Id]] > x.YearComissioning ? strElData[st1Data[x.st2Id]] : 0)
            //                : 0,
            //    x.YearPublication
            //})
            //    .ToList();

            //return new ListDataResult(result, totalCount);
            #endregion

            var loadParams = this.GetLoadParam(baseParams);

            var year = baseParams.Params.GetAs("year", 0);
            var locality = baseParams.Params.GetAs("muId", string.Empty);
            var address = baseParams.Params.GetAs("roId", string.Empty).ToUpper();

            var data =
                this.LoadProgramDomain.GetAll()
                    .WhereIf(year > 0, x => x.PlanOverhaulYear == year)
                    .WhereIf(!string.IsNullOrEmpty(locality), x => x.Locality == locality)
                    .WhereIf(!string.IsNullOrEmpty(address), x => x.Address.ToUpper().Contains(address))
                    .Select(
                        x =>
                            new
                            {
                                Id = x.IndexNumber,
                                MunicipalityName = x.Locality,
                                AddrPlace = x.Address,
                                AddrStreet = x.Street,
                                AddrHouse = x.House,
                                AddrHousing = x.Housing,
                                YearComissioning = x.CommissioningYear,
                                CeoName = x.CommonEstateobject,
                                YearLastOverhaul = x.LastOverhaulYear,
                                YearPublication = x.PlanOverhaulYear
                            })
                    .OrderBy(x => x.Id);

            var totalCount = data.Count();

            //поулчаем отрезанные записи по Пейджингу
            var list = data.Paging(loadParams).ToList();

            return new ListDataResult(list, totalCount);
        }


        #region Запросы для виджетов
        
        
        /// <inheritdoc />
        public IDataResult MunicipalityListWithoutPaging(BaseParams baseParams)
        {
            var municipalityDomain = this.Container.ResolveDomain<Municipality>();

            using (this.Container.Using(municipalityDomain))
            {
                var municipalityIds = this.GetMunicipalityIds();

                return municipalityDomain.GetAll()
                    .WhereIfContains(municipalityIds.IsNotEmpty(), x => x.Id, municipalityIds)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name
                    })
                    .OrderBy(x => x.Name)
                    .ToListDataResult(baseParams.GetLoadParam(), usePaging: false);
            }
        }
        
        /// <inheritdoc />
        public IDataResult GetNotIncludedInCrHousesCount(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            var data = this.GetNotIncludedInCrHousesQuery(true, municipalityId);

            using (var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession())
            {
                var queryResult = session.CreateSQLQuery(data.Query)
                    .SetParams(data.Params)
                    .SetResultTransformer(Transformers.AliasToBean<CountQueryDto>())
                    .UniqueResult<CountQueryDto>();

                var percentage = this.GetPercentage(queryResult.Count, queryResult.TotalCount);

                return new BaseDataResult(new { Count = queryResult.Count, Percentage = percentage });
            }
        }

        /// <inheritdoc />
        public IDataResult GetWorksNotIncludedPublishProgramCount(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            var stage3Domain = this.Container.ResolveDomain<VersionRecord>();
            var pubProgramRecordDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var stage2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            
            using (this.Container.Using(stage3Domain, pubProgramRecordDomain, stage2Domain))
            {
                var municipalityIds = this.GetMunicipalityIds();
                
                var versionRecords = stage3Domain.GetAll()
                    .Where(x => x.ProgramVersion.IsMain)
                    .WhereIfElse(municipalityId > 0, x => x.ProgramVersion.Municipality.Id == municipalityId, 
                        x => municipalityIds.Contains(x.ProgramVersion.Municipality.Id));
                
                var totalCount = versionRecords.Count();
                
                var pubVersionCount = pubProgramRecordDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .WhereIfElse(municipalityId > 0, x => x.PublishedProgram.ProgramVersion.Municipality.Id == municipalityId,
                        x => municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                    .Select(x => x.Stage2.Stage3Version.Id)
                    .Distinct()
                    .Count();
                var nonPubVersionCount = totalCount - pubVersionCount;
                
                var percentage = this.GetPercentage(nonPubVersionCount, totalCount);

                return new BaseDataResult(new { Count = nonPubVersionCount, Percentage = percentage });
            }
        }

        /// <inheritdoc />
        public IDataResult GetHousesWithNotFilledFias(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var baseQuery = this.GetRealityObjectBaseQuery(municipalityId);

            var totalCount = baseQuery.Count();

            var count = baseQuery
                .Where(x => !x.FiasAddress.HouseGuid.HasValue)
                .Select(x => x.Id)
                .Count();

            var percentage = this.GetPercentage(count, totalCount);
                
            return new BaseDataResult(new { Count = count, Percentage = percentage });
        }

        /// <inheritdoc />
        public IDataResult GetIncludedInCrHousesByYearsCount(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var baseQuery = this.GetDpkrDocumentRealityObjectBaseQuery(municipalityId);

            var totalCount = baseQuery.Count();

            return baseQuery
                .AsEnumerable()
                .GroupBy(x => x.DpkrDocument.DocumentDate.Value.Year)
                .Select(x => new
                {
                    Year = x.Key,
                    Count = x.Count(),
                    TotalCount = totalCount
                })
                .ToList()
                .ToBaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult GetHousesWithMissingParamsCount(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var baseQuery = this.GetRealityObjectBaseQuery(municipalityId);
            var missingCeoDomain = this.Container.ResolveDomain<RealityObjectMissingCeo>();
            
            using (this.Container.Using(this.RealityObjectStructuralElementDomain, missingCeoDomain))
            {
                var emptyParams = this.RealityObjectStructuralElementDomain
                    .GetAll()
                    .Where(x => baseQuery.Any(bq => bq.Id == x.RealityObject.Id))
                    .Where(x => x.StructuralElement.Group.CommonEstateObject.IncludedInSubjectProgramm)
                    .Where(x => x.LastOverhaulYear == 0 || x.Volume == 0)
                    .Select(x => new
                    {
                        RealityObjectId = x.RealityObject.Id,
                        MunicipalityId = x.RealityObject.Municipality.Id,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        Address = x.RealityObject.Address,
                        MissingParameter = x.LastOverhaulYear == 0 ? "Год установки или последнего кап. ремонта" : "Объём",
                        ParameterName = x.StructuralElement.Group.Name,
                    });

                var municipalityIds = this.GetMunicipalityIds();
                
                var missingCeoParams = missingCeoDomain
                    .GetAll()
                    .WhereIfElse(municipalityId > 0, x => x.RealityObject.Municipality.Id == municipalityId,
                        x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => baseQuery.Any(bq => bq.Id == x.RealityObject.Id))
                    .Where(x => x.MissingCommonEstateObject.IncludedInSubjectProgramm)
                    .Select(x => new
                    {
                        RealityObjectId = x.RealityObject.Id,
                        MunicipalityId = x.RealityObject.Municipality.Id,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        Address = x.RealityObject.Address,
                        MissingParameter = "КЭ",
                        ParameterName = x.MissingCommonEstateObject.Name,
                    });

                var result = emptyParams.ToList();
                result.AddRange(missingCeoParams);

                var totalCount = baseQuery.Count();
                var count = result
                    .Select(x => x.RealityObjectId)
                    .Distinct()
                    .Count();

                var percentage = this.GetPercentage(count, totalCount);

                return new BaseDataResult(new { Count = count, Percentage = percentage });
            }
        }

        /// <inheritdoc />
        public IDataResult GetCrBudgetingCount(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var subsidyRecordVersionDomain = this.Container.ResolveDomain<SubsidyRecordVersion>();

            using (this.Container.Using(subsidyRecordVersionDomain))
            {
                var municipalityIds = this.GetMunicipalityIds();
                
                var baseQuery = subsidyRecordVersionDomain.GetAll()
                    .Where(x => x.Version.IsMain)
                    .Where(x => x.CorrectionFinance != default(long))
                    .WhereIfElse(municipalityId != default(long), x => x.Version.Municipality.Id == municipalityId,
                        x => municipalityIds.Contains(x.Version.Municipality.Id));
                
                object data;

                if (municipalityId > 0)
                {
                    var result = baseQuery
                        .Select(x => new
                        {
                            Name = x.SubsidyYear,
                            Value = x.CorrectionFinance
                        })
                        .OrderBy(x => x.Name)
                        .AsEnumerable();

                    data = result
                        .Select(x => new
                        {
                            x.Name,
                            Value = x.Value.RoundDecimal()
                        })
                        .ToArray();
                }
                else
                {
                    data = baseQuery
                        .GroupBy(x => new { x.Version.Municipality.Id, x.Version.Municipality.Name },
                            (x, y) => new
                            {
                                x.Name,
                                Value = y.Sum(z => z.CorrectionFinance)
                            })
                        .OrderBy(x => x.Value)
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.Name,
                            Value = x.Value.RoundDecimal()
                        })
                        .ToArray();
                }
                
                return new BaseDataResult(data);
            }
        }
        
        /// <inheritdoc />
        public IDataResult GetCrCeoWorkCounts(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var publishedProgramRecordDomain = this.Container.ResolveDomain<PublishedProgramRecord>();

            using (this.Container.Using(publishedProgramRecordDomain))
            {
                var municipalityIds = this.GetMunicipalityIds();
                
                return publishedProgramRecordDomain.GetAll()
                    .WhereIfElse(municipalityId != default(long), x => x.PublishedProgram.ProgramVersion.Municipality.Id == municipalityId,
                        x => municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .Where(x => x.Stage2.CommonEstateObject.IncludedInSubjectProgramm)
                    .GroupBy(x => x.Stage2.CommonEstateObject.ShortName,
                        (x, y) => new CrCeoWorkReportDataDto
                        {
                            CommonEstateObjectName = x,
                            WorkCount = y.Count()
                        })
                    .ToList()
                    .ToBaseDataResult();
            }
        }
        
        /// <inheritdoc />
        public IDataResult GetCostOfWorksInStructuralElementContext(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var stage1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var publishedProgramRecordDomain = this.Container.ResolveDomain<PublishedProgramRecord>();

            using (this.Container.Using(stage1Domain, publishedProgramRecordDomain))
            {
                var municipalityIds = this.GetMunicipalityIds();
                
                return stage1Domain.GetAll()
                    .WhereIfElse(municipalityId != default(long), 
                        x => x.Stage2Version.Stage3Version.ProgramVersion.Municipality.Id == municipalityId,
                        x => municipalityIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Municipality.Id))
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                    .Where(x => publishedProgramRecordDomain.GetAll()
                        .Any(y => y.Stage2.Id == x.Stage2Version.Id 
                            && y.Stage2.Stage3Version.ProgramVersion.Id == x.Stage2Version.Stage3Version.ProgramVersion.Id))
                    .Select(x => new
                    {
                        x.StructuralElement.StructuralElement.Name,
                        x.Stage2Version.Stage3Version.Sum
                    })
                    .GroupBy(x => x.Name,
                        (x, y) => new CostOfWorksInStructuralElementContextReportDataDto
                        {
                            StructuralElementName = x,
                            WorkCost = y.Sum(s => s.Sum)
                        })
                    .ToList()
                    .ToBaseDataResult();
            }
        }
        
        /// <inheritdoc />
        public ReportStreamResult GetExcelFileExport(BaseParams baseParams)
        {
            var reportKey = baseParams.Params.GetAs<string>("reportName");

            var reportGenerator = this.Container.Resolve<IReportGenerator>("XlsIoGenerator");
            var dataExportReport = this.Container.Resolve<IDataExportReport>(reportKey, new Arguments
            {
                {"BaseParams", baseParams}
            });
            
            using (this.Container.Using(reportGenerator, dataExportReport))
            {
                var reportParams = new ReportParams();
                var memoryStream = new MemoryStream();
                dataExportReport.PrepareReport(reportParams);
            
                var template = dataExportReport.GetTemplate();
                reportGenerator.Open(template);
                reportGenerator.Generate(memoryStream, reportParams);
            
                return new ReportStreamResult(memoryStream, "export.xlsx");
            }
        }

        /// <summary>
        /// Получить базовый запрос с общими условиями для объектов
        /// </summary>
        /// <param name="municipalityId">Идентификатор МО</param>
        /// <returns name="baseQuery"></returns>
        public IQueryable<RealityObject> GetRealityObjectBaseQuery(long municipalityId)
        {
            var municipalityIds = this.GetMunicipalityIds();
            
            return this.RealityObjectDomain.GetAll()
                .WhereIfElse(municipalityId > 0, x => x.Municipality.Id == municipalityId, 
                    x => municipalityIds.Contains(x.Municipality.Id))
                .Where(x => new[] { ConditionHouse.Serviceable, ConditionHouse.Dilapidated }.Contains(x.ConditionHouse))
                .Where(x => x.RealEstateType.Name.ToLower() != "не задано")
                .Where(x => !x.IsNotInvolvedCr)
                .Where(x => x.NumberApartments >= this.MinCountApartments)
                .Where(x => this.RealityObjectStructuralElementDomain
                    .GetAll()
                    .Where(y => x.Id == y.RealityObject.Id)
                    .Any(y => y.State.StartState));
        }

        /// <summary>
        /// Получить базовый запрос с общими условиями для документов ДПКР, привязанных к объектам
        /// </summary>
        /// <param name="municipalityId">Идентификатор МО</param>
        /// <returns name="baseQuery"></returns>
        public IQueryable<DpkrDocumentRealityObject> GetDpkrDocumentRealityObjectBaseQuery(long municipalityId)
        {
            var dpkrDocumentRealityObjectDomain = this.Container.ResolveDomain<DpkrDocumentRealityObject>();

            using (this.Container.Using(dpkrDocumentRealityObjectDomain))
            {
                IQueryable<DpkrDocumentRealityObject> baseQuery;
                
                if (municipalityId > 0)
                {
                    baseQuery = dpkrDocumentRealityObjectDomain.GetAll()
                        .Where(x => x.RealityObject.Municipality.Id == municipalityId);
                }
                else
                {
                    var municipalityIds = this.GetMunicipalityIds();

                    baseQuery = dpkrDocumentRealityObjectDomain.GetAll()
                        .WhereIfContains(municipalityIds.IsNotEmpty(), x => x.RealityObject.Municipality.Id, municipalityIds);
                }
                

                var excludedQuery = baseQuery.Where(x => x.IsExcluded);

                return baseQuery
                    .Where(x => !x.IsExcluded && x.DpkrDocument.DocumentDate != null)
                    .Where(x => !excludedQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id));
            }
        }

        /// <summary>
        /// Собрать запрос для выгрузки по домам, не попавшим в версии ДПКР 
        /// </summary>
        /// <param name="onlyCount"></param>
        /// <param name="municipalityId"></param>
        /// <returns></returns>
        public CountQueryDto GetNotIncludedInCrHousesQuery(bool onlyCount, long municipalityId)
        {
            var @params = new List<(string, object)>
            {
                ("house_types", new[] { ConditionHouse.Serviceable, ConditionHouse.Dilapidated }),
                ("min_count_apartments", this.MinCountApartments)
            };
            
            var query = new StringBuilder(@"
                FROM public.GKH_REALITY_OBJECT ro
	                INNER JOIN public.GKH_DICT_MUNICIPALITY gdm
		                ON gdm.id = ro.municipality_id
	                LEFT JOIN public.OVRHL_REAL_ESTATE_TYPE oret
		                ON oret.id = ro.real_est_type_id
	                LEFT JOIN (
		                public.OVRHL_VERSION_REC rec
			                INNER JOIN public.OVRHL_PRG_VERSION programver2
				                ON programver2.id = rec.version_id
				                AND programver2.is_main
	                ) ON rec.ro_id = ro.id
                WHERE
	                ro.condition_house IN (:house_types)
	                AND NOT ro.is_not_involved_cr
	                AND ro.number_apartments >= :min_count_apartments
	                AND COALESCE(oret.name, '') !~* 'не задано'
	                AND EXISTS (
		                SELECT FROM public.OVRHL_RO_STRUCT_EL el
			                INNER JOIN public.B4_STATE bs
				                ON el.state_id = bs.id
				                AND bs.start_state
		                WHERE
			                ro.id = el.ro_id
	                )"
            );

            query.Insert(0, onlyCount
                ? @"SELECT
                        COUNT(DISTINCT ro.id) AS ""TotalCount"",
                        COUNT(DISTINCT CASE WHEN rec.id IS NULL THEN ro.id END) AS ""Count"""
                : @"SELECT DISTINCT ON (ro.id)
	                    gdm.id AS ""MunicipalityId"",
	                    gdm.name AS ""MunicipalityName"",
	                    ro.address AS ""Address""");

            if (municipalityId > default(long))
            {
                query.AppendLine("AND ro.municipality_id = :municipality_id");
                @params.Add(("municipality_id", municipalityId));
            }
            else
            {
                var municipalityIds = this.GetMunicipalityIds();
                query.AppendLine("AND ro.municipality_id IN (:municipalityIds)");
                @params.Add(("municipalityIds", municipalityIds));
            }

            if (!onlyCount)
            {
                query.AppendLine("AND rec.id IS NULL");
            }

            return new CountQueryDto { Query = query.ToString(), Params = @params };
        }
        
        public List<long> GetMunicipalityIds()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var municipalityDomain = this.Container.ResolveDomain<Municipality>();

            using (this.Container.Using(userManager, municipalityDomain))
            {
                var municipalityIds = userManager.GetMunicipalityIds();

                return municipalityDomain.GetAll()
                    .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Id))
                    .Where(x => x.Level == TypeMunicipality.UrbanArea)
                    .Select(x => x.Id)
                    .ToList();
            }
        }

        /// <summary>
        /// DTO выборки
        /// </summary>
        public class CountQueryDto
        {
            public long Count { get; set; }
            public long TotalCount { get; set; }
            public string Query { get; set; }
            public List<(string, object)> Params { get; set; }
        }
        
        /// <summary>
        /// DTO данных для выгрузки количества работ ДПКР в разрезе ООИ
        /// </summary>
        public class CrCeoWorkReportDataDto
        {
            public string CommonEstateObjectName { get; set; }
            public int WorkCount { get; set; }
        }
        
        /// <summary>
        /// DTO данных для выгрузки стоимости работ в разрезе КЭ
        /// </summary>
        public class CostOfWorksInStructuralElementContextReportDataDto
        {
            public string StructuralElementName { get; set; }
            public decimal WorkCost { get; set; }
        }

        /// <summary>
        /// Вычислить процент
        /// </summary>
        /// <param name="totalCount">Общее количество</param>
        /// <param name="count">Значение</param>
        private decimal GetPercentage(long count, long totalCount) =>
            totalCount != default(long)
                ? decimal.Round(count.ToDecimal() / totalCount.ToDecimal() * 100, 1, MidpointRounding.AwayFromZero)
                : default(long);
        
        #endregion

        private LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>().Execute(Bars.B4.DomainService.BaseParams.Converter.ToLoadParam);
        }
    }
}