namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using System.IO;

    public class ManOrgLicenseService : IManOrgLicenseService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ManOrgLicenseRequest> RequestDomain { get; set; }

        public IDomainService<ManOrgLicense> LicenseDomain { get; set; }

        public IDomainService<ManOrgLicensePerson> LicensePersonDomain { get; set; }

        public IDomainService<Person> PersonDomain { get; set; }

        public IDomainService<PersonPlaceWork> placeWorkDomain { get; set; }

        public IDomainService<PersonQualificationCertificate> qualDomain { get; set; }

        public IDomainService<PersonDisqualificationInfo> disqualDomain { get; set; }


        public IGkhUserManager UserManager { get; set; }

        ///<inheritdoc/>
        public IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var contragentList = this.UserManager.GetContragentIds();

            var loadParams = baseParams.GetLoadParam();

            var dateFromStart = baseParams.Params.GetAs("dateFromStart", DateTime.MinValue);
            var dateFromEnd = baseParams.Params.GetAs("dateFromEnd", DateTime.MinValue);
            var endDateStart = baseParams.Params.GetAs("endDateStart", DateTime.MinValue);
            var endDateEnd = baseParams.Params.GetAs("endDateEnd", DateTime.MinValue);

            var data = this.LicenseDomain.GetAll()
                .WhereIf(contragentList.Any(), x => contragentList.Contains(x.Contragent.Id))
                .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateIssued >= dateFromStart)
                .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateIssued <= dateFromEnd)
                .WhereIf(endDateStart != DateTime.MinValue, x => x.DateTermination >= endDateStart)
                .WhereIf(endDateEnd != DateTime.MinValue, x => x.DateTermination <= endDateEnd)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.State,
                        x.LicNum,
                        x.LicNumber,
                        x.DateIssued,
                        x.DateValidity,
                        x.DateTermination,
                        x.DisposalNumber,
                        x.DateDisposal,
                        Request = x.Request != null ? x.Request.Id : 0,
                        Contragent = x.Contragent.Name,
                        Inn = x.Contragent.Inn,
                        Ogrn = x.Contragent.Ogrn,
                        ContragentMunicipality = x.Contragent.Municipality.Name,
                        x.Contragent.ContragentState,
                        x.ERULNumber,
                        x.ERULDate
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
        public IDataResult ListForRequestType(BaseParams baseParams)
        {
            var contragentList = this.UserManager.GetContragentIds();

            var loadParams = baseParams.GetLoadParam();
            var requestType = baseParams.Params.GetAs<LicenseRequestType>("requestType");

            var data = this.LicenseDomain.GetAll()
                .WhereIf(contragentList.Any(), x => contragentList.Contains(x.Contragent.Id))
                .WhereIf(
                    requestType == LicenseRequestType.RenewalLicense || requestType == LicenseRequestType.ExtractFromRegisterLicense,
                    x => !x.State.StartState && !x.State.FinalState)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.State,
                        x.LicNum,
                        x.DateIssued,
                        Request = x.Request != null ? x.Request.Id : 0,
                        Contragent = x.Contragent.Name,
                        x.Contragent.ShortName,
                        x.Contragent.Inn,
                        ContragentMunicipality = x.Contragent.Municipality.Name
                    })
                .AsQueryable()
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        ///<inheritdoc/>
        public IDataResult GetInfo(string type, long id)
        {
            ManOrgLicenseRequest request = null;
            ManOrgLicense license = null;

            switch (type)
            {
                case "request":
                {
                    request = this.RequestDomain.GetAll().FirstOrDefault(x => x.Id == id);

                    if (request == null)
                    {
                        return new BaseDataResult(false, "Не удалось определить заявку на лицензию по Id " + id.ToStr());
                    }

                    license = this.LicenseDomain.GetAll().FirstOrDefault(x => x.Request.Id == id) ?? request.ManOrgLicense;
                }
                    break;

                case "license":
                {
                    license = this.LicenseDomain.GetAll().FirstOrDefault(x => x.Id == id);

                    if (license == null)
                    {
                        return new BaseDataResult(false, "Не удалось определить лицензию по Id " + id.ToStr());
                    }

                    if (license.Request != null)
                    {
                        request = license.Request;
                    }
                }
                    break;
            }

            return new BaseDataResult(
                new ManOrgLicenseInfo
                {
                    licenseId = license != null ? license.Id : 0,
                    requestId = request != null ? request.Id : 0,
                    requestType = request?.Type
                });
        }

        public IDataResult GetPrintFormResult(BaseParams baseParams)
        {
            var printDomain = this.Container.ResolveAll<IPrintForm>();

            try
            {
                var printForm = printDomain.FirstOrDefault(x => x.Name == "ExportLicenseGIS");

                if (printForm == null)
                {
                    return new BaseDataResult(false);
                }

                var rp = new ReportParams();

                printForm.SetUserParams(baseParams);
                printForm.PrepareReport(rp);
                var template = printForm.GetTemplate();

                IReportGenerator generator;
                if (printForm is IGeneratedPrintForm)
                {
                    generator = printForm as IGeneratedPrintForm;
                }
                else
                {
                    generator = this.Container.Resolve<IReportGenerator>("XlsIoGenerator");
                }

                var result = new MemoryStream();

                generator.Open(template);
                generator.Generate(result, rp);
                result.Seek(0, SeekOrigin.Begin);

                return new BaseDataResult(result);
            }
            finally
            {
                this.Container.Release(printDomain);
            }
         
        }

        ///<inheritdoc/>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var type = baseParams.Params.GetAs("type", "request");
            var id = baseParams.Params.GetAs("id", 0L);

            return this.GetInfo(type, id);
        }

        ///<inheritdoc/>
        public IDataResult GetStateInfo(BaseParams baseParams)
        {
            var ctrId = baseParams.Params.GetAs("contragentId", 0L);

            // Получаем по контрагенту последнюю лицензию и оказываем в каком она статусе

            var stateName = "Отсутсвует";

            var license = this.LicenseDomain.GetAll()
                .Where(x => x.Contragent.Id == ctrId)
                .OrderByDescending(x => x.DateIssued)
                .FirstOrDefault();

            if (license != null && license.State != null)
            {
                stateName = license.State.Name;
            }

            return new BaseDataResult(
                new
                {
                    stateName
                });
        }

        ///<inheritdoc/>
        public IDataResult AddPersons(BaseParams baseParams)
        {
            var licenseId = baseParams.Params.GetAs("licenseId", 0L);
            var personIds = baseParams.Params.GetAs("personIds", new long[0]);

            var license = this.LicenseDomain.GetAll().FirstOrDefault(x => x.Id == licenseId);

            if (license == null)
            {
                return new BaseDataResult(false, "Не удалось определить лицензию по Id " + licenseId.ToStr());
            }

            var personToSave = new List<ManOrgLicensePerson>();

            var currentIds = this.LicensePersonDomain.GetAll()
                .Where(x => x.ManOrgLicense.Id == licenseId)
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
                    new ManOrgLicensePerson
                    {
                        Person = new Person { Id = id },
                        ManOrgLicense = license
                    });
            }

            if (personToSave.Any())
            {
                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        personToSave.ForEach(x => this.LicensePersonDomain.Save(x));

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
        public IDataResult GetListPersonByContragent(BaseParams baseParams, bool isPaging, out int totalCount)
        {
            totalCount = 0;

            var loadParams = baseParams.GetLoadParam();

            var contragentId = baseParams.Params.GetAs("contragentId", 0L);
            var licenseId = baseParams.Params.GetAs("licenseId", 0L);
            var date = DateTime.Now;

            var placeWorkDict = this.placeWorkDomain.GetAll()
                .Where(x => x.Contragent.Id == contragentId && x.StartDate.HasValue)
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
                .Where(x => x.Contragent.Id == contragentId)
                .Where(x => x.StartDate <= date)
                .Where(x => !x.EndDate.HasValue || x.EndDate.Value >= date)
                .Select(x => x.Person.Id)
                .Distinct()
                .ToList();

            personIds = this.qualDomain.GetAll()
                .Where(x => personIds.Contains(x.Person.Id))
                .Where(x => !x.HasCancelled || x.HasRenewed)
                .Where(x => x.IssuedDate.HasValue && x.IssuedDate.Value <= date)
                .Where(x => !x.EndDate.HasValue || x.EndDate.Value >= date)
                .Select(x => x.Person.Id)
                .Distinct()
                .ToList();

            var disqQuery = this.disqualDomain.GetAll()
                .Where(x => x.DisqDate.HasValue && x.DisqDate.Value <= date)
                .Where(x => !x.EndDisqDate.HasValue || x.EndDisqDate.Value >= date);

            var data = this.PersonDomain.GetAll()
                .Where(x => !disqQuery.Any(y => y.Person.Id == x.Id))
                .Where(x => !this.LicensePersonDomain.GetAll().Any(y => y.Person.Id == x.Id && y.ManOrgLicense.Id == licenseId))
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
            var licenseId = baseParams.Params.GetAs("licenseId", 0L);

            var license = this.LicenseDomain.GetAll().FirstOrDefault(x => x.Id == licenseId);

            if (license == null)
            {
                return new BaseDataResult(false, "Не удалось определить заявку на лицензию по Id " + license.ToStr());
            }

            if (license.Contragent == null)
            {
                return new BaseDataResult();
            }

            return new BaseDataResult(
                new
                {
                    license.Contragent.Name,
                    license.Contragent.ShortName,
                    OrgForm = license.Contragent.OrganizationForm != null ? license.Contragent.OrganizationForm.Name : string.Empty,
                    JurAddress = license.Contragent.FiasJuridicalAddress != null ? license.Contragent.FiasJuridicalAddress.AddressName : string.Empty,
                    FactAddress = license.Contragent.FiasFactAddress != null ? license.Contragent.FiasFactAddress.AddressName : string.Empty,
                    license.Contragent.Ogrn,
                    license.Contragent.Inn,
                    license.Contragent.OgrnRegistration,
                    license.Contragent.Phone,
                    license.Contragent.Email,
                    license.Contragent.TaxRegistrationSeries,
                    license.Contragent.TaxRegistrationNumber,
                    license.Contragent.TaxRegistrationIssuedBy,
                    license.Contragent.TaxRegistrationDate
                });
        }

    }
}