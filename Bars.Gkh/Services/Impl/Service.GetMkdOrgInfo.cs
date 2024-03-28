namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.GetMkdOrgInfo;

    using PublicServiceOrg = Bars.Gkh.Services.DataContracts.GetMkdOrgInfo.PublicServiceOrg;
    using ServiceOrganization = Bars.Gkh.Services.DataContracts.GetMkdOrgInfo.ServiceOrganization;
    using SupplyResourceOrg = Bars.Gkh.Services.DataContracts.GetMkdOrgInfo.SupplyResourceOrg;

    public partial class Service
    {
        /// <summary>
        /// Менеджер файлов
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="RealityObject"/>
        /// </summary>
        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="ManOrgContractRealityObject"/>
        /// </summary>
        public IRepository<ManOrgContractRealityObject> ManOrgContractRealityObjectRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="RealityObjectResOrg"/>
        /// </summary>
        public IRepository<RealityObjectResOrg> RealityObjectResOrgRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="ServiceOrgRealityObjectContract"/>
        /// </summary>
        public IRepository<ServiceOrgRealityObjectContract> ServiceOrgRealityObjectContractRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="PublicServiceOrgContractRealObj"/>
        /// </summary>
        public IRepository<PublicServiceOrgContractRealObj> PublicServiceOrgContractRealObjRepository { get; set; }

        /// <summary>
        /// Получить сведения об обслуживающих МКД организациях
        /// </summary>
        /// <param name="roId">Идентификатор жилого дома</param>
        /// <returns>Результат выполнения запроса</returns>
        public GetMkdOrgInfoResponse GetMkdOrgInfo(string roId)
        {
            var result = Result.NoErrors;

            var roIdLong = roId.ToLong();

            var realityObject = this.RealityObjectRepository.Get(roIdLong);
            if (realityObject == null)
            {
                result = Result.DataNotFound;
                result.Message = "По введенному параметру дом не найден";

                return new GetMkdOrgInfoResponse {Result = result};
            }

            return new GetMkdOrgInfoResponse
            {
                Result = result,
                ManagementOrganization = this.GetManOrgContractInfo(roIdLong),
                SupplyResourceOrgs = this.GetResOrgsInfo(roIdLong),
                ServiceOrganizations = this.GetServOrgsInfo(roIdLong),
                PublicServiceOrgs = this.GetPublicServOrgsInfo(roIdLong)
            };
        }

        private ManagementOrganization GetManOrgContractInfo(long roId)
        {
            var manOrgContractInfo = this.ManOrgContractRealityObjectRepository.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate > DateTime.Now)
                .OrderByDescending(x => x.ManOrgContract.DocumentDate)
                .Select(x => new
                    {
                        x.ManOrgContract.Id,
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.DocumentNumber,
                        ContragentName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                        ContragentShortName = x.ManOrgContract.ManagingOrganization.Contragent.ShortName,
                        ContragentOrganizationForm = x.ManOrgContract.ManagingOrganization.Contragent.OrganizationForm.Name,
                        ContragentFactAddress = x.ManOrgContract.ManagingOrganization.Contragent.FactAddress,
                        ContragentTimeZoneType = x.ManOrgContract.ManagingOrganization.Contragent.TimeZoneType,
                        ContragentOkfs = x.ManOrgContract.ManagingOrganization.Contragent.Okfs,
                        x.ManOrgContract.FileInfo
                    })
                .AsEnumerable()
                .Select(x => new ManagementOrganization
                    {
                        Id = x.Id,
                        Name = x.ContragentName,
                        Shortname = x.ContragentShortName,
                        LegalForm = x.ContragentOrganizationForm,
                        FactAddr = x.ContragentFactAddress,
                        TimeZone = x.ContragentTimeZoneType != null ? x.ContragentTimeZoneType.Value.GetDisplayName() : string.Empty,
                        Okfs = x.ContragentOkfs.ToStr(),
                        Duid = x.Id,
                        DateDu = x.StartDate != null ? x.StartDate.Value.ToShortDateString() : string.Empty,
                        NumDu = x.DocumentNumber,
                        FileDuid = x.FileInfo != null ? x.FileInfo.Id : 0,
                        FileDu = x.FileInfo != null ? this.FileManager.GetBase64String(x.FileInfo) : string.Empty,
                        FileExt = x.FileInfo != null ? x.FileInfo.Extention : string.Empty,
                        FileName = x.FileInfo != null ? x.FileInfo.Name : string.Empty
                    })
                .FirstOrDefault();

            return manOrgContractInfo;
        }

        private SupplyResourceOrg[] GetResOrgsInfo(long roId)
        {
            var resOrgsInfo = this.RealityObjectResOrgRepository.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Where(x => x.DateEnd == null || x.DateEnd > DateTime.Now)
                .Select(x => new
                    {
                        x.Id,
                        x.DateStart,
                        x.ContractNumber,
                        ContragentName = x.ResourceOrg.Contragent.Name,
                        ContragentShortName = x.ResourceOrg.Contragent.ShortName,
                        ContragentOrganizationForm = x.ResourceOrg.Contragent.OrganizationForm.Name,
                        ContragentFactAddress = x.ResourceOrg.Contragent.FactAddress,
                        ContragentTimeZoneType = x.ResourceOrg.Contragent.TimeZoneType,
                        ContragentOkfs = x.ResourceOrg.Contragent.Okfs,
                        x.FileInfo
                    })
                .AsEnumerable()
                .Select(x => new SupplyResourceOrg
                    {
                        Id = x.Id,
                        Name = x.ContragentName,
                        Shortname = x.ContragentShortName,
                        LegalForm = x.ContragentOrganizationForm,
                        FactAddr = x.ContragentFactAddress,
                        TimeZone = x.ContragentTimeZoneType != null ? x.ContragentTimeZoneType.Value.GetDisplayName() : string.Empty,
                        Okfs = x.ContragentOkfs.ToStr(),
                        ContractId = x.Id,
                        Date = x.DateStart != null ? x.DateStart.Value.ToShortDateString() : string.Empty,
                        Num = x.ContractNumber,
                        FileId = x.FileInfo != null ? x.FileInfo.Id : 0,
                        File = x.FileInfo != null ? this.FileManager.GetBase64String(x.FileInfo) : string.Empty,
                        FileExt = x.FileInfo != null ? x.FileInfo.Extention : string.Empty,
                        FileName = x.FileInfo != null ? x.FileInfo.Name : string.Empty
                    })
                .ToArray();

            return resOrgsInfo;
        }

        private ServiceOrganization[] GetServOrgsInfo(long roId)
        {
            var servOrgsInfo = this.ServiceOrgRealityObjectContractRepository.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Where(x => x.ServOrgContract.DateEnd == null || x.ServOrgContract.DateEnd > DateTime.Now)
                .Select(x => new
                    {
                        x.ServOrgContract.Id,
                        x.ServOrgContract.DateStart,
                        x.ServOrgContract.DocumentNumber,
                        ContragentName = x.ServOrgContract.ServOrg.Contragent.Name,
                        ContragentShortName = x.ServOrgContract.ServOrg.Contragent.ShortName,
                        ContragentOrganizationForm = x.ServOrgContract.ServOrg.Contragent.OrganizationForm.Name,
                        ContragentFactAddress = x.ServOrgContract.ServOrg.Contragent.FactAddress,
                        ContragentTimeZoneType = x.ServOrgContract.ServOrg.Contragent.TimeZoneType,
                        ContragentOkfs = x.ServOrgContract.ServOrg.Contragent.Okfs,
                        x.ServOrgContract.FileInfo
                    })
                .AsEnumerable()
                .Select(x => new ServiceOrganization
                    {
                        Id = x.Id,
                        Name = x.ContragentName,
                        Shortname = x.ContragentShortName,
                        LegalForm = x.ContragentOrganizationForm,
                        FactAddr = x.ContragentFactAddress,
                        TimeZone = x.ContragentTimeZoneType != null ? x.ContragentTimeZoneType.Value.GetDisplayName() : string.Empty,
                        Okfs = x.ContragentOkfs.ToStr(),
                        ContractId = x.Id,
                        Date = x.DateStart != null ? x.DateStart.Value.ToShortDateString() : string.Empty,
                        Num = x.DocumentNumber,
                        FileId = x.FileInfo != null ? x.FileInfo.Id : 0,
                        File = x.FileInfo != null ? this.FileManager.GetBase64String(x.FileInfo) : string.Empty,
                        FileExt = x.FileInfo != null ? x.FileInfo.Extention : string.Empty,
                        FileName = x.FileInfo != null ? x.FileInfo.Name : string.Empty
                    })
                .ToArray();

            return servOrgsInfo;
        }

        private PublicServiceOrg[] GetPublicServOrgsInfo(long roId)
        {
            var publicServOrgsInfo = this.PublicServiceOrgContractRealObjRepository.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Where(x => x.RsoContract.DateEnd == null || x.RsoContract.DateEnd > DateTime.Now)
                .Select(x => new
                    {
                        x.RsoContract.Id,
                        x.RsoContract.DateStart,
                        x.RsoContract.ContractNumber,
                        ContragentName = x.RsoContract.PublicServiceOrg.Contragent.Name,
                        ContragentShortName = x.RsoContract.PublicServiceOrg.Contragent.ShortName,
                        ContragentOrganizationForm = x.RsoContract.PublicServiceOrg.Contragent.OrganizationForm.Name,
                        ContragentFactAddress = x.RsoContract.PublicServiceOrg.Contragent.FactAddress,
                        ContragentTimeZoneType = x.RsoContract.PublicServiceOrg.Contragent.TimeZoneType,
                        ContragentOkfs = x.RsoContract.PublicServiceOrg.Contragent.Okfs,
                        x.RsoContract.FileInfo
                    })
                .AsEnumerable()
                .Select(x => new PublicServiceOrg
                    {
                        Id = x.Id,
                        Name = x.ContragentName,
                        Shortname = x.ContragentShortName,
                        LegalForm = x.ContragentOrganizationForm,
                        FactAddr = x.ContragentFactAddress,
                        TimeZone = x.ContragentTimeZoneType != null ? x.ContragentTimeZoneType.Value.GetDisplayName() : string.Empty,
                        Okfs = x.ContragentOkfs.ToStr(),
                        ContractId = x.Id,
                        Date = x.DateStart != null ? x.DateStart.Value.ToShortDateString() : string.Empty,
                        Num = x.ContractNumber,
                        FileId = x.FileInfo != null ? x.FileInfo.Id : 0,
                        File = x.FileInfo != null ? this.FileManager.GetBase64String(x.FileInfo) : string.Empty,
                        FileExt = x.FileInfo != null ? x.FileInfo.Extention : string.Empty,
                        FileName = x.FileInfo != null ? x.FileInfo.Name : string.Empty
                    })
                .ToArray();

            return publicServOrgsInfo;
        }
    }
}