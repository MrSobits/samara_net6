namespace Bars.Gkh.Services.Impl
{
    // using System.ServiceModel.Activation;

    using Bars.Gkh.Services.ServiceContracts;

    using Castle.Windsor;

    using Bars.Gkh.Entities;
    using Bars.B4;
    using Bars.Gkh.DomainService.TechPassport;
    using Bars.Gkh.Services.Override;

    using DomainService;

    // TODO wcf
    // [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        /// <summary>
        /// Контейнер 
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Доме сервис Жилой дом
        /// </summary>
        public IDomainService<RealityObject> RealObjDomainService { get; set; }

        /// <summary>
        /// Доме сервис Страховой полис МКД
        /// </summary>
        public IDomainService<BelayPolicyMkd> BelayPolicyMkdDomainService { get; set; }

        /// <summary>
        /// Доме сервис Жилой дом договора управляющей организации
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> MoContractRoDomainService { get; set; }

        /// <summary>
        /// Доме сервис Фото-архив жилого дома
        /// </summary>
        public IDomainService<RealityObjectImage> RoImageDomainService { get; set; }

        /// <summary>
        /// Доме сервис Договор непосредственного управления жилым домом
        /// </summary>
        public IDomainService<RealityObjectDirectManagContract> MoContractDirectManagService { get; set; }

        /// <summary>
        /// Доме сервис Помещение
        /// </summary>
        public IDomainService<Room> RoomDomainService { get; set; }

        /// <summary>
        /// Сервис для получение данных из Акта выполненных работ
        /// </summary>
        public IPerfomedWorkActIntegrationService PerfomedWorkActIntegrationService { get; set; }

        /// <summary>
        /// Сервис для получение количество собственников
        /// </summary>
        public IOwnersService OwnersService { get; set; }

        /// <summary>
        /// Интерфейс взаимодействия с тех паспортом для сервисов
        /// </summary>
        public ITehPassportValueService TehPassportValueService { get; set; }

        /// <summary>
        /// Сервис получения информации по домам
        /// </summary>
        public IServiceOverride ServiceOverride { get; set; }
    }
}