namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Habarovsk.Controllers;
    using Bars.GkhGji.Regions.Habarovsk.Entities;
    using Bars.GkhGji.Regions.Habarovsk.Enums;
    using Bars.GkhGji.Regions.Habarovsk.Tasks.EGRIPSendInformationRequest;
    using Bars.GkhGji.Regions.Habarovsk.Tasks.EGRULSendInformationRequest;
    using Bars.GkhGji.Regions.Habarovsk.Tasks.GetSMEVAnswers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с обращением за лицензией
    /// </summary>
    public class ManOrgLicenseRequestService : IManOrgLicenseRequestService
    {
        private SMEVEGRULExecuteController _SMEVEGRULExecuteController;

        public IWindsorContainer Container { get; set; }

        public IDomainService<Person> PersonDomain { get; set; }

        public IDomainService<SMEVEGRUL> SMEVEGRULDomain { get; set; }

        public IDomainService<SMEVEGRIP> SMEVEGRIPDomain { get; set; }

        public IDomainService<ManOrgRequestSMEV> ManOrgRequestSMEVDomain { get; set; }

        public IDomainService<ManOrgLicenseRequest> RequestDomain { get; set; }

        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }

        public IDomainService<ManOrgLicenseDoc> ManOrgLicenseDocDomain { get; set; }

        public IDomainService<ManOrgRequestPerson> RequestPersonDomain { get; set; }

        public IDomainService<ManOrgRequestProvDoc> RequestProvDocDomain { get; set; }

        public IDomainService<PersonPlaceWork> placeWorkDomain { get; set; }

        public IDomainService<PersonQualificationCertificate> qualDomain { get; set; }

        public IDomainService<PersonDisqualificationInfo> disqualDomain { get; set; }

        public IGkhUserManager UserManager { get; set; }
       
        ///<inheritdoc/>
        public IList GetList(BaseParams baseParams, bool isPaging, out int totalCount)
        {
            var loadParams = baseParams.GetLoadParam();

            var licenseId = baseParams.Params.GetAsId("licenseId");

            var requestPersons = this.RequestPersonDomain.GetAll()
                .GroupBy(x => x.LicRequest.Id)
                .ToDictionary(x => x.Key, y => y.Count());

            var contragentList = this.UserManager.GetContragentIds();

            var data = this.RequestDomain.GetAll()
                .WhereIf(licenseId > 0, x =>  x.ManOrgLicense.Id == licenseId 
                    || this.ManOrgLicenseDomain.GetAll().Where(y => y.Id == licenseId).Any(y => y.Request == x))
                .WhereIf(contragentList.Any(), x => contragentList.Contains(x.Contragent.Id))
                .Select(
                    x => new
                    {
                        x.Id,
                        x.State,
                        x.ObjectCreateDate,
                        x.RegisterNum,
                        x.DateRequest,
                        Contragent = x.Contragent.Name ?? string.Empty,
                        ContragentMunicipality = x.Contragent.Municipality.Name ?? string.Empty,
                        x.Type
                    })
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.State,
                        x.RegisterNum,
                        x.DateRequest,
                        x.ObjectCreateDate,
                        x.Contragent,
                        x.ContragentMunicipality,
                        OfficialsCount = requestPersons.Get(x.Id),
                        x.Type
                    })
                .AsQueryable()
                .Filter(loadParams, this.Container);

            totalCount = data.Count();

            if (isPaging)
            {
                return data.Order(loadParams).Paging(loadParams).ToList();
            }

            return data.Order(loadParams).ToList();
        }

        ///<inheritdoc/>
        public IDataResult AddPersons(BaseParams baseParams)
        {
            var requestId = baseParams.Params.GetAs("requestId", 0L);
            var personIds = baseParams.Params.GetAs("personIds", new long[0]);

            var request = this.RequestDomain.GetAll().FirstOrDefault(x => x.Id == requestId);

            if (request == null)
            {
                return new BaseDataResult(false, "Не удалось определить заявку на лицензию по Id " + requestId.ToStr());
            }

            var personToSave = new List<ManOrgRequestPerson>();

            var currentIds = this.RequestPersonDomain.GetAll()
                .Where(x => x.LicRequest.Id == requestId)
                .Select(x => x.Person.Id)
                .Distinct()
                .ToList();

            foreach (var id in personIds)
            {
                if (currentIds.Contains(id))
                {
                    continue;
                }

                personToSave.Add(
                    new ManOrgRequestPerson
                    {
                        Person = new Person {Id = id},
                        LicRequest = request
                    });
            }

            if (personToSave.Any())
            {
                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        personToSave.ForEach(x => this.RequestPersonDomain.Save(x));

                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        return new BaseDataResult(false, exc.Message);
                    }
                }
            }

            return new BaseDataResult();
        }

        ///<inheritdoc/>
        public IDataResult AddProvDocs(BaseParams baseParams)
        {
            var requestId = baseParams.Params.GetAs("requestId", 0L);
            var provdocIds = baseParams.Params.GetAs("provdocIds", new long[0]);

            var request = this.RequestDomain.GetAll().FirstOrDefault(x => x.Id == requestId);

            if (request == null)
            {
                return new BaseDataResult(false, "Не удалось определить заявку на лицензию по Id " + request.ToStr());
            }

            var provDocToSave = new List<ManOrgRequestProvDoc>();

            var currentIds = this.RequestProvDocDomain.GetAll()
                .Where(x => x.LicRequest.Id == requestId)
                .Select(x => x.LicProvidedDoc.Id)
                .Distinct()
                .ToList();

            foreach (var id in provdocIds)
            {
                if (currentIds.Contains(id))
                {
                    continue;
                }

                provDocToSave.Add(
                    new ManOrgRequestProvDoc
                    {
                        LicProvidedDoc = new LicenseProvidedDoc {Id = id},
                        LicRequest = request
                    });
            }

            if (provDocToSave.Any())
            {
                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        provDocToSave.ForEach(x => this.RequestProvDocDomain.Save(x));

                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        return new BaseDataResult(false, exc.Message);
                    }
                }
            }

            return new BaseDataResult();
        }

        ///<inheritdoc/>
        public IDataResult ListManOrg(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var manOrgDomain = this.Container.ResolveDomain<ManagingOrganization>();

            var data = manOrgDomain.GetAll()
                .Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                .Where(x => x.TypeManagement == TypeManagementManOrg.UK)
                .Select(
                    x => new
                    {
                        x.Contragent.Id,
                        x.Contragent.Name,
                        Municipality = x.Contragent.Municipality.Name,
                        x.Contragent.JuridicalAddress,
                        x.Contragent.Inn,
                        x.Contragent.ShortName
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.JuridicalAddress)
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        ///<inheritdoc/>
        public IDataResult ListManOrgForRequestType(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var requestType = baseParams.Params.GetAs<LicenseRequestType>("requestType");

            var manOrgDomain = this.Container.ResolveDomain<ManagingOrganization>();
            var manOrgLicenseDomain = this.Container.ResolveDomain<ManOrgLicense>();

            using (this.Container.Using(manOrgDomain, manOrgLicenseDomain))
            {
                var baseLicenseQuery = manOrgLicenseDomain.GetAll()
                    .Where(x => !x.DateTermination.HasValue);

                IQueryable<ManOrgLicense> licenseQuery = null;

                switch (requestType)
                {
                    case LicenseRequestType.GrantingLicense:
                        // если тип - предоставление лицензии, то не фильтруем
                        break;
                    case LicenseRequestType.RenewalLicense:
                        licenseQuery = baseLicenseQuery.Where(x => x.State.StartState || x.State.FinalState);
                        break;

                    case LicenseRequestType.IssuingDuplicateLicense:
                    case LicenseRequestType.TerminationActivities:
                    case LicenseRequestType.ProvisionCopiesLicense:
                        licenseQuery = baseLicenseQuery.Where(x => !x.State.StartState && !x.State.FinalState);
                        break;
                }

                var data = manOrgDomain.GetAll()
                    .Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                    .Where(x => x.TypeManagement == TypeManagementManOrg.UK)
                    .WhereIf(licenseQuery != null, x => licenseQuery.Any(y => y.Contragent == x.Contragent))
                    .Select(
                        x => new
                        {
                            x.Contragent.Id,
                            x.Contragent.Name,
                            Municipality = x.Contragent.Municipality.Name,
                            x.Contragent.JuridicalAddress,
                            x.Contragent.Inn,
                            x.Contragent.ShortName
                        })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.JuridicalAddress)
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
        }

        /// <inheritdoc />
        public IList<ManOrgLicense> ListLicenseByManOrg(long contragentId)
        {
            return this.ManOrgLicenseDomain.GetAll()
                .Where(x => x.Contragent.Id == contragentId)
                .ToList();
        }

        /// <inheritdoc />
        public IList<ManOrgLicenseDoc> ListLicenseDocs(long licenseId)
        {
            return this.ManOrgLicenseDocDomain.GetAll()
                .Where(x => x.ManOrgLicense.Id == licenseId)
                .ToList();
        }

        ///<inheritdoc/>
        public IDataResult GetListPersonByContragent(BaseParams baseParams, bool isPaging, out int totalCount)
        {
            totalCount = 0;

            var loadParams = baseParams.GetLoadParam();

            var ctrId = baseParams.Params.GetAs("contragentId", 0L);
            var requestId = baseParams.Params.GetAs("requestId", 0L);
            var dateRequest = baseParams.Params.GetAs("dateRequest", DateTime.MinValue);

            if (dateRequest == DateTime.MinValue)
            {
                return new BaseDataResult(false, "Необходимо указать дату обращения");
            }

            var placeWorkDict = this.placeWorkDomain.GetAll()
                .Where(x => x.Contragent.Id == ctrId && x.StartDate.HasValue)
                .Select(
                    x => new
                    {
                        personId = x.Person.Id,
                        position = x.Position.Name,
                        date = x.StartDate.Value
                    })
                .AsEnumerable()
                .GroupBy(x => x.personId)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.date).Select(z => z.position).First());

            var personIds = this.placeWorkDomain.GetAll()
                .Where(x => x.Contragent.Id == ctrId)
                .Where(x => x.StartDate <= dateRequest)
                .Where(x => !x.EndDate.HasValue || x.EndDate.Value >= dateRequest)
                .Select(x => x.Person.Id)
                .Distinct()
                .ToList();

            personIds = this.qualDomain.GetAll()
                .Where(x => personIds.Contains(x.Person.Id))
                .Where(x => !x.HasCancelled || x.HasRenewed)
                .Where(x => x.IssuedDate.HasValue && x.IssuedDate.Value <= dateRequest)
                .Where(x => !x.EndDate.HasValue || x.EndDate.Value >= dateRequest)
                .Select(x => x.Person.Id)
                .Distinct()
                .ToList();

            var disqQuery = this.disqualDomain.GetAll()
                .Where(x => x.DisqDate.HasValue && x.DisqDate.Value <= dateRequest)
                .Where(x => !x.EndDisqDate.HasValue || x.EndDisqDate.Value >= dateRequest);

            var data = this.PersonDomain.GetAll()
                .Where(x => !disqQuery.Any(y => y.Person.Id == x.Id))
                .Where(x => !this.RequestPersonDomain.GetAll().Any(y => y.Person.Id == x.Id && y.LicRequest.Id == requestId))
                .Where(x => personIds.Contains(x.Id))
                .Select(
                    x => new
                    {
                        x.Id,
                        x.FullName
                    })
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.FullName,
                        Position = placeWorkDict.Get(x.Id) ?? ""
                    })
                .AsQueryable()
                .Filter(loadParams, this.Container);

            totalCount = data.Count();

            if (isPaging)
            {
                return new BaseDataResult(data.Order(loadParams).Paging(loadParams).ToList());
            }

            return new BaseDataResult(data.Order(loadParams).ToList());
        }

        ///<inheritdoc/>
        public IDataResult GetContragentInfo(BaseParams baseParams)
        {
            var requestId = baseParams.Params.GetAs("requestId", 0L);

            var request = this.RequestDomain.GetAll().FirstOrDefault(x => x.Id == requestId);

            if (request == null)
            {
                return new BaseDataResult(false, "Не удалось определить заявку на лицензию по Id " + request.ToStr());
            }


            var contragent = request.Contragent ?? request.ManOrgLicense?.Contragent;

            if (contragent == null)
            {
                return new BaseDataResult();
            }

            return new BaseDataResult(
                new
                {
                    contragent.Id,
                    contragent.Name,
                    contragent.ShortName,
                    OrgForm = contragent.OrganizationForm != null ? contragent.OrganizationForm.Name : string.Empty,
                    JurAddress = contragent.FiasJuridicalAddress != null ? contragent.FiasJuridicalAddress.AddressName : string.Empty,
                    FactAddress = contragent.FiasFactAddress != null ? contragent.FiasFactAddress.AddressName : string.Empty,
                    contragent.Ogrn,
                    contragent.Inn,
                    contragent.OgrnRegistration,
                    contragent.Phone,
                    contragent.Email,
                    contragent.TaxRegistrationSeries,
                    contragent.TaxRegistrationNumber,
                    contragent.TaxRegistrationIssuedBy,
                    contragent.TaxRegistrationDate
                });
        }

        private IFileManager _fileManager;

        private readonly ITaskManager _taskManager;

        private IDomainService<FileInfo> _fileDomain;

        public ManOrgLicenseRequestService(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        public IDataResult AddSMEVRequest(BaseParams baseParams, RequestSMEVType requestType, long requestId)
        {
            Inspector thisInspector = null;
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator.Inspector != null)
                thisInspector = thisOperator.Inspector;
            else
                return new BaseDataResult(false, "Обмен данными со СМЭВ доступен только сотрудникам ГЖИ");

            ManOrgLicenseRequest req = RequestDomain.Get(requestId);

            if (requestType == RequestSMEVType.EGRULRequest && requestId > 0)
            {
                SMEVEGRUL newEGRUL = new SMEVEGRUL
                {
                    InnOgrn = Enums.InnOgrn.INN,
                    INNReq = req.Contragent.Inn,
                    Inspector = thisInspector
                };
                SMEVEGRULDomain.Save(newEGRUL);

                ManOrgRequestSMEV newRequest = new ManOrgRequestSMEV
                {
                    RequestSMEVType = RequestSMEVType.EGRULRequest,
                    Date = DateTime.Now,
                    LicRequest = new ManOrgLicenseRequest { Id = requestId },
                    SMEVRequestState = SMEVRequestState.Formed,
                    RequestId = newEGRUL.Id,
                    Inspector = thisInspector
                };
                ManOrgRequestSMEVDomain.Save(newRequest);

                var smevRequestData = SMEVEGRULDomain.Get(newEGRUL.Id);
                if (smevRequestData == null)
                    return new BaseDataResult(false, "Запрос не найден");

                if (smevRequestData.RequestState == RequestState.Queued)
                    return new BaseDataResult(false, "Запрос уже отправлен");

                try
                {
                    baseParams.Params.Add("taskId", newEGRUL.Id);
                    _taskManager.CreateTasks(new SendInformationRequestTaskProvider(Container), baseParams);
                    return GetResponce(baseParams, newEGRUL.Id);
                }
                catch (Exception e)
                {
                    return new BaseDataResult(false, "Создание задачи на запрос данных из ЕГРЮЛ не удалось: " + e.Message);
                }


            }
            else if (requestType == RequestSMEVType.EGRIPRequest && requestId > 0)
            {
                SMEVEGRIP newEGRIP = new SMEVEGRIP
                {
                    InnOgrn = Enums.InnOgrn.INN,
                    INNReq = req.Contragent.Inn,
                    Inspector = thisInspector
                };
                SMEVEGRIPDomain.Save(newEGRIP);

                ManOrgRequestSMEV newRequest = new ManOrgRequestSMEV
                {
                    RequestSMEVType = RequestSMEVType.EGRIPRequest,
                    Date = DateTime.Now,
                    LicRequest = new ManOrgLicenseRequest { Id = requestId },
                    SMEVRequestState = SMEVRequestState.Formed,
                    RequestId = newEGRIP.Id,
                    Inspector = thisInspector
                };
                ManOrgRequestSMEVDomain.Save(newRequest);

                var smevRequestData = SMEVEGRULDomain.Get(newEGRIP.Id);
                if (smevRequestData == null)
                    return new BaseDataResult(false, "Запрос не найден");

                if (smevRequestData.RequestState == RequestState.Queued)
                    return new BaseDataResult(false, "Запрос уже отправлен");

                try
                {
                    baseParams.Params.Add("taskId", newEGRIP.Id);
                    _taskManager.CreateTasks(new SendEGRIPRequestTaskProvider(Container), baseParams);
                    return GetEGRIPResponce(baseParams, newEGRIP.Id);
                }
                catch (Exception e)
                {
                    return new BaseDataResult(false, "Создание задачи на запрос данных из ЕГРЮЛ не удалось: " + e.Message);
                }
            }
            return new BaseDataResult();
        }

        public BaseDataResult GetResponce(BaseParams baseParams, Int64 taskId)
        {
            //Из-за нехватки времени все проверки ответа запускают таску на проверку всех ответоп
            var smevRequestData = SMEVEGRULDomain.Get(taskId);
            if (smevRequestData == null)
                return new BaseDataResult(false, "Запрос не найден");

            if (smevRequestData.RequestState == RequestState.ResponseReceived)
                return new BaseDataResult(false, "Ответ уже получен");

            //if (!baseParams.Params.ContainsKey("taskId"))
            //    baseParams.Params.Add("taskId", taskId);

            try
            {
                _taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), baseParams);
                return new BaseDataResult(true, "Задача поставлена в очередь задач");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, "Создание задачи на проверку ответов не удалось: " + e.Message);
            }
        }

        public BaseDataResult GetEGRIPResponce(BaseParams baseParams, Int64 taskId)
        {
            //Из-за нехватки времени все проверки ответа запускают таску на проверку всех ответоп
            var smevRequestData = SMEVEGRIPDomain.Get(taskId);
            if (smevRequestData == null)
                return new BaseDataResult(false, "Запрос не найден");

            if (smevRequestData.RequestState == RequestState.ResponseReceived)
                return new BaseDataResult(false, "Ответ уже получен");

            //if (!baseParams.Params.ContainsKey("taskId"))
            //    baseParams.Params.Add("taskId", taskId);

            try
            {
                _taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), baseParams);
                return new BaseDataResult(true, "Задача поставлена в очередь задач");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, "Создание задачи на проверку ответов не удалось: " + e.Message);
            }
        }
    }
}