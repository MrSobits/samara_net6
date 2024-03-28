namespace Bars.Gkh.Reforma.Domain.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.Gkh.Reforma.Utils;

    using Castle.Windsor;

    /// <summary>
    ///     Сервис жилых домов
    /// </summary>
    public class RobjectService : IRobjectService
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public RobjectService(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Ищет жилые дома по адресу Реформы
        /// </summary>
        /// <param name="address">Адрес Реформы</param>
        /// <returns>Жилые дома</returns>
        public IQueryable<RealityObject> FindRobjects(FullAddress address)
        {
            var placeGuidAliases = AddressUtils.GetGuidAliases(this.container, address.city1_guid, address.city2_guid, address.city3_guid);
            var streetGuidAliases = AddressUtils.GetGuidAliases(this.container, address.street_guid);

            // нужны все дома, без фильтрации по оператору
            var repo = this.container.Resolve<IRepository<RealityObject>>();
            try
            {
                var buildingIsEmpty = address.building.IsEmpty();
                var blockIsEmpty = address.block.IsEmpty();
                return
                    repo.GetAll()
                        .Where(x => placeGuidAliases.Contains(x.FiasAddress.PlaceGuidId) && streetGuidAliases.Contains(x.FiasAddress.StreetGuidId))
                        .Where(x => x.FiasAddress.House == address.house_number)
                        .WhereIf(buildingIsEmpty, x => x.FiasAddress.Building == string.Empty || x.FiasAddress.Building == null)
                        .WhereIf(!buildingIsEmpty, x => x.FiasAddress.Building.ToLower() == address.building.ToLower())
                        .WhereIf(blockIsEmpty, x => x.FiasAddress.Housing == string.Empty || x.FiasAddress.Housing == null)
                        .WhereIf(!blockIsEmpty, x => x.FiasAddress.Housing.ToLower() == address.block.ToLower());
            }
            finally
            {
                this.container.Release(repo);
            }
        }

        /// <summary>
        ///     Получает историю управления жилым домом
        /// </summary>
        /// <param name="robjectId">Идентификатор жилого дома</param>
        /// <returns>История управления</returns>
        public ManagingHistory[] GetManagingHistory(long robjectId)
        {
            var service = this.container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var ownerContractsDomain = this.container.Resolve<IDomainService<ManOrgContractOwners>>();
            try
            {
                return
                    service.GetAll()
                        .Where(x => x.RealityObject.Id == robjectId)
                        .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                        .AsEnumerable()
                        .Select(
                            x =>
                                new ManagingHistory
                                {
                                    Id = x.ManOrgContract.Id,
                                    ManOrgId = x.ManOrgContract.ManagingOrganization.Id,
                                    ManOrgInn = x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                                    DateStart = x.ManOrgContract.StartDate,
                                    DateEnd = x.ManOrgContract.EndDate,
                                    PlannedDateEnd = x.ManOrgContract.PlannedEndDate,
                                    TerminateReason = x.ManOrgContract.TerminateReason,
                                    Note = x.ManOrgContract.Note,
                                    DocumentName = x.ManOrgContract.DocumentName,
                                    TypeManagement = x.ManOrgContract.TypeContractManOrgRealObj,
                                    ContractFoundation = ownerContractsDomain.Get(x.ManOrgContract.Id)?.ContractFoundation ?? 0,
                                    DocumentNumber = x.ManOrgContract.DocumentNumber,
                                    DocumentDate = x.ManOrgContract.DocumentDate,
                                    ContractStopReason = x.ManOrgContract.ContractStopReason,
                                    DocumentFile = x.ManOrgContract.FileInfo
                                })
                        .OrderBy(x => x.DateStart)
                        .ThenBy(x => x.DateEnd)
                        .ToArray();
            }
            finally
            {
                this.container.Release(service);
                this.container.Release(ownerContractsDomain);
            }
        }

        /// <summary>
        ///     Получает информацию об обслуживаемых домах УО
        /// </summary>
        /// <param name="inn">ИНН УО</param>
        /// <returns>Информация об обслуживаемых домах</returns>
        public RobjectManagement[] GetManagingRobjects(string inn)
        {
            var robjectContractService = this.container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var baseContractService = this.container.ResolveDomain<ManOrgBaseContract>();
            try
            {
                var robjects =
                    robjectContractService.GetAll()
                        .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Inn == inn)
                        .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                        .Where(x => x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                        .Select(x => new { ContractId = x.ManOrgContract.Id, RobjectId = x.RealityObject.Id })
                        .ToArray();

                return
                    baseContractService.GetAll()
                        .Where(x => x.ManagingOrganization.Contragent.Inn == inn)
                        .Where(x => x.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                        .Select(x => new { x.Id, x.StartDate, x.EndDate, x.PlannedEndDate, x.TypeContractManOrgRealObj, x.DocumentName, x.TerminateReason, x.Note, x.ManagingOrganization.TypeManagement, x.ContractStopReason})
                        .AsEnumerable()
                        .SelectMany(
                            x => robjects.Where(y => y.ContractId == x.Id).Select(y => y.RobjectId),
                            (contract, robjectId) =>
                            new RobjectManagement
                                {
                                    Id = contract.Id,
                                    DateStart = contract.StartDate,
                                    DateEnd = contract.EndDate,
                                    PlannedDateEnd = contract.PlannedEndDate,
                                    TerminateReason = contract.TerminateReason,
                                    DocumentName = contract.DocumentName,
                                    RobjectId = robjectId,
                                    Note = contract.Note,
                                    TypeManagement = contract.TypeManagement,
                                    ContractStopReason = contract.ContractStopReason,
                                    TypeContractManOrgRealObj = contract.TypeContractManOrgRealObj
                                })
                        .ToArray();
            }
            finally
            {
                this.container.Release(robjectContractService);
                this.container.Release(baseContractService);
            }
        }

        #endregion
    }
}