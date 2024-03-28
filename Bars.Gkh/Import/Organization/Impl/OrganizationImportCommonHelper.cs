namespace Bars.Gkh.Import.Organization.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import.FiasHelper;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using NHibernate;

    public class OrganizationImportCommonHelper : IOrganizationImportCommonHelper
    {
        public IWindsorContainer Container { get; set; }

        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        public IRepository<ManOrgContractRealityObject> ManOrgContractRealityObjectRepository { get; set; }

        public IRepository<Contragent> ContragentRepository { get; set; }

        public IRepository<OrganizationForm> OrganizationFormRepository { get; set; }

        public IRepository<Municipality> MunicipalityRepository { get; set; }

        private IFiasHelper fiasHelper;

        private Dictionary<string, List<Contragent>> contragentsDict;

        private Dictionary<string, MunicipalityProxy> municipalitiesDict;

        private Dictionary<string, long> organizationFormDict;

        private List<Contragent> contragentsToCreate = new List<Contragent>();

        private List<FiasAddress> fiasAddressesToCreate = new List<FiasAddress>();

        private Dictionary<long, RealityObject> realityObjectDict;

        private Dictionary<long, RealityObjectManOrgDictData> realityObjectManOrgDict;

        private Dictionary<long, RealityObject> realityObjectToUpdate = new Dictionary<long, RealityObject>();

        public void Init()
        {
            // В случае с IFiasHelper резолвить именно так, чтобы не было лишней обработки из-за IInitializible при вызове других импортов 
            this.fiasHelper = this.Container.Resolve<IFiasHelper>();

            this.contragentsDict = this.ContragentRepository.GetAll()
                .Where(x => x.ContragentState != ContragentState.Bankrupt)
                .Where(x => x.ContragentState != ContragentState.Liquidated)
                .Select(x => new { Contragent = x, x.Inn, x.Kpp })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Contragent,
                    mixedkey = string.Format(
                        "{0}#{1}",
                        (x.Inn ?? string.Empty).Trim(),
                        (x.Kpp ?? string.Empty).Trim()).ToLower()
                })
                .GroupBy(x => x.mixedkey)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Contragent).ToList());

            // словарь Организационно-правовых форм
            this.organizationFormDict = this.OrganizationFormRepository.GetAll()
                .Select(x => new { Name = x.Name ?? "", x.Id })
                .AsEnumerable()
                .GroupBy(x => x.Name.ToLower())
                .ToDictionary(x => x.Key, x => x.First().Id);

            // словарь Муниципальных образований
            this.municipalitiesDict = this.MunicipalityRepository.GetAll()
                .Select(x => new { Name = x.Name ?? "", x.Id, x.FiasId })
                .AsEnumerable()
                .GroupBy(x => x.Name.ToLower())
                .ToDictionary(x => x.Key, x => new MunicipalityProxy { Id = x.First().Id, FiasId = x.First().FiasId });

            this.realityObjectManOrgDict = this.ManOrgContractRealityObjectRepository.GetAll()
                .Where(x => x.ManOrgContract.StartDate <= DateTime.Now.Date)
                .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Now.Date)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    ManOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    InnManOrg = x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                    StartControlDate = x.ManOrgContract.StartDate,
                    x.ManOrgContract.TypeContractManOrgRealObj
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(
                    x => x.Key,
                    x => new RealityObjectManOrgDictData
                    {
                        ManOrgs = x.AggregateWithSeparator(y => y.ManOrgName, ", "),
                        InnManOrgs = x.AggregateWithSeparator(y => y.InnManOrg, ", "),
                        StartControlDate = x.AggregateWithSeparator(y => y.StartControlDate?.ToString("dd.MM.yyyy"), ","),
                        TypesContract = x.AggregateWithSeparator(y => y.TypeContractManOrgRealObj.GetEnumMeta().Display, ", ")
                    });

            this.realityObjectDict = this.RealityObjectRepository.GetAll().ToDictionary(x => x.Id);
        }

        public void SaveData(IStatelessSession session)
        {
            this.fiasAddressesToCreate.ForEach(x => session.Insert(x));
            this.contragentsToCreate.ForEach(x => session.Insert(x));
            this.realityObjectToUpdate.Select(x => x.Value).ForEach(session.Update);
        }

        public List<Contragent> GetContragentListByMixedKey(string mixedKey)
        {
            if (this.contragentsDict.ContainsKey(mixedKey))
            {
                return this.contragentsDict[mixedKey];
            }

            return new List<Contragent>();
        }

        public MunicipalityProxy GetMunicipalityProxy(string name)
        {
            if (this.municipalitiesDict.ContainsKey(name.ToLower()))
            {
                return this.municipalitiesDict[name.ToLower()];
            }

            return null;
        }

        public long? GetOrganizationFormIdByName(string name)
        {
            if (this.organizationFormDict.ContainsKey(name.ToLower()))
            {
                return this.organizationFormDict[name.ToLower()];
            }

            return null;
        }

        public List<ContragentProxy> GetContragents()
        {
            var enumerable = this.contragentsDict.Values.SelectMany(x => x.ToList());

            return enumerable.Where(x => x.Kpp != null && x.Inn != null)
                .Select(x => new ContragentProxy
                {
                    Name = x.Name,
                    Inn = x.Inn,
                    Kpp = x.Kpp
                })
                .ToList();
        }

        public IDataResult CreateOrSetContragent(Record record)
        {
            var mixedkey = string.Format("{0}#{1}", record.Inn, record.Kpp).ToLower();
            
            if (this.contragentsDict.ContainsKey(mixedkey))
            {
                record.Contragent = this.contragentsDict[mixedkey].First();

                return new BaseDataResult();
            }

            var result = new BaseDataResult();
            var fiasAddress = this.CreateAddressForContragent(record, result);
            
            if (fiasAddress == null)
            {
                return result;
            }

            var contragent = new Contragent
            {
                Name = record.OrganizationName,
                Inn = record.Inn,
                Kpp = record.Kpp,
                Ogrn = record.Ogrn,
                FiasJuridicalAddress = fiasAddress,
                JuridicalAddress = fiasAddress.AddressName,
                Municipality = new Municipality { Id = record.ContragentMunicipalityId },
                DateRegistration = record.DateRegistration,
                ContragentState = ContragentState.Active,
                OrganizationForm = record.OrganizationForm,
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now
            };

            fiasAddress.ObjectCreateDate = DateTime.Now;
            fiasAddress.ObjectEditDate = DateTime.Now;

            this.fiasAddressesToCreate.Add(fiasAddress);
            this.contragentsToCreate.Add(contragent);

            record.Contragent = contragent;

            this.contragentsDict[mixedkey] = new List<Contragent> { contragent };

            return result;
        }

        private FiasAddress CreateAddressForContragent(Record record, IDataResult message)
        {
            var faultReason = string.Empty;
            DynamicAddress address;

            message.Data = LogMessageType.Error;
            message.Success = false;

            if (this.RecordHasValidCodeKladrStreet(record))
            {
                if (!this.fiasHelper.FindInBranchByKladr(record.ContragentMunicipalityFiasId, record.OrgStreetKladrCode, ref faultReason, out address))
                {
                    message.Message = "Организация: " + faultReason;
                    return null;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(record.OrgLocalityName))
                {
                    message.Message = "Организация: не задан населенный пункт.";
                    return null;
                }

                if (string.IsNullOrWhiteSpace(record.OrgStreetName))
                {
                    message.Message = "Организация: не задана улица.";
                    return null;
                }

                if (!this.fiasHelper.FindInBranch(record.ContragentMunicipalityFiasId, record.OrgLocalityName, record.OrgStreetName, ref faultReason, out address))
                {
                    message.Message = "Организация: " + faultReason;
                    return null;
                }
            }

            message.Data = LogMessageType.Info;
            message.Success = true;
            return this.fiasHelper.CreateFiasAddress(address, record.OrgHouse, record.OrgLetter, record.OrgHousing, record.OrgBuilding);
        }

        private bool RecordHasValidCodeKladrStreet(Record record)
        {
            if (string.IsNullOrWhiteSpace(record.OrgStreetKladrCode))
            {
                return false;
            }

            var codeLength = record.OrgStreetKladrCode.Length;

            if (codeLength < 15)
            {
                return false;
            }

            if (codeLength > 17)
            {
                record.OrgStreetKladrCode = record.OrgStreetKladrCode.Substring(0, 17);
            }
            else if (codeLength < 17)
            {
                record.OrgStreetKladrCode = record.OrgStreetKladrCode + (codeLength == 15 ? "00" : "0");
            }

            return this.fiasHelper.HasValidStreetKladrCode(record.OrgStreetKladrCode);
        }

        public void UpdateRealtyObjectManOrg(long roId, DateTime contractStartDate, 
            string manOrgName, string manInn, string startControlDate, string typeContract)
        {
            if (contractStartDate > DateTime.Now)
            {
                return;
            }

            var realtyObject = this.realityObjectDict[roId];

            if (this.realityObjectManOrgDict.ContainsKey(roId))
            {
                var manorgData = this.realityObjectManOrgDict[roId];

                realtyObject.ManOrgs = manorgData.ManOrgs;
                realtyObject.InnManOrgs = manorgData.InnManOrgs;
                realtyObject.StartControlDate = manorgData.StartControlDate;
                realtyObject.TypesContract = manorgData.TypesContract;
            }

            realtyObject.ManOrgs = CheckVariable(realtyObject.ManOrgs, manOrgName);
            realtyObject.InnManOrgs = CheckVariable(realtyObject.InnManOrgs, manInn);
            realtyObject.StartControlDate = CheckVariable(realtyObject.StartControlDate, startControlDate);
            realtyObject.TypesContract = CheckVariable(realtyObject.TypesContract, typeContract);
            
            realtyObject.ObjectEditDate = DateTime.Now;
            realtyObject.ObjectVersion++;

            this.realityObjectToUpdate[roId] = realtyObject;
        }

        private string CheckVariable(string oldVariable, string addVariable)
        {
            if (!string.IsNullOrWhiteSpace(oldVariable))
            {
                if (!oldVariable.Contains(addVariable))
                {
                    return oldVariable + ", " + addVariable;
                }
            }

            return addVariable;
        }

        private class RealityObjectManOrgDictData
        {
            public string ManOrgs { get; set; }
            
            public string InnManOrgs { get; set; }

            public string StartControlDate { get; set; }

            public string TypesContract { get; set; }
        }
    }
}