namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;

    using NHibernate;

    /// <summary>
    /// Вспомогательные метода сервиса
    /// </summary>
    public partial class ProgramVersionService
    {
        private void SaveOwnerDecision(BaseParams baseParams, Dictionary<long, Tuple<int, int>> stage1Changes)
        {
            var ownerDecision = baseParams.Params.ReadClass<ChangeYearOwnerDecision>();
            var file = baseParams.Files.ContainsKey("File") ? baseParams.Files["File"] : null;

            var ownerDecisionDomain = this.Container.ResolveDomain<ChangeYearOwnerDecision>();
            var fileInfoDomain = this.Container.ResolveDomain<FileInfo>();
            var fileManager = this.Container.Resolve<IFileManager>();

            using (this.Container.Using(ownerDecisionDomain, fileInfoDomain))
            {
                var fileInfo = fileManager.SaveFile(file);
                fileInfoDomain.Save(fileInfo);

                var ownerDecisionsForSave = stage1Changes
                    .Select(
                        stage1Change => new ChangeYearOwnerDecision
                        {
                            VersionRecordStage1 = new VersionRecordStage1 {Id = stage1Change.Key},
                            Date = ownerDecision.Date,
                            DocumentBase = ownerDecision.DocumentBase,
                            DocumentNumber = ownerDecision.DocumentNumber,
                            OldYear = stage1Change.Value.Item1,
                            NewYear = stage1Change.Value.Item2,
                            Remark = ownerDecision.Remark,
                            File = fileInfo
                        })
                    .ToList();

                ownerDecisionsForSave.ForEach(ownerDecisionDomain.Save);
            }
        }

        /// <summary>
        /// Создать новую версию по временным связкам с КЭ
        /// </summary>
        private MunicipalityVersion MakeNewVersion(VersionData versionData)
        {
            var muId = versionData.Municipality.Id;

            var version = new ProgramVersion
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                IsMain = versionData.IsMain,
                Municipality = versionData.Municipality,
                VersionDate = versionData.Date,
                ActualizeDate = DateTime.Now,
                Name = versionData.Name
            };

            var versionParams = new List<VersionParam>();

            this.Container.Resolve<IDomainService<CurrentPrioirityParams>>().GetAll()
                .Select(x => new {x.Code, x.Order})
                .ForEach(
                    x =>
                        versionParams.Add(
                            new VersionParam
                            {
                                Code = x.Code,
                                ProgramVersion = version,
                                Municipality = versionData.Municipality,
                                Weight = x.Order
                            }));

            var stage3Data = this.Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>().GetAll()
                .Where(
                    x => x.RealityObject.Municipality.Id == muId
                        || x.RealityObject.MoSettlement.Id == muId)
                .Select(
                    x => new
                    {
                        x.Id,
                        RealityObjectId = x.RealityObject.Id,
                        x.Year,
                        x.CommonEstateObjects,
                        x.Sum,
                        x.IndexNumber,
                        x.Point,
                        x.StoredCriteria,
                        x.StoredPointParams,
                    })
                .AsEnumerable();

            var stage2Data = this.Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage2>>().GetAll()
                .Where(
                    x => x.RealityObject.Municipality.Id == muId
                        || x.RealityObject.MoSettlement.Id == muId)
                .Select(
                    x => new
                    {
                        x.Id,
                        CommonEstateObjectId = x.CommonEstateObject.Id,
                        x.CommonEstateObject.Weight,
                        x.Sum,
                        Stage3Id = x.Stage3.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => y.ToArray());

            var stage1Data = this.Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>().GetAll()
                .Where(
                    x => x.Stage2.RealityObject.Municipality.Id == muId
                        || x.Stage2.RealityObject.MoSettlement.Id == muId)
                .Select(
                    x => new
                    {
                        x.Id,
                        Stage2Id = x.Stage2.Id,
                        RealityObjectId = x.StructuralElement.RealityObject.Id,
                        x.Year,
                        StructuralElementId = x.StructuralElement.Id,
                        x.StructuralElement.Volume,
                        x.Sum,
                        x.ServiceCost
                    })
                .AsEnumerable()
                .GroupBy(x => x.Stage2Id)
                .ToDictionary(x => x.Key, y => y.ToArray());

            var ver3S = new List<VersionRecord>();
            var ver2S = new List<VersionRecordStage2>();
            var ver1S = new List<VersionRecordStage1>();

            foreach (var stage3 in stage3Data)
            {
                var ver3 = new VersionRecord
                {
                    ProgramVersion = version,
                    RealityObject = new RealityObject {Id = stage3.RealityObjectId},
                    Year = stage3.Year,
                    YearCalculated = stage3.Year,
                    CommonEstateObjects = stage3.CommonEstateObjects,
                    Sum = stage3.Sum,
                    IndexNumber = stage3.IndexNumber,
                    Point = stage3.Point,
                    StoredCriteria = stage3.StoredCriteria,
                    StoredPointParams = stage3.StoredPointParams,
                    Show = true
                };

                ver3S.Add(ver3);

                if (stage2Data.ContainsKey(stage3.Id))
                {
                    var st2RecList = stage2Data[stage3.Id];

                    foreach (var stage2 in st2RecList)
                    {
                        var st2Version = new VersionRecordStage2
                        {
                            CommonEstateObject = new CommonEstateObject {Id = stage2.CommonEstateObjectId},
                            Stage3Version = ver3,
                            CommonEstateObjectWeight = stage2.Weight,
                            Sum = stage2.Sum
                        };

                        ver2S.Add(st2Version);

                        if (stage1Data.ContainsKey(stage2.Id))
                        {
                            var st1RecList = stage1Data[stage2.Id];
                            foreach (var stage1 in st1RecList)
                            {
                                ver1S.Add(
                                    new VersionRecordStage1
                                    {
                                        RealityObject = new RealityObject {Id = stage1.RealityObjectId},
                                        Stage2Version = st2Version,
                                        Year = stage1.Year,
                                        StructuralElement = new RealityObjectStructuralElement {Id = stage1.StructuralElementId},
                                        Sum = stage1.Sum,
                                        SumService = stage1.ServiceCost,
                                        Volume = stage1.Volume,
                                        VersionRecordState = Hmao.Enum.VersionRecordState.Actual                                        
                                    });
                            }
                        }
                    }
                }
            }

            var versionsForUpdate = versionData.IsMain
                ? this.Container.Resolve<IDomainService<ProgramVersion>>().GetAll()
                    .Where(x => x.IsMain && x.Municipality.Id == muId)
                    .ToList()
                : new List<ProgramVersion>();

            versionsForUpdate.ForEach(x => x.IsMain = false);

            var result = new MunicipalityVersion
            {
                ProgramVersion = version,
                Stage1Records = ver1S,
                Stage2Records = ver2S,
                Stage3Records = ver3S,
                VersionParams = versionParams,
                VersionsForUpdate = versionsForUpdate
            };

            return ver3S.Any() ? result : null;
        }

        private void InTransaction(Action<IStatelessSession> action)
        {
            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

            using (var tr = session.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    action(session);

                    tr.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }
}