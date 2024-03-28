namespace Bars.Gkh.Gis.DomainService.GisAddressMatching.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.Entities.Register.HouseRegister;
    using Castle.Windsor;
    using Bars.Gkh.DomainService.AddressMatching;
    using Bars.Gkh.Dto;
    using Bars.Gkh.Gis.Entities.ImportAddressMatching;
    using Bars.Gkh.Domain;
    using System;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Gis.Enum;

    public class AddressService : IAddressService, IImportedAddressService
    {
        public IRepository<Fias> FiasRepository { get; set; }
        public IWindsorContainer Container { get; set; }
        public IRepository<HouseRegister> HouseRepository { get; set; }
        public IRepository<ImportedAddressMatch> ImportedAddressRepository { get; set; }
        public IRepository<FiasAddressUid> FiasAddressRepository { get; set; }

        /// <summary>
        /// Список районов с пагинацией
        /// </summary>        
        public IDataResult AreaListPaging(BaseParams baseParams)
        {

            var loadParams = baseParams.GetLoadParam();
            var result = GetAreaList().AsQueryable().Filter(loadParams, Container).OrderBy(x => x.Name);
            return
                new ListDataResult(
                    result.AsQueryable().Order(loadParams).Paging(loadParams).ToList(),
                    result.Count());
        }

        /// <summary>
        /// Список населенных пунктов с пагинацией
        /// </summary>        
        public IDataResult PlaceListPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var result = GetPlaceList(baseParams).AsQueryable().Filter(loadParams, Container).OrderBy(x => x.Name);
            return
                new ListDataResult(
                    result.AsQueryable().Order(loadParams).Paging(loadParams).ToList(),
                    result.Count());
        }

        /// <summary>
        /// Список улиц с пагинацией
        /// </summary>        
        public IDataResult StreetListNoGuidPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var data =
                GetStreetList(baseParams)
                    .AsQueryable()
                    .Filter(loadParams, Container)
                    .Order(loadParams);
            var result =
                data.Paging(loadParams)
                    .ToList();
            result.ForEach(x => x.Id = x.FormalName);
            return
                new ListDataResult(result, data.Count());
        }

        /// <summary>
        /// Сокращения адресных объектов уровня улицы
        /// </summary>        
        public IDataResult StreetShortNameList(BaseParams baseParams)
        {
            var data = FiasRepository.GetAll()
                .Where(x => x.ShortName != null && x.ShortName != "")
                .GroupBy(x => x.ShortName)
                .Select(x => new AoProxy
                {
                    Id = x.Key,
                    Name = x.Key
                })
                //есть 4 объекта, котрые содержат данное сокращение. Вероятно это добавили вручную
                //что бы не было ул и ул. - убираем ул.
                .Where(x => x.Name != "ул.")
                .OrderBy(x => x.Name)
                .ToList();
            return new ListDataResult(data, data.Count);
        }

        public IQueryable<AddressProxy> GetAddresses(BaseParams baseParams)
        {
            var typeAddressMatched = baseParams.Params.GetAs<TypeAddressMatched>("typeAddressMatched");
            return HouseRepository
                .GetAll()
                .Select(x => new AddressProxy
                {
                    Id = x.Id,
                    RegionName = x.Region,
                    Supplier = x.Supplier,
                    CityName = x.City != string.Empty
                        ? x.City
                        : x.Area,
                    StreetName = x.Street,
                    Number = x.HouseNum,
                    Address = Regex.Replace(Regex.Replace(
                        string.Join(",",
                            new List<string> { x.Region, x.Area, x.City, x.Street, x.HouseNum, x.BuildNum }), @"[\,]+", ","), @"^[\,]|[\,]$", ""),
                    TypeAddressMatched = x.FiasAddress == null
                        ? TypeAddressMatched.NotMatched
                        : TypeAddressMatched.MatchedFound,
                    HouseType = x.TypeHouse,
                    Municipality = x.Municipality.Name
                })
                .WhereIf(typeAddressMatched > 0, x => x.TypeAddressMatched == typeAddressMatched);
        }

        public IQueryable<ImportedAddressProxy> GetImportAddresses()
        {
            return ImportedAddressRepository.GetAll()
                .Select(x => new ImportedAddressProxy
                {
                    Id = x.Id,
                    ImportType = x.ImportType,
                    ImportFilename = x.ImportFilename,
                    AddressCodeRemote = x.AddressCode,
                    AddressRemote = (x.City == null || x.City == "") && (x.Street == null || x.Street == "") && (x.House == null || x.House == "")
                        ? ""    
                        : x.City + ", " + x.Street + ", д. " + x.House,
                    AddressCode = x.FiasAddress != null ? x.FiasAddress.Address.Id : 0,
                    Address = x.FiasAddress != null ? x.FiasAddress.Address.AddressName : string.Empty,
                    IsMatched = x.FiasAddress != null
                });
        }

        public void Save(List<NotMatchedAddressInfo> addresses, string filename, string importType)
        {
            var newCodes = addresses.Select(a => a.BillingId).ToList();
            var duplicates = ImportedAddressRepository.GetAll()
                    .Where(x => newCodes.Contains(x.AddressCode)).ToList();

            //убираем записи с повторяющимися BillingID & полями адреса
            addresses.RemoveAll(a => duplicates.Any(
                        d => d.AddressCode == a.BillingId && d.City == a.City && d.Street == a.Street &&
                            d.House == a.House));


            var entities = addresses.Select(x => new ImportedAddressMatch
            {
                ImportFilename = filename,
                ImportType = importType,
                AddressCode = x.BillingId,
                City = x.City, 
                Street = x.Street, 
                House = x.House,
                ImportDate = DateTime.Now.Date
            });
            
            TransactionHelper.InsertInManyTransactions(Container, entities, 1000, true, true);
        }


        #region private
        /// <summary>
        /// Список районов
        /// </summary>        
        private IEnumerable<AoProxy> GetAreaList()
        {            
            var regions =
                FiasRepository.GetAll()
                    .Where(x =>
                        x.AOLevel == FiasLevelEnum.Region
                        &&
                        x.ActStatus == FiasActualStatusEnum.Actual)
                    .Select(x => x.AOGuid)
                    .ToList();

            var data = FiasRepository
                .GetAll()
                .Where(x =>
                    regions.Contains(x.ParentGuid)
                    &&
                    (x.AOLevel == FiasLevelEnum.Raion || x.AOLevel == FiasLevelEnum.City)
                    &&
                    x.ActStatus == FiasActualStatusEnum.Actual
                )
                .Select(x => new AoProxy
                {
                    Id = x.AOGuid,
                    Name =
                        string.IsNullOrEmpty(x.ShortName)
                            ? x.FormalName
                            : string.Format("{0}. {1}", x.ShortName, x.FormalName)
                })
                .ToList();

            return data;
        }

        /// <summary>
        /// Cписок населенных пунктов 
        /// </summary>
        /// <param name="baseParams">список районов</param>
        /// <returns>Список населенных пунктов</returns>
        private IEnumerable<AoProxy> GetPlaceList(BaseParams baseParams)
        {
            var municipalIdList = baseParams.Params.GetAs<List<string>>("municipalIdList");

            var data = FiasRepository
                .GetAll()
                .Where(
                    x =>
                        municipalIdList.Contains(x.ParentGuid)
                        &&
                        (x.AOLevel == FiasLevelEnum.City || x.AOLevel == FiasLevelEnum.Place)
                        &&
                        x.ActStatus == FiasActualStatusEnum.Actual
                )
                .Select(x => new AoProxy
                {
                    Id = x.AOGuid,
                    Name =
                        string.IsNullOrEmpty(x.ShortName)
                            ? x.FormalName
                            : string.Format("{0}. {1}", x.ShortName, x.FormalName)
                })
                .ToList();

            return data;

        }

        /// <summary>
        /// Список улиц
        /// </summary>
        /// <param name="baseParams">список населенных пунктов</param>
        /// <returns>список улиц</returns>
        private IEnumerable<AoProxy> GetStreetList(BaseParams baseParams)
        {
            var placeIdList = baseParams.Params.GetAs<List<string>>("placeIdList");

            var data = FiasRepository
                .GetAll()
                .Where(
                    x =>
                        x.AOLevel == FiasLevelEnum.Street
                        &&
                        placeIdList.Contains(x.ParentGuid)
                        &&
                        x.ActStatus == FiasActualStatusEnum.Actual)
                .Select(x => new AoProxy
                {
                    Id = x.AOGuid,
                    Name = string.Format("{0}. {1}", x.ShortName, x.FormalName),
                    FormalName = x.FormalName,
                    ShortName = x.ShortName
                })
                .ToList();
            return data;
        }
        #endregion

    }

    /// <summary>
    /// Прокси клас для адресного объекта
    /// </summary>
    public class AoProxy
    {
        public string Id { set; get; }        
        public string Name { set; get; }
        public string FormalName { set; get; }

        public string ShortName { set; get; }
    }
}
