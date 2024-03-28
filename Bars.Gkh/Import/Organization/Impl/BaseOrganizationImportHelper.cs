namespace Bars.Gkh.Import.Organization.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhExcel;

    using B4;

    using Enums;

    /// <summary>
    /// Помощник импорта базовой организации
    /// </summary>
    public abstract class BaseOrganizationImportHelper
    {
        public BaseOrganizationImportHelper(IOrganizationImportCommonHelper commonHelper, IDictionary<string, long> headersDict, ILogImport logImport)
        {
            this.CommonHelper = commonHelper;
            this.HeadersDict = headersDict;
            this.LogImport = logImport;
        }

        /// <summary>
        /// Домен-сервиса "Жилой дом договора управляющей организации"
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectService { get; set; }

        protected IOrganizationImportCommonHelper CommonHelper { get; set; }

        protected IDictionary<string, long> HeadersDict { get; set; }
        
        public ILogImport LogImport { get; }

        protected string GetValue(GkhExcelCell[] data, string field)
        {
            var result = string.Empty;

            if (this.HeadersDict.ContainsKey(field))
            {
                var index = this.HeadersDict[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
            }

            return result.Trim();
        }

        protected abstract void GetOrganizationId(Record record);

        protected virtual IDataResult CreateOrSetOrganization(Record record)
        {
            return new BaseDataResult();
        }

        protected abstract void CreateContractIfNotExist(Record record, bool updatePeriodsManOrgs = false);

        /// <summary>
        /// Создание контракта
        /// </summary>
        /// <param name="record">Запись из файла</param>
        /// <param name="updatePeriod">Обновить период</param>
        public IDataResult CreateContract(Record record, bool updatePeriod)
        {
            var organizationId = record.OrganizationId;

            if (organizationId == 0)
            {
                var result = this.CommonHelper.CreateOrSetContragent(record);

                if (!result.Success || record.Contragent == null)
                {
                    return result;
                }

                result = this.CreateOrSetOrganization(record);

                if (!result.Success || record.Organization == null)
                {
                    return result;
                }
            }

            var endDateManOrgContract = this.ManOrgContractRealityObjectService.GetAll()
                .Where(x => x.RealityObject.Id == record.RealtyObjectId)
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
                .OrderByDescending(x => x.ManOrgContract.EndDate)
                .Select(x => x.ManOrgContract.EndDate).FirstOrDefault();

            if (endDateManOrgContract != null)
            {
                var dats = record.ContractStartDate.Subtract(endDateManOrgContract.Value).Days;

                if (dats > 1000 && dats < 0)
                {
                    var msg =
                        $"Перерыв в управлении МКД не может быть больше одного дня.Дата окончания предыдущего управления: {endDateManOrgContract.Value.ToShortDateString()}";
                    return BaseDataResult.Error(msg);
                }
            }

            this.CreateContractIfNotExist(record, updatePeriod);

            return new BaseDataResult();
        }

        /// <summary>
        /// Парсинг записи из файла  
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="record">Запись</param>
        /// <returns>Результат</returns>
        public virtual IDataResult ProcessLine(GkhExcelCell[] data, Record record)
        {
            record.ImportOrganizationId = this.GetValue(data, "ID_COM").ToInt();

            var rowNumber = $"Строка {record.RowNumber}";

            if (record.ImportOrganizationId > 0)
            {
                this.GetOrganizationId(record);
            }

            if (record.OrganizationId == 0)
            {
                record.Inn = this.GetValue(data, "INN");
                record.Kpp = this.GetValue(data, "KPP");
                record.OrganizationName = this.GetValue(data, "NAME_COM");

                var mixedKey = $"{record.Inn}#{record.Kpp}";

                record.ContragentMixedKey = mixedKey;
                var contragentsList = this.CommonHelper.GetContragentListByMixedKey(mixedKey);
                var contragents = this.CommonHelper.GetContragents();

                if (contragentsList.Any())
                {
                    if (contragentsList.Count > 1)
                    {
                        var msg = "Неоднозначная ситуация. Соответствующих данному ИНН/КПП контрагентов найдено: " + contragentsList.Count;
                        this.WriteLog(rowNumber, msg, LogMessageType.Error);
                        return BaseDataResult.Error(msg);
                    }

                    if (contragentsList.FirstOrDefault().Name.ToLower().Trim() != record.OrganizationName.ToLower().Trim())
                    {
                        var msg = "Запись с таким ИНН и КПП существует в системе, но с другим наименованием;";
                        this.WriteLog(rowNumber, msg, LogMessageType.Error);
                        return BaseDataResult.Error(msg);
                    }
                }

                else
                {
                    if (!(Utils.Utils.VerifyInn(record.Inn, false) || Utils.Utils.VerifyInn(record.Inn, true)))
                    {
                        var msg = "Некорректный ИНН: " + record.Inn;
                        this.WriteLog(rowNumber, msg, LogMessageType.Error);
                        return BaseDataResult.Error(msg);
                    }

                    record.Ogrn = this.GetValue(data, "OGRN");

                    if (!(Utils.Utils.VerifyOgrn(record.Ogrn, false) || Utils.Utils.VerifyOgrn(record.Ogrn, true)))
                    {
                        var msg = "Некорректный ОГРН: " + record.Ogrn;
                        this.WriteLog(rowNumber, msg, LogMessageType.Error);
                        return BaseDataResult.Error(msg);
                    }

                    if (string.IsNullOrEmpty(record.OrganizationName))
                    {
                        var msg = "Не задано Наименование юр лица.";
                        this.WriteLog(rowNumber, msg, LogMessageType.Error);
                        return BaseDataResult.Error(msg);
                    }

                    record.OrgStreetKladrCode = this.GetValue(data, "KLADR_COM");
                    record.OrgMunicipalityName = this.GetValue(data, "MU_COM");

                    if (string.IsNullOrEmpty(record.OrgMunicipalityName))
                    {
                        record.OrgMunicipalityName = this.GetValue(data, "MR_COM");

                        if (string.IsNullOrEmpty(record.OrgMunicipalityName))
                        {
                            var msg = "Организация: не задан мун.район и мун.образование.";
                            this.WriteLog(rowNumber, msg, LogMessageType.Error);
                            return BaseDataResult.Error(msg);
                        }
                    }

                    var municipalityProxy = this.CommonHelper.GetMunicipalityProxy(record.OrgMunicipalityName);

                    if (municipalityProxy == null)
                    {
                        var msg = "Организация: указанный мун.район/мун.образование не найден в системе: " + record.OrgMunicipalityName;
                        this.WriteLog(rowNumber, msg, LogMessageType.Error);
                        return BaseDataResult.Error(msg);
                    }

                    record.ContragentMunicipalityId = municipalityProxy.Id;
                    record.ContragentMunicipalityFiasId = municipalityProxy.FiasId;

                    record.OrgLocalityName = Simplified(this.GetValue(data, "CITY_COM") + " " + this.GetValue(data, "TYPE_CITY_COM"));
                    record.OrgStreetName = Simplified(this.GetValue(data, "STREET_COM") + " " + this.GetValue(data, "TYPE_STREET_COM"));
                    record.OrgHouse = this.GetValue(data, "HOUSE_NUM_COM");

                    if (string.IsNullOrWhiteSpace(record.OrgHouse))
                    {
                        var msg = "Организация: Не задан номер дома.";
                        this.WriteLog(rowNumber, msg, LogMessageType.Error);
                        return BaseDataResult.Error(msg);
                    }

                    record.OrgLetter = this.GetValue(data, "LITER_COM");
                    record.OrgHousing = this.GetValue(data, "KORPUS_COM");
                    record.OrgBuilding = this.GetValue(data, "BUILDING_COM");

                    var organizationForm = this.GetValue(data, "TYPE_LAW_FORM");
                    if (string.IsNullOrEmpty(organizationForm))
                    {
                        var msg = "Не задана Организационно-правовая форма.";
                        this.WriteLog(rowNumber, msg, LogMessageType.Error);
                        return BaseDataResult.Error(msg);
                    }
                    else
                    {
                        var organizationFormId = this.CommonHelper.GetOrganizationFormIdByName(organizationForm);

                        if (organizationFormId == null)
                        {
                            var msg = "Неизвестная Организационно-правовая форма: " + organizationForm;
                            this.WriteLog(rowNumber, msg, LogMessageType.Error);
                            return BaseDataResult.Error(msg);
                        }

                        record.OrganizationForm = new OrganizationForm { Id = organizationFormId.Value };
                    }

                    var dateRegistration = this.GetValue(data, "DATE_REG");

                    if (!string.IsNullOrEmpty(dateRegistration))
                    {
                        dateRegistration = dateRegistration.Trim();
                    }

                    if (string.IsNullOrWhiteSpace(dateRegistration))
                    {
                        var msg = "Не задана Дата регистрации организации.";
                        this.WriteLog(rowNumber, msg, LogMessageType.Error);
                        return BaseDataResult.Error(msg);
                    }
                    else
                    {
                        DateTime date;

                        dateRegistration = dateRegistration.Length > 10
                            ? dateRegistration.Substring(0, 10)
                            : dateRegistration;

                        if (DateTime.TryParseExact(
                            dateRegistration,
                            "dd.MM.yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out date))
                        {
                            record.DateRegistration = date;
                        }
                        else
                        {
                            var msg = "Некорректная дата в поле 'DATE_REG': " + dateRegistration + "'. Дата ожидается в формате 'дд.мм.гггг'";
                            this.WriteLog(rowNumber, msg, LogMessageType.Error);
                            return BaseDataResult.Error(msg);
                        }
                    }
                }

                foreach (var contragent in contragents)
                {
                    if (contragent.Kpp == record.Kpp
                        && contragent.Name.ToLower().Trim() == record.OrganizationName.ToLower().Trim()
                        && contragent.Inn != record.Inn)
                    {
                        var msg = "Запись с таким Наименованием и КПП существует, но с другим ИНН";
                        this.WriteLog(rowNumber, msg, LogMessageType.Warning);
                    }

                    if (contragent.Inn == record.Inn
                        && contragent.Name.ToLower().Trim() == record.OrganizationName.ToLower().Trim()
                        && contragent.Kpp != record.Kpp)
                    {
                        var msg = "Запись с таким Наименованием и ИНН существует, но с другим КПП";
                        this.WriteLog(rowNumber, msg, LogMessageType.Warning);
                    }
                }
            }
            return new BaseDataResult();
        }

        /// <summary>
        /// Returns a string that has space removed from the start and the end, and that has each sequence of internal space replaced with a single space.
        /// </summary>
        /// <param name="initialString"></param>
        /// <returns>Результат</returns>
        private static string Simplified(string initialString)
        {
            if (string.IsNullOrEmpty(initialString))
            {
                return initialString;
            }

            var trimmed = initialString.Trim();

            if (!trimmed.Contains(" "))
            {
                return trimmed;
            }

            var result = string.Join(" ", trimmed.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)));

            return result;
        }
        
        /// <summary>
        /// Запись в лог импорта 
        /// </summary>
        protected void WriteLog(string rowNumber, string message, LogMessageType logMessageType)
        {
            switch (logMessageType)
            {
                case LogMessageType.Debug:
                    this.LogImport.Debug(rowNumber, message);
                    break;
                case LogMessageType.Info:
                    this.LogImport.Info(rowNumber, message);
                    break;
                case LogMessageType.Warning:
                    this.LogImport.Warn(rowNumber, message);
                    break;
                case LogMessageType.Error:
                    this.LogImport.Error(rowNumber, message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logMessageType), logMessageType, null);
            }
        }
    }
}