namespace Bars.Gkh.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.HouseSearch;
    using Bars.Gkh.Authentification;
    using Bars.B4;
    using Bars.Gkh.Services.DataContracts.GetAllHouses;
    using System;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml;
    using System.Net;
    using Bars.Gkh.Services.ServiceContracts;

    using CoreWCF.Web;

    public partial class Service
    {
        /// <summary>
        ///  Конструктивный элемент дома
        /// </summary>
        public IDomainService<ViewStructElementRealObj> RealityObjectStructuralElementDomain { get; set; }

        /// <summary>
        ///  Конструктивный элемент дома
        /// </summary>
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        /// <summary>
        /// Получить все дома
        /// </summary>
        /// <returns>Ответ со списком домов</returns>
        public AllHousesResponse GetAllHouses(string afterDate)
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            DateTime? dateFrom = null;
            try
            {
                dateFrom = Convert.ToDateTime(afterDate);
            }
            catch
            {
                dateFrom = null;
            }
            try
            {

                string login = headers["UserLogin"].ToString();
                string password = headers["UserPassword"].ToString();
                //var hashpsw = MD5.GetHashString64("xt67Br41!*", HashType.MD5B2);
                var correctLogin = "GeoPortal";
                if (login != correctLogin || password != "xt67Br41!*")
                {
                    return new AllHousesResponse { AllHouses = null, Result = Result.AuthorizationFailed };
                }
               
            }
            catch
            {
                return new AllHousesResponse { AllHouses = null, Result = Result.NoLoginPassword };
            }
            var houses = RealityObjectDomain.GetAll()
                .WhereIf(dateFrom.HasValue, x=> x.ObjectCreateDate>= dateFrom)
                     .Select(x => new AllHouses
                     {
                         Id = x.Id,
                         NameMO = x.Municipality.Name,
                         FiasAddress = x.FiasAddress.AddressName ?? "",
                         HouseGuid = x.FiasAddress.HouseGuid.ToString() ?? ""
                     })
                     .OrderBy(x => x.NameMO)
                     .ThenBy(x => x.FiasAddress)
                     .ToArray();

            var result = houses.Length == 0 ? Result.DataNotFound : Result.NoErrors;
            return new AllHousesResponse { AllHouses = houses, Result = result };
        }

        /// <summary>
        /// Получить все дома
        /// </summary>
        /// <returns>Ответ со списком домов</returns>
        public HouseElemPropResponse GetHouseProp(string houseId)
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            try
            {
                string login = headers["UserLogin"].ToString();
                string password = headers["UserPassword"].ToString();
                //var hashpsw = MD5.GetHashString64("xt67Br41!*", HashType.MD5B2);
                var correctLogin = "GeoPortal";
                if (login != correctLogin || password != "xt67Br41!*")
                {
                    return new HouseElemPropResponse {Result = Result.AuthorizationFailed };
                }
            }
            catch
            {
                return new HouseElemPropResponse {Result = Result.NoLoginPassword };
            }

            long mkdId = Convert.ToInt64(houseId);
            var viewRoDomain = this.Container.ResolveDomain<ViewStructElementRealObj>();
            List<HouseElements> elements = new List<HouseElements>();

            var properties = RealityObjectDomain.GetAll()
                    .Where(x => x.Id == mkdId)
                    .Select(x => new HouseProp
                    {
                        Id = x.Id,
                        Address = x.Address,
                        TypeHouse = x.TypeHouse,
                        HouseNumber = x.FiasAddress.House,
                        ConditionHouse = x.ConditionHouse,
                        BuildYear = x.BuildYear ?? 0,
                        DateCommissioning = x.DateCommissioning ?? DateTime.MinValue,
                        HasPrivatizedFlats = x.HasPrivatizedFlats ?? false,
                        PrivatizationDateFirstApartment = x.PrivatizationDateFirstApartment ?? DateTime.MinValue,
                        NecessaryConductCr = Enums.YesNoNotSet.Yes,
                        DateLastOverhaul = x.DateLastOverhaul ?? DateTime.MinValue,
                        TypeProject = x.TypeProject.Name,
                        PhysicalWear = x.PhysicalWear ?? 0,                        
                        CadastreNumber = x.CadastreNumber,
                        AreaMkd = x.AreaMkd ?? 0,
                        AreaOwned = x.AreaOwned ?? 0,
                        AreaMunicipalOwned = x.AreaMunicipalOwned ?? 0,
                        AreaGovernmentOwned = x.AreaGovernmentOwned ?? 0,
                        AreaLivingNotLivingMkd = x.AreaLivingNotLivingMkd ?? 0,
                        AreaLiving = x.AreaLiving ?? 0,
                        AreaLivingOwned = x.AreaLivingOwned ?? 0,
                        AreaNotLivingFunctional = x.AreaNotLivingFunctional ?? 0,
                        AreaCommonUsage = x.AreaCommonUsage ?? 0,
                        AreaCleaning = x.AreaCleaning ?? 0,
                        Floors = x.Floors ?? 0,
                        MaximumFloors = x.MaximumFloors ?? 0,
                        FloorHeight = x.FloorHeight ?? 0,
                        NumberEntrances = x.NumberEntrances ?? 0,
                        NumberApartments = x.NumberApartments ?? 0,
                        NumberLiving = x.NumberLiving ?? 0,
                        NumberLifts = x.NumberLifts ?? 0,
                        RoofingMaterial = x.RoofingMaterial.Name,
                        WallMaterial = x.WallMaterial.Name,
                        TypeRoof = x.TypeRoof,
                        HeatingSystem = x.HeatingSystem,
                        ManOrgs = x.ManOrgs
                    })
                    .ToArray();     

            var query = viewRoDomain.GetAll()
                .Where(x => x.RealityObjectId>0 &&  x.RealityObjectId == mkdId)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObjectId,
                    GroupName = x.Ooi,
                    ElementName = x.NameStructEl,
                    Unit = x.UnitMeasure,
                    x.Volume,
                    Wear = x.Wearout,
                    YearRepair = x.LastYear
                })
                .ToList();

            foreach (var v in query)
            {
                try
                {
                    elements.Add(new HouseElements
                    {
                        Id = v.Id,
                        RealityObject = Convert.ToInt32(v.RealityObjectId),
                        GroupName = v.GroupName,
                        ElementName = v.ElementName,
                        Unit = v.Unit,
                        Volume = v.Volume,
                        Wear = v.Wear,
                        YearRepair = v.YearRepair
                    });
                }
                catch (Exception e)
                {

                }
            }

            var result = properties.Length == 0 && elements.Count == 0 ? Result.DataNotFound : Result.NoErrors;
            try
            {

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(HouseElemPropResponse));

                HouseElemPropResponse newResponce = new HouseElemPropResponse { Houses = properties, Elements = elements.ToArray(), Result = result };

                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
                    {
                        xmlSerializer.Serialize(xmlWriter, newResponce);

                        var resultXml = new XmlDocument();
                        resultXml.LoadXml(stringWriter.ToString());
                    }
                }
            }
            catch (Exception e)
            {

            }

            return new HouseElemPropResponse { Houses = properties, Elements = elements.ToArray(), Result = result };
        }
    }
}