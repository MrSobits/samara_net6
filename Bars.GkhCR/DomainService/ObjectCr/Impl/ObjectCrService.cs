namespace Bars.GkhCr.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Ionic.Zlib;
    using Castle.Windsor;
    using Ionic.Zip;
    using Bars.GkhCr.Properties;
    using Bars.B4.Application;
    using System.Net.Mail;
    using System.Net;

    /// <inheritdoc />
    public class ObjectCrService : IObjectCrService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IRepository<State> StateDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IRepository<BuildContract> BuildContractDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IRepository<MassBuildContract> MassBuildContractDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IRepository<BuildContractTypeWork> BuildContractWorkDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IRepository<MassBuildContractWork> MassBuildContractWorkDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IRepository<MassBuildContractObjectCr> MassBuildContractObjectCrDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IRepository<MassBuildContractObjectCrWork> MassBuildContractObjectCrWorkDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<RealityObjectImage> RealityObjectImageDomain { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<Work> WorkDomain { get; set; }

        /// <summary>
        /// Домен-сервис работы в объекте КР
        /// </summary>
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        /// <summary>
        /// Домен-сервис Предложение КР
        /// </summary>
        public IDomainService<OverhaulProposal> OverhaulProposalDomain { get; set; }

        /// <summary>
        /// Домен-сервис Работы в предложении КР
        /// </summary>
        public IDomainService<OverhaulProposalWork> OverhaulProposalWorkDomain { get; set; }//Work

        /// <inheritdoc />
        public IQueryable<ViewObjectCr> GetFilteredByOperator()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();
            var contragentIds = userManager.GetContragentIds();
            List<long> contragentsToRemove;
            if (contragentIds.Count > 0)
            {
                 var manorg = this.Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                    .Where(x=> contragentIds.Contains(x.Contragent.Id));

                if (manorg.Count() == 0)
                {
                    contragentIds.Clear();
                }                

            }

            var serviceManOrgContractRobject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            return this.Container.Resolve<IDomainService<ViewObjectCr>>().GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.MunicipalityId));
                //.WhereIf(
                //    contragentIds.Count > 0,
                //    y => serviceManOrgContractRobject.GetAll()
                //        .Any(
                //            x => x.RealityObject.Id == y.RealityObjectId
                //                && contragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id)
                //                && x.ManOrgContract.StartDate <= DateTime.Now.Date
                //                && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value >= DateTime.Now.Date)));
        }

        /// <inheritdoc />
        public IDataResult RealityObjectsByProgramm(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var programId = baseParams.Params.GetAs<long>("programCrId");
            var municipalityIds = baseParams.Params.GetAs<long[]>("municipalities");

            var data = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == programId)
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(
                    x => new
                    {
                        x.RealityObject.Id,
                        x.RealityObject.Address
                    })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <inheritdoc />
        public IDataResult MassChangeState(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<long[]>("ids");
            var newStateId = baseParams.Params.GetAs<long>("newStateId");
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                var stateProvider = this.Container.Resolve<IStateProvider>();
                var serviceState = this.Container.Resolve<IDomainService<State>>();

                try
                {
                    var newState = serviceState.Load(newStateId);

                    foreach (var id in ids)
                    {
                        stateProvider.ChangeState(id, "cr_object", newState, "Массовый перевод статуса", true);
                    }

                    tr.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(stateProvider);
                    this.Container.Release(serviceState);
                }
            }
        }

        /// <inheritdoc />
        public IDataResult CreateContract(long objectId, long[] ids)
        {
            var competitionLotRepo = this.Container.ResolveRepository<CompetitionLotBid>();
            var competitionLotWorkRepo = this.Container.ResolveRepository<CompetitionLotTypeWork>();
            var builderDomain = this.Container.ResolveDomain<Builder>();
            var stateProvider = this.Container.Resolve<IStateProvider>();

            var contractsToSave = new List<BuildContract>();
            var contractWorksToSave = new List<BuildContractTypeWork>();
            var resultMessage = string.Empty;
            try
            {
                var lotsDict = competitionLotRepo.GetAll()
                    .Where(x => ids.Contains(x.Lot.Id))
                    .Where(x => x.IsWinner)
                    .Where(x => x.Lot.ContractNumber != null && x.Lot.ContractDate.HasValue)
                    .Select
                    (
                        x =>
                            new
                            {
                                LotId = x.Lot.Id,
                                BuilderId = x.Builder.Id,
                                x.Lot.ContractNumber,
                                x.Lot.ContractDate,
                                ContractFileId = x.Lot.ContractFile != null ? x.Lot.ContractFile.Id : (long?) null
                            })
                    .ToDictionary(x => x.LotId, x => x);

                var typeWorks = competitionLotWorkRepo.GetAll()
                    .Where(x => ids.Contains(x.Lot.Id))
                    .Where(x => x.TypeWork.ObjectCr.Id == objectId)
                    .GroupBy(x => x.Lot.Id)
                    .ToArray();

                var objectCr = this.ObjectCrDomain.Load(objectId);

                var existingContracts = this.BuildContractDomain.GetAll()
                    .Where(x => x.ObjectCr.Id == objectId)
                    .Select(
                        x => new
                        {
                            x.DocumentNum,
                            x.DocumentDateFrom,
                            BuilderId = x.Builder.Id
                        }).ToArray();

                // нет конкурсов с победителем
                if (typeWorks.Length == 0)
                {
                    resultMessage +=
                        "У выбранных записей отсутствуют сведения по договорам или отсутствуют победители. Формирование договора подряда не возможно.\n";
                }

                foreach (var lotTypeWork in typeWorks)
                {
                    var lot = lotsDict.ContainsKey(lotTypeWork.Key) ? lotsDict[lotTypeWork.Key] : null;
                    if (lot == null)
                    {
                        resultMessage +=
                            "У записи по лотам {0} отсутствуют сведения по договорам или отсутствуют победители. Формирование договора подряда не возможно.\n"
                                .FormatUsing(string.Join(",", lotTypeWork.Select(x => x.Lot).Distinct().Select(x => x.ContractNumber).ToArray()));
                        continue;
                    }

                    // если есть существующий договор подряда с теми же данными
                    if (
                        existingContracts.Any(
                            x => x.DocumentNum == lot.ContractNumber && x.DocumentDateFrom == lot.ContractDate && x.BuilderId == lot.BuilderId))
                    {
                        resultMessage +=
                            "По записи {0} и договору {1} договор подряда сформирован в разделе \"Договоры подряда\". Повторное формирование договора не возможно.\n"
                                .FormatUsing(
                                    objectCr.RealityObject.Address,
                                    lot.ContractNumber);
                        continue;
                    }

                    var contract = new BuildContract
                    {
                        Builder = builderDomain.Load(lot.BuilderId),
                        DocumentNum = lot.ContractNumber,
                        DocumentDateFrom = lot.ContractDate,
                        DocumentFile = lot.ContractFileId.HasValue
                            ? new FileInfo
                            {
                                Id = lot.ContractFileId.Value
                            }
                            : null,
                        ObjectCr = objectCr
                    };
                    stateProvider.SetDefaultState(contract);
                    contractsToSave.Add(contract);

                    foreach (var competitionLotTypeWork in lotTypeWork)
                    {
                        var contractWork = new BuildContractTypeWork
                        {
                            BuildContract = contract,
                            Sum = competitionLotTypeWork.TypeWork.Sum,
                            TypeWork = competitionLotTypeWork.TypeWork
                        };

                        contractWorksToSave.Add(contractWork);
                    }
                }
            }
            finally
            {
                this.Container.Release(competitionLotRepo);
                this.Container.Release(competitionLotWorkRepo);
                this.Container.Release(builderDomain);
                this.Container.Release(stateProvider);
            }

            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var contract in contractsToSave)
                    {
                        this.BuildContractDomain.Save(contract);
                    }

                    foreach (var work in contractWorksToSave)
                    {
                        this.BuildContractWorkDomain.Save(work);
                    }

                    tr.Commit();
                }
                catch (Exception exception)
                {
                    resultMessage += "Произошла ошибка сохранения контрактов. " + exception.Message + "\n";
                    tr.Rollback();
                }
            }

            if (string.IsNullOrWhiteSpace(resultMessage))
            {
                return new BaseDataResult();
            }

            return new BaseDataResult(false, resultMessage);
        }

        /// <inheritdoc />
        public IDataResult GetTypeStates(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var ids = baseParams.Params.GetAs("Id", string.Empty).ToLongArray();
            var typeId = baseParams.Params.GetAs("typeId", string.Empty);

            var data = this.Container.Resolve<IDomainService<State>>().GetAll()
                .WhereIf(ids.Length > 0, x => ids.Contains(x.Id))
                .WhereIf(!string.IsNullOrEmpty(typeId), x => x.TypeId == typeId)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Name
                    })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        /// <inheritdoc />
        public IDataResult GetTypeChangeProgramCr(BaseParams baseParams)
        {
            /*
             * Вообщем это ввобще мега неудачное решение чтобы фиксировать из кучи мест сохранение изменений по программе а потмо смотреть почему у грида на клиенте непоказываются колонки
             */
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId");

            var service = this.Container.Resolve<IDomainService<ProgramCrChangeJournal>>();

            using (this.Container.Using(service))
            {
                var objectCr = this.ObjectCrDomain.Load(objectCrId);

                if (objectCr != null)
                {
                    if (service.GetAll().Any(x => x.ProgramCr.Id == objectCr.ProgramCr.Id && x.TypeChange == TypeChangeProgramCr.FromDpkr))
                    {
                        return new BaseDataResult(TypeChangeProgramCr.FromDpkr);
                    }
                }

                return new BaseDataResult(TypeChangeProgramCr.Manually);
            }
        }

        /// <inheritdoc />
        public IDataResult UseAddWorkFromLongProgram(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId");

            var service = this.Container.Resolve<IDomainService<ProgramCr>>();

            var result = false;

            using (this.Container.Using(service))
            {
                var objectCr = this.ObjectCrDomain.Get(objectCrId);

                if (objectCr != null)
                {
                    result = service.GetAll().
                        Any(x => x.Id == objectCr.ProgramCr.Id && x.AddWorkFromLongProgram == AddWorkFromLongProgram.Use);
                }

                return new BaseDataResult(result);
            }
        }

        /// <inheritdoc />
        public IDataResult GetBuilders(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var data = this.BuildContractDomain.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)
                .Select(x => x.Builder)
                .Select(
                    x => new
                    {
                        x.Id,
                        Municipality = x.Contragent.Municipality.Name,
                        ContragentName = x.Contragent.Name,
                        ContragentId = x.Contragent.Id,
                        x.Contragent.Inn,
                        x.Contragent.Kpp,
                        x.AdvancedTechnologies,
                        x.ConsentInfo,
                        x.WorkWithoutContractor,
                        x.Rating,
                        x.ActivityGroundsTermination,
                        x.File,
                        x.FileLearningPlan,
                        x.FileManningShedulle,
                        ContragentPhone = x.Contragent.Phone
                    })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <inheritdoc />
        public IDataResult Recover(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("selected").ToLongArray();

            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                var objectCrDomain = this.Container.ResolveDomain<ObjectCr>();

                try
                {
                    var objectCrList = this.Container.ResolveDomain<ObjectCr>().GetAll()
                        .Where(x => ids.Contains(x.Id))
                        .ToList();

                    foreach (var objectCr in objectCrList)
                    {
                        objectCr.ProgramCr = objectCr.BeforeDeleteProgramCr;
                        objectCr.BeforeDeleteProgramCr = null;

                        objectCrDomain.Update(objectCr);
                    }

                    tr.Commit();
                    return new BaseDataResult();
                }
                catch (Exception e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                finally
                {
                    this.Container.Release(objectCrDomain);
                }
            }
        }

        /// <inheritdoc />
        public IDataResult GetAdditionalParams(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var domain = this.Container.ResolveDomain<AdditionalParameters>();

            AdditionalParameters rec;

            try
            {
                rec = domain.GetAll().FirstOrDefault(x => x.ObjectCr.Id == objectCrId);
            }
            catch (Exception exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
            finally
            {
                this.Container.Release(domain);
            }

            return new BaseDataResult(rec);
        }
        
        public IDataResult GetListCrObjectWorksByObjectId(BaseParams baseParams, bool isPaging, out int totalCount)
        {
            var loadParams = baseParams.GetLoadParam();
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var domain = this.Container.ResolveDomain<TypeWorkCr>();

            var data = domain.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)
                .Select(x=> new
                {
                    x.Id,
                    x.Work.Name,
                    x.YearRepair,
                    x.Sum,
                    x.Work.TypeWork,
                    x.Volume,
                    UnitMeasure = x.Work.UnitMeasure.Name
                }).Filter(loadParams, Container);
            totalCount = data.Count();

            if (isPaging)
            {
                return new BaseDataResult(data.Order(loadParams).Paging(loadParams).ToList());
            }

            return new BaseDataResult(data.Order(loadParams).ToList());
        }

        public IDataResult GetListDistinctWorksByProgramId(BaseParams baseParams, bool isPaging, out int totalCount)
        {
            var loadParams = baseParams.GetLoadParam();
            var programCrId = baseParams.Params.GetAsId("programCrId");

            if (programCrId > 0)
            {
                var objectCrList = ObjectCrDomain.GetAll()
                    .Where(x => x.ProgramCr.Id == programCrId)
                    .Select(x => x.Id).ToList();
                var workIds = TypeWorkCrDomain.GetAll()
                    .Where(x => x.Work != null && x.ObjectCr != null && objectCrList.Contains(x.ObjectCr.Id))
                    .Select(x => x.Work.Id).Distinct().ToList();
                var domain = this.Container.ResolveDomain<Work>();

                var data = domain.GetAll()
                    .Where(x => workIds.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.TypeWork,
                        UnitMeasure = x.UnitMeasure.Name
                    }).Filter(loadParams, Container);
                totalCount = data.Count();

                if (isPaging)
                {
                    return new BaseDataResult(data.Order(loadParams).Paging(loadParams).ToList());
                }

                return new BaseDataResult(data.Order(loadParams).ToList());
            }
            else
            {
                totalCount = 0;
                return null;
            }

         
        }

        public IDataResult GetListObjectCRByMassBuilderId(BaseParams baseParams, bool isPaging, out int totalCount)
        {
            var loadParams = baseParams.GetLoadParam();
            var buildContractId = baseParams.Params.GetAsId("buildContractId");

            if (buildContractId > 0)
            {
                var massBuilder = MassBuildContractDomain.Get(buildContractId);

                var workIds = MassBuildContractWorkDomain.GetAll()
                    .Where(x => x.MassBuildContract == massBuilder)
                    .Select(x => x.Work.Id).Distinct().ToList();
                //оптимизировали получение списка объектов
                var data = TypeWorkCrDomain.GetAll()
                    .Where(x=> x.ObjectCr!= null && x.ObjectCr.ProgramCr.Id == massBuilder.ProgramCr.Id)
                    .Where(x => workIds.Contains(x.Work.Id))
                    .Select(x=> new
                    {
                        x.ObjectCr.Id,
                        Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                        x.ObjectCr.RealityObject.Address
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.Municipality,
                        x.Address
                    }).Distinct(x=> x.Id).AsQueryable()
                    .Filter(loadParams, Container);
                totalCount = data.Count();

                if (isPaging)
                {
                    return new BaseDataResult(data.Order(loadParams).Paging(loadParams).ToList());
                }

                return new BaseDataResult(data.Order(loadParams).ToList());
            }
            else
            {
                totalCount = 0;
                return null;
            }


        }

        public IDataResult AddOverhaulProposalWork(BaseParams baseParams)
        {
            var parentId = baseParams.Params.ContainsKey("parentId") ? baseParams.Params["parentId"].ToLong() : 0;
            var objectCrWorkIds = baseParams.Params.ContainsKey("objectCrWorkIds") ? baseParams.Params["objectCrWorkIds"].ToString() : "";
            var listIds = new List<long>();

            var overhaulProposal = OverhaulProposalDomain.Get(parentId);

            if (parentId == null)
            {
                return new BaseDataResult(false, "Не удалось определить Предложение по КР по Id " + parentId.ToStr());
            }

            


            var gjiArtLawIds = objectCrWorkIds.Split(',').Select(id => id.ToLong()).ToList();
                      

            foreach (var newId in gjiArtLawIds)
            {
                var typeWorkCr = TypeWorkCrDomain.Get(newId);
                // Если такого решения еще нет то добалвяем
                var newObj = new OverhaulProposalWork();
                newObj.Work = typeWorkCr.Work;
                newObj.OverhaulProposal = overhaulProposal;
                newObj.ObjectVersion = 1;
                newObj.ObjectCreateDate = DateTime.Now;
                newObj.ObjectEditDate = DateTime.Now;
                if (typeWorkCr.YearRepair.HasValue)
                {
                    newObj.DateStartWork = new DateTime(typeWorkCr.YearRepair.Value, 1, 1);
                }
                newObj.Sum = typeWorkCr.Sum;
                newObj.Volume = typeWorkCr.Volume;
                if (overhaulProposal.ObjectCr.RealityObject != null && typeWorkCr.Volume == overhaulProposal.ObjectCr.RealityObject.AreaMkd)
                {
                    newObj.ByAreaMkd = true;
                }
                OverhaulProposalWorkDomain.Save(newObj);
            }

            return new BaseDataResult();
        }

        public IDataResult ChangeBuildContractStateFromMassBuild(BaseParams baseParams)
        {
            var contractIds = baseParams.Params.ContainsKey("contractIds") ? baseParams.Params["contractIds"].ToString() : "";
            var newStateId = baseParams.Params.ContainsKey("newStateId") ? baseParams.Params["newStateId"].ToLong() : 0;
            var bcIds = contractIds.Split(',').Select(id => id.ToLong()).ToList();

            if (newStateId != 0)
            {
                var buildContracts = BuildContractDomain.GetAll()
                    .Where(x => bcIds.Contains(x.Id))
                    .Select(x => x)
                    .ToList();

                var findState = StateDomain.GetAll()
                    .Where(x => x.Id == newStateId)
                    .Select(x => x)
                    .ToList();

                foreach (var bc in buildContracts)
                {
                    bc.State = findState[0];
                    BuildContractDomain.Update(bc);
                }
            }

            return new BaseDataResult();
        }

        public IDataResult AddMassBuilderContractWork(BaseParams baseParams)
        {
            var parentId = baseParams.Params.ContainsKey("parentId") ? baseParams.Params["parentId"].ToLong() : 0;
            var objectCrWorkIds = baseParams.Params.ContainsKey("objectCrWorkIds") ? baseParams.Params["objectCrWorkIds"].ToString() : "";
            var listIds = new List<long>();

            var massBK = MassBuildContractDomain.Get(parentId);

            var existsWorkIds = MassBuildContractWorkDomain.GetAll()
                .Where(x => x.MassBuildContract == massBK)
                .Select(x => x.Work.Id).ToList();

            if (parentId == null)
            {
                return new BaseDataResult(false, "Не удалось определить массовый договор по Id " + parentId.ToStr());
            }


            var workIds = objectCrWorkIds.Split(',').Select(id => id.ToLong()).ToList();


            foreach (var newId in workIds)
            {
                var work = WorkDomain.Get(newId);
                // Если такого решения еще нет то добалвяем
                if (!existsWorkIds.Contains(newId))
                {
                    var newObj = new MassBuildContractWork();
                    newObj.Work = work;
                    newObj.MassBuildContract = massBK;
                    newObj.ObjectVersion = 1;
                    newObj.ObjectCreateDate = DateTime.Now;
                    newObj.ObjectEditDate = DateTime.Now;
                    newObj.Sum = 0;
                    MassBuildContractWorkDomain.Save(newObj);
                }
               
            }

            return new BaseDataResult();
        }

        public IDataResult AddMassBuilderContractCRWork(BaseParams baseParams)
        {
            var parentId = baseParams.Params.ContainsKey("parentId") ? baseParams.Params["parentId"].ToLong() : 0;
            var objectCrWorkIds = baseParams.Params.ContainsKey("objectCrWorkIds") ? baseParams.Params["objectCrWorkIds"].ToString() : "";
            var listIds = new List<long>();

            var massBKObjectCR = MassBuildContractObjectCrDomain.Get(parentId);

            var existsWorkIds = MassBuildContractObjectCrWorkDomain.GetAll()
                .Where(x => x.MassBuildContractObjectCr == massBKObjectCR)
                .Select(x => x.Work.Id).ToList();

            if (parentId == null)
            {
                return new BaseDataResult(false, "Не удалось определить объект КР по Id " + parentId.ToStr());
            }


            var workIds = objectCrWorkIds.Split(',').Select(id => id.ToLong()).ToList();


            foreach (var newId in workIds)
            {
                var work = WorkDomain.Get(newId);
                // Если такого решения еще нет то добалвяем
                if (!existsWorkIds.Contains(newId))
                {
                    var newObj = new MassBuildContractObjectCrWork();
                    newObj.Work = work;
                    newObj.MassBuildContractObjectCr = massBKObjectCR;
                    newObj.ObjectVersion = 1;
                    newObj.ObjectCreateDate = DateTime.Now;
                    newObj.ObjectEditDate = DateTime.Now;
                    newObj.Sum = 0;
                    MassBuildContractObjectCrWorkDomain.Save(newObj);
                }

            }

            return new BaseDataResult();
        }

        public IDataResult AddMassBuilderContractObjectCr(BaseParams baseParams)
        {
            var parentId = baseParams.Params.ContainsKey("parentId") ? baseParams.Params["parentId"].ToLong() : 0;
            var objectCrIds = baseParams.Params.ContainsKey("objectCrIds") ? baseParams.Params["objectCrIds"].ToString() : "";
            var listIds = new List<long>();

            var massBK = MassBuildContractDomain.Get(parentId);

            var existsObjectIds = MassBuildContractObjectCrDomain.GetAll()
                .Where(x => x.MassBuildContract == massBK)
                .Select(x => x.ObjectCr.Id).ToList();

            if (parentId == null)
            {
                return new BaseDataResult(false, "Не удалось определить массовый договор по Id " + parentId.ToStr());
            }


            var objIds = objectCrIds.Split(',').Select(id => id.ToLong()).ToList();

            var workInBCList = MassBuildContractWorkDomain.GetAll()
                .Where(x => x.MassBuildContract == massBK)
                .Select(x => x.Work.Id).ToList();


            foreach (var newId in objIds)
            {
                var objectCr = ObjectCrDomain.Get(newId);
                // Если такого решения еще нет то добалвяем
                if (!existsObjectIds.Contains(newId))
                {
                    var newObj = new MassBuildContractObjectCr();
                    newObj.ObjectCr = objectCr;
                    newObj.MassBuildContract = massBK;
                    newObj.ObjectVersion = 1;
                    newObj.ObjectCreateDate = DateTime.Now;
                    newObj.ObjectEditDate = DateTime.Now;
                    newObj.Sum = 0;
                    MassBuildContractObjectCrDomain.Save(newObj);
                    //добавляем работы
                    var workInObjectCRList = TypeWorkCrDomain.GetAll()
                        .Where(x => x.ObjectCr == objectCr)
                        .Where(x => workInBCList.Contains(x.Work.Id))
                        .ToList();
                    decimal worksSum = 0;
                    foreach (TypeWorkCr tw in workInObjectCRList)
                    {
                        var newCRWork = new MassBuildContractObjectCrWork
                        {
                            MassBuildContractObjectCr = newObj,
                            Work = tw.Work,
                            Sum = tw.Sum,
                            ObjectVersion = 1,
                            ObjectCreateDate = DateTime.Now,
                            ObjectEditDate = DateTime.Now
                        };
                        if (newCRWork.Sum.HasValue)
                        {
                            worksSum += newCRWork.Sum.Value;
                        }
                        MassBuildContractObjectCrWorkDomain.Save(newCRWork);
                    }
                    newObj.Sum = worksSum;
                    MassBuildContractObjectCrDomain.Update(newObj);
                }

            }

            return new BaseDataResult();
        }

        public IDataResult GetBuildContractsForMassBuild(BaseParams baseParams, bool isPaging, out int totalCount)
        {
            var buildContractId = baseParams.Params.ContainsKey("buildContractId") ? baseParams.Params["buildContractId"].ToLong() : 0;
            var loadParams = baseParams.GetLoadParam();
            var massBC = MassBuildContractDomain.Get(buildContractId);

            var existsObjectIds = MassBuildContractObjectCrDomain.GetAll()
                .Where(x => x.MassBuildContract.Id == buildContractId)
                .Select(x => x.ObjectCr.Id).ToList();

            var data = BuildContractDomain.GetAll()
                .Where(x => existsObjectIds.Contains(x.ObjectCr.Id) && (x.DocumentNum == massBC.DocumentNum) && (x.DocumentDateFrom == massBC.DocumentDateFrom))
                .Select(x => new 
                {
                    x.Id,
                    x.State,
                    ObjectCr = x.ObjectCr.RealityObject.Address,
                    Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                    x.DocumentNum,
                    x.DocumentDateFrom,
                    x.Sum
                })
                .Filter(loadParams, Container);

            totalCount = data.Count();

            if (isPaging)
            {
                return new BaseDataResult(data.Order(loadParams).Paging(loadParams).ToList());
            }

            return new BaseDataResult(data.Order(loadParams).ToList());
        }

        public IDataResult SendFKRToGZHIMail(BaseParams baseParams, Int64 taskId)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var realityObject = RealityObjectDomain.Get(taskId);
            if (realityObject == null)
            {
                return new BaseDataResult(false, "Не удалось определить МКД по ID " + taskId);
            }
            if (!realityObject.Iscluttered)
            {
                return new BaseDataResult(false, "У данного МКД нет признака МОП захламлены");
            }
            var files = RealityObjectImageDomain.GetAll()
                .Where(x=> x.RealityObject == realityObject && x.ImagesGroup == Gkh.Enums.ImagesGroup.MOPWasted && x.ObjectCreateDate > DateTime.Now.AddMonths(-5))
                .Select(x=> x.File).ToList();
            if (files.Count == 0)
            {
                return new BaseDataResult(false, "У данного МКД нет подтвержданющих захламление МОП материалов");
            }

            var tempDir = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"_{taskId}"));
            var archive = new ZipFile(Encoding.UTF8)
            {
                CompressionLevel = CompressionLevel.Level9,
                AlternateEncoding = Encoding.GetEncoding("cp866"),
                AlternateEncodingUsage = ZipOption.AsNecessary
            };
            foreach (var file in files)
            {
                System.IO.File.WriteAllBytes(
                     System.IO.Path.Combine(tempDir.FullName, $"{file.Name}.{file.Extention}"),
                    fileManager.GetFile(file).ReadAllBytes());
            }

            archive.AddDirectory(tempDir.FullName);
            FileInfo zipfile = null;
            using (var ms = new System.IO.MemoryStream())
            {
                archive.Save(ms);
                zipfile = fileManager.SaveFile(ms, $"{taskId}.zip");
            }
            System.IO.Directory.Delete(tempDir.FullName, true);            
            
            // TODO: переделать на формирование отчета в сервисе
            /*StiReport report = new StiReport();           
            report.Load(Resources.FKRToGZHIReport);
            report.Dictionary.Variables["address"].Value = $"{realityObject.Municipality.Name}. {realityObject.Address}";
            report["address"] = $"{realityObject.Municipality.Name}. {realityObject.Address}";
            report.Render();

            SendToGZHI(report, zipfile, realityObject);*/
            return new BaseDataResult();
        }

        /*private void SendToGZHI(StiReport repo, FileInfo archive, RealityObject entity)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var smtpClient = appSettings.GetAs<string>("smtpClient");
            var toEmail = appSettings.GetAs<string>("fkrToGZHIMail");
            var smtpPort = appSettings.GetAs<int>("smtpPort");
            var mailFrom = appSettings.GetAs<string>("smtpEmail");
            var smtpLogin = appSettings.GetAs<string>("smtpLogin");
            var smtpPassword = appSettings.GetAs<string>("smtpPassword");
            var enableSsl = appSettings.GetAs<bool>("enableSsl");

            try
            {
                var stream = new System.IO.MemoryStream();
                repo.ExportDocument(StiExportFormat.Pdf, stream);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                MailAddress from = new MailAddress(mailFrom, "Фонд капитального ремонта");
                MailAddress to = new MailAddress(toEmail);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Сведения о захламлении МОП";
                m.Body =
                    $"<h3>Фонд капитального ремонта сообщает, что обнаружено захламление мест общего пользования по адресу {entity.Address}. Обращение и подтверждающие материалы по вложениях.</h3>";
                m.IsBodyHtml = true;
                m.Attachments.Add(new Attachment(stream, $"Обращение.pdf"));
                m.Attachments.Add(new Attachment(fileManager.GetFile(archive), archive.FullName));
                SmtpClient smtp = new SmtpClient(smtpClient, smtpPort);
                smtp.Credentials = new NetworkCredential(smtpLogin, smtpPassword);
                smtp.EnableSsl = enableSsl;
                smtp.Send(m);
            }
            catch (Exception e)
            {
            }
    }*/
                
        /// <inheritdoc />
        public IDataResult ListWithoutFilter(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var programId = baseParams.Params.GetAs("programId", string.Empty);
            var municipalityId = baseParams.Params.GetAs("municipalityId", string.Empty);
            var deleted = baseParams.Params.GetAs("deleted", false);
            var ids = baseParams.Params.GetAs("Id", string.Empty).ToLongArray();
            var programIds = !string.IsNullOrEmpty(programId) ? programId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var municipalityIds = !string.IsNullOrEmpty(municipalityId) ? municipalityId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var realityObjectId = baseParams.Params.GetAs("realityObjectId", 0L);

            var stateId = baseParams.Params.GetAs<long>("stateId", 0);

            var serviceProgramCr = this.Container.Resolve<IDomainService<ProgramCr>>();
            var viewObjectCrDomain = this.Container.Resolve<IDomainService<ViewObjectCr>>();

            try
            {
                var data = viewObjectCrDomain.GetAll()
               .WhereIf(stateId > 0, x => x.State.Id == stateId)
               .WhereIf(realityObjectId > 0, x => x.RealityObjectId == realityObjectId)
               .WhereIf(deleted, x => !x.ProgramCrId.HasValue && x.BeforeDeleteProgramCrId.HasValue)
               .WhereIf(!deleted, x => x.ProgramCrId.HasValue && !x.BeforeDeleteProgramCrId.HasValue )
               .WhereIf(programIds.Length > 0 && deleted, x => programIds.Contains(x.BeforeDeleteProgramCrId.Value))
               .WhereIf(programIds.Length > 0 && !deleted, x => programIds.Contains(x.ProgramCrId.Value))
               .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.MunicipalityId) || municipalityIds.Contains(x.SettlementId))
               .WhereIf(ids.Length > 0, x => ids.Contains(x.Id))
               .WhereIf(deleted, y => serviceProgramCr.GetAll().Any(x =>
                       x.Id == y.BeforeDeleteProgramCrId
                       && x.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full))
               .WhereIf(!deleted, y => serviceProgramCr.GetAll().Any(x =>
                       x.Id == y.ProgramCrId
                       && x.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full))
               .Select(x => new
               {
                   x.Id,
                   x.ProgramNum,
                   x.DateAcceptCrGji,
                   RealityObjName = x.Address,
                   x.Municipality,
                   x.Settlement,
                   x.State,
                   ProgramCrName = deleted ? x.BeforeDeleteProgramCrName : x.ProgramCrName,
                   AllowReneg = x.DateAcceptCrGji,
                   MonitoringSmrState = x.SmrState,
                   x.MonitoringSmrId,
                   RealityObject = x.RealityObjectId,
                   x.PeriodName,
                   x.MethodFormFund
               })
               .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
               .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObjName)
               .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                data = loadParams.Order.Length == 0 ? data.Paging(loadParams) : data.Order(loadParams).Paging(loadParams);

                return new ListDataResult(data.ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(serviceProgramCr);
                this.Container.Release(viewObjectCrDomain);
            }
        }

        /// <inheritdoc />
        public IDataResult ListByFormatDataExport(BaseParams baseParams)
        {
            var repository = this.Container.ResolveRepository<ObjectCr>();

            using (this.Container.Using(repository))
            {
                return repository.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        Municipality = x.RealityObject.Municipality.Name
                    })
                    .ToListDataResult(baseParams.GetLoadParam());
            }
        }
    }
}