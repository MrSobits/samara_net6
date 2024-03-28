namespace Bars.Gkh.DomainService.Impl
{
    using System;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class GkhScriptService : IGkhScriptService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult CorrectJskTsjContract(BaseParams baseParams)
        {
            var repManOrgContractRealityObject = Container.Resolve<IRepository<ManOrgContractRealityObject>>();
            var repManOrgJskTsjContract = Container.Resolve<IRepository<ManOrgJskTsjContract>>();
            var fileManager = this.Container.Resolve<IFileManager>();

            var dictJskTsjRealityObject = repManOrgContractRealityObject.GetAll()
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj)
                .Select(x => new { x.Id, JskTsjId = x.ManOrgContract.Id, RealityObjectId = x.RealityObject.Id })
                .AsEnumerable()
                .GroupBy(x => x.JskTsjId)
                .ToDictionary(x => x.Key, x => x
                    .Select(y => new
                    {
                        y.Id,
                        y.RealityObjectId
                    })
                    .ToArray());

            int countContracts = 0;

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var contractRealityObject in dictJskTsjRealityObject.Where(x => x.Value.Length > 1))
                    {
                        var contrRealityObjects = contractRealityObject.Value;

                        var jskTsjContract = repManOrgJskTsjContract.Get(contractRealityObject.Key);

                        for (var index = 1; index < contrRealityObjects.Length; index++)
                        {
                            countContracts++;

                            var newJskTsjContract = new ManOrgJskTsjContract
                            {
                                ManagingOrganization = jskTsjContract.ManagingOrganization,
                                TypeContractManOrgRealObj = jskTsjContract.TypeContractManOrgRealObj,
                                DocumentName = jskTsjContract.DocumentName,
                                DocumentNumber = jskTsjContract.DocumentNumber,
                                DocumentDate = jskTsjContract.DocumentDate,
                                StartDate = jskTsjContract.StartDate,
                                EndDate = jskTsjContract.EndDate,
                                PlannedEndDate = jskTsjContract.PlannedEndDate,
                                Note = jskTsjContract.Note,
                                FileInfo = this.ReCreateFile(jskTsjContract.FileInfo, fileManager),
                                TerminateReason = jskTsjContract.TerminateReason,
                                IsTransferredManagement = jskTsjContract.IsTransferredManagement,
                                ManOrgTransferredManagement = jskTsjContract.ManOrgTransferredManagement
                            };

                            repManOrgJskTsjContract.Save(newJskTsjContract);

                            var newContractRealityObject = repManOrgContractRealityObject.Get(contrRealityObjects[index].Id);
                            newContractRealityObject.ManOrgContract = newJskTsjContract;

                            repManOrgContractRealityObject.Update(newContractRealityObject);
                        }
                    }

                    tr.Commit();
                }
                catch (Exception e)
                {
                    tr.Rollback();
                    return new BaseDataResult
                    {
                        Success = false,
                        Message = "Error",
                        Data = new { exceptionMessage = e.Message }
                    };
                }
            }

            return new BaseDataResult(new { countContracts });
        }

        public IDataResult CorrectContractJskTsj(BaseParams baseParams)
        {
            var repManOrgContractRealityObject = Container.Resolve<IRepository<ManOrgContractRealityObject>>();
            var repManOrgContractJskTsj = Container.Resolve<IRepository<ManOrgContractTransfer>>();
            var fileManager = this.Container.Resolve<IFileManager>();

            var dictManagingOrgJskTsjRealityObject = repManOrgContractRealityObject.GetAll()
               .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
               .Select(x => new
               {
                   x.Id,
                   ContractId = x.ManOrgContract.Id,
                   RealityObjectId = x.RealityObject.Id
               })
               .AsEnumerable()
               .GroupBy(x => x.ContractId)
               .ToDictionary(x => x.Key, x => x.Select(y => new { y.Id, y.RealityObjectId }).ToArray());

            int countContracts = 0;

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var contractRealityObject in dictManagingOrgJskTsjRealityObject.Where(x => x.Value.Length > 1))
                    {
                        var contrRealityObjects = contractRealityObject.Value;

                        var contractTransfer = repManOrgContractJskTsj.Get(contractRealityObject.Key);

                        for (var index = 1; index < contrRealityObjects.Length; index++)
                        {
                            countContracts++;

                            var newContractTransfer = new ManOrgContractTransfer
                            {
                                ManagingOrganization = contractTransfer.ManagingOrganization,
                                TypeContractManOrgRealObj = contractTransfer.TypeContractManOrgRealObj,
                                DocumentName = contractTransfer.DocumentName,
                                DocumentNumber = contractTransfer.DocumentNumber,
                                DocumentDate = contractTransfer.DocumentDate,
                                StartDate = contractTransfer.StartDate,
                                EndDate = contractTransfer.EndDate,
                                PlannedEndDate = contractTransfer.PlannedEndDate,
                                Note = contractTransfer.Note,
                                FileInfo = ReCreateFile(contractTransfer.FileInfo, fileManager),
                                TerminateReason = contractTransfer.TerminateReason,
                                ManOrgJskTsj = contractTransfer.ManOrgJskTsj
                            };

                            repManOrgContractJskTsj.Save(newContractTransfer);

                            var newContractRealityObject = repManOrgContractRealityObject.Get(contrRealityObjects[index].Id);
                            newContractRealityObject.ManOrgContract = newContractTransfer;
                            repManOrgContractRealityObject.Update(newContractRealityObject);
                        }
                    }

                    tr.Commit();
                }
                catch (Exception e)
                {
                    tr.Rollback();
                    return new BaseDataResult
                        {
                            Success = false,
                            Message = "Error",
                            Data = new {exceptionMessage = e.Message}
                        };
                }
            }

            return new BaseDataResult(new {countContracts });
        }

        protected FileInfo ReCreateFile(FileInfo fileInfo, IFileManager fileManager)
        {
            if (fileInfo == null)
            {
                return null;
            }

            var data = new byte[0];

            try
            {
                var fileInfoStream = fileManager.GetFile(fileInfo);

                data = new byte[fileInfoStream.Length];

                fileInfoStream.Seek(0, SeekOrigin.Begin);
                fileInfoStream.Read(data, 0, data.Length);
                fileInfoStream.Seek(0, SeekOrigin.Begin);
            }
            catch (FileNotFoundException)
            {
                return null;
            }

            var newFileInfo = fileManager.SaveFile(fileInfo.Name, fileInfo.Extention, data);

            return newFileInfo;
        }

        public IDataResult CreateRelation(BaseParams baseParams)
        {
            var servManOrgJskTsjContract = Container.Resolve<IRepository<ManOrgJskTsjContract>>();
            var servManOrgContractRelation = Container.Resolve<IRepository<ManOrgContractRelation>>();
            var servManOrgContractRealityObject = Container.Resolve<IRepository<ManOrgContractRealityObject>>();

            var servManOrgBaseContract = Container.Resolve<IRepository<ManOrgBaseContract>>();

            var jskTsjContracts = servManOrgJskTsjContract.GetAll()
                .Where(x => x.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj && x.ManOrgTransferredManagement != null)
                .Select(x => new
                {
                    x.Id,
                    ManOrgTransferredManagementId = x.ManOrgTransferredManagement.Id
                })
                .ToArray();

            var dictJskTsj = servManOrgContractRealityObject.GetAll()
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj)
                .Select(x => new
                {
                    ContractId = x.ManOrgContract.Id,
                    RealityObjectId = x.RealityObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.ContractId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.RealityObjectId).First());

            var dictManagingOrgJskTsj = servManOrgContractRealityObject.GetAll()
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
                .Select(x => new
                {
                    ContractId = x.ManOrgContract.Id,
                    ManOrgId = x.ManOrgContract.ManagingOrganization.Id,
                    RealityObjectId = x.RealityObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => string.Format("{0}_{1}", x.ManOrgId, x.RealityObjectId))
                .ToDictionary(x => x.Key, x => x.Select(y => y.ContractId).First());

            int countRelations = 0;

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var jskTsjContract in jskTsjContracts)
                    {
                        if (!dictJskTsj.ContainsKey(jskTsjContract.Id))
                            continue;

                        var realityObjId = dictJskTsj[jskTsjContract.Id];
                        var key = string.Format("{0}_{1}", jskTsjContract.ManOrgTransferredManagementId, realityObjId);

                        if (!dictManagingOrgJskTsj.ContainsKey(key))
                            continue;

                        var childrenId = dictManagingOrgJskTsj[key];

                        var parentId = jskTsjContract.Id;
                        var contr = servManOrgContractRelation.GetAll().Any(x => x.Children.Id == childrenId && x.Parent.Id == parentId);
                        if (contr)
                            continue;

                        countRelations++;

                        var manOrgContractRelation = new ManOrgContractRelation
                        {
                            Parent = servManOrgBaseContract.Load(parentId),
                            Children = servManOrgBaseContract.Load(childrenId),
                            TypeRelation = TypeContractRelation.TransferTsjUk
                        };
                        servManOrgContractRelation.Save(manOrgContractRelation);
                    }

                    tr.Commit();
                }
                catch (Exception e)
                {
                    tr.Rollback();
                    return new BaseDataResult
                        {
                            Success = false,
                            Message = "Error",
                            Data = new {exceptionMessage = e.Message}
                        };
                }
            }

            return new BaseDataResult(new { countRelations });
        }

        public IDataResult CreateRelationSecond(BaseParams baseParams)
        {
            var serviceContractRobject = Container.Resolve<IRepository<ManOrgContractRealityObject>>();

            var serviceBaseContract = Container.Resolve<IRepository<ManOrgBaseContract>>();
            var serviceRelation = Container.Resolve<IRepository<ManOrgContractRelation>>();
            var serviceTransfer = Container.Resolve<IRepository<ManOrgContractTransfer>>();
            //var serviceTsjContract = Container.Resolve<IRepository<ManOrgJskTsjContract>>();

            var contractTransfer = serviceTransfer.GetAll()
                .Where(y => !serviceRelation.GetAll()
                    .Any(x => x.Children.Id == y.Id
                        && x.TypeRelation == TypeContractRelation.TransferTsjUk))
                .Select(x => new
                {
                    x.Id,
                    TsjId = x.ManOrgJskTsj.Id,
                    x.StartDate,
                    x.EndDate
                })
                .ToList();

            var contractTransferRobjects = serviceContractRobject.GetAll()
                .Where(y => !serviceRelation.GetAll()
                    .Any(x => x.Children.Id == y.ManOrgContract.Id
                        && x.TypeRelation == TypeContractRelation.TransferTsjUk))
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
                .Select(x => new
                {
                    x.ManOrgContract.Id,
                    RoId = x.RealityObject.Id
                })
                .AsEnumerable()
                .ToDictionary(x => x.Id, y => y.RoId);

            var contractTsj = serviceContractRobject.GetAll()
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj)
                .Select(x => new
                {
                    x.ManOrgContract.Id,
                    RoId = x.RealityObject.Id,
                    TsjId = x.ManOrgContract.ManagingOrganization.Id,
                    x.ManOrgContract.StartDate,
                    x.ManOrgContract.EndDate
                })
                .AsEnumerable()
                .GroupBy(x => x.TsjId)
                .ToDictionary(x => x.Key, y => y
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, z => z.Select(x => new { x.StartDate, x.EndDate, x.Id }).ToList()));

            int countRelations = 0;

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var item in contractTransfer)
                    {
                        if (!contractTsj.ContainsKey(item.TsjId))
                            continue;

                        if (!contractTransferRobjects.ContainsKey(item.Id))
                            continue;

                        if (!contractTsj[item.TsjId].ContainsKey(contractTransferRobjects[item.Id]))
                            continue;

                        var tsjContract = contractTsj[item.TsjId][contractTransferRobjects[item.Id]]
                            .FirstOrDefault(x => x.StartDate <= item.StartDate && (!x.EndDate.HasValue || x.EndDate >= item.StartDate));

                        if (tsjContract != null)
                        {
                            countRelations++;

                            var newRelation = new ManOrgContractRelation
                            {
                                Parent = serviceBaseContract.Load(tsjContract.Id),
                                Children = serviceBaseContract.Load(item.Id),
                                TypeRelation = TypeContractRelation.TransferTsjUk
                            };

                            serviceRelation.Save(newRelation);
                        }
                    }

                    tr.Commit();
                }
                catch (Exception e)
                {
                    tr.Rollback();
                    return new BaseDataResult
                    {
                        Success = false,
                        Message = "Error",
                        Data = new { exceptionMessage = e.Message }
                    };
                }
            }

            return new BaseDataResult(new { countRelations });
        }
    }
}