namespace Bars.Gkh.SystemDataTransfer
{
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.SystemDataTransfer.Meta;
    using Bars.Gkh.SystemDataTransfer.Meta.Serialization;

    internal class TransferEntityProvider : ITransferEntityProvider
    {
        /// <inheritdoc />
        public void FillContainer(TransferEntityContainer container)
        {
            container.Add<Inspector>("Инспектор").AddComparer(x => x.Code, x => x.Fio);
            container.Add<Role>("Роли").AddComparer(x => x.Name);
            container.Add<State>("Статусы").AddComparer(x => x.Name, x => x.TypeId).AsPartially();
            container.Add<FileInfo>("Файл").AsPartially().AddCustomSerializer<FileInfoSerializer>();
            container.Add<Fias>("ФИАС").AddComparer(x => x.AOGuid, x => x.ActStatus).AddComparer(x => x.KladrCurrStatus, x => x.AOLevel);
            container.Add<Municipality>("Муниципальное образование").AddComparer(x => x.Okato, x => x.Oktmo).AddComparer(x => x.FiasId, x => x.Code);
            container.Add<FiasAddress>("Адрес ФИАС").AsPartially().AlwaysCreateNew();

            container.Add<MultipurposeGlossary>("Универсальные справочники").AddComparer(x => x.Code);
            container.Add<MultipurposeGlossaryItem>("Элемент универсального справочника").AddComparer(x => x.Glossary, x => x.Key);

            container.Add<CapitalGroup>("Группа капитальности").AddComparer(x => x.Name);
            container.Add<RoofingMaterial>("Материал кровли").AddComparer(x => x.Name);
            container.Add<WallMaterial>("Материал стен").AddComparer(x => x.Name);
            container.Add<TypeOwnership>("Форма собственности").AddComparer(x => x.Name);
            container.Add<RealEstateType>("Тип дома").AddComparer(x => x.Code);
            container.Add<CentralHeatingStation>("ЦТП").AddComparer(x => x.Name);
            container.Add<RealityObject>("Жилой дом").AddComparer(x => x.Address);
            container.Add<Entrance>("Подъезд").AddComparer(x => x.RealityObject, x => x.Number);
            container.Add<Room>("Помещение").AddComparer(x => x.RealityObject, x => x.RoomNum).AddComparer(x => x.ChamberNum);
            container.Add<TypeProject>("Тип проекта").AddComparer(x => x.Name, x => x.Code);

            container.Add<UnitMeasure>("Единицы измерения").AddComparer(x => x.Name);
            container.Add<MeteringDevice>("Прибор учета").AddComparer(x => x.Name, x => x.AccuracyClass);
            container.Add<RealityObjectImage>("Фото-архив жилого дома").AddComparer(x => x.Name, x => x.RealityObject);
            container.Add<RealityObjectMeteringDevice>("Прибор учета дома").AddComparer(x => x.MeteringDevice, x => x.RealityObject).AddComparer(x => x.ObjectCreateDate);

            container.Add<Period>("Период").AddComparer(x => x.Name);
            container.Add<Work>("Работа").AddComparer(x => x.Code);
            container.Add<TehPassport>("ТехПаспорт").AddComparer(x => x.RealityObject);
            container.Add<TehPassportValue>("Значение ТехПаспорта").AddComparer(x => x.TehPassport, x => x.CellCode).AddComparer(x => x.FormCode);

            container.Add<WorkKindCurrentRepair>("Виды работ текущего ремонта").AddComparer(x => x.Code);
            container.Add<BuildingFeature>("Особые признаки строения").AddComparer(x => x.Code, x => x.Name);
            container.Add<RealityObjectBuildingFeature>("Особые признаки дома").AddComparer(x => x.BuildingFeature, x => x.RealityObject);

            container.Add<StopReason>("Причина расторжения договора").AddComparer(x => x.Code);
            container.Add<ResettlementProgramSource>("Источники по программе переселения").AddComparer(x => x.Code);

            container.Add<Position>("Должность").AddComparer(x => x.Code);
            container.Add<OrganizationForm>("Организационно правовая форма").AddComparer(x => x.OkopfCode, x => x.Code).AddComparer(x => x.Name); // TODO: поправить данные!
            container.Add<Contragent>("Контрагенты").AddComparer(x => x.Inn, x => x.Kpp).AddComparer(x => x.Parent, x => x.OrganizationForm);
            container.Add<ContragentContact>("Контактная информация по контрагенту").AddComparer(x => x.Contragent, x => x.FullName);

            container.Add<ManagingOrganization>("Управляющие организации").AddComparer(x => x.Contragent);
            container.Add<ManagingOrgMunicipality>("Муниципальные образования УО").AddComparer(x => x.ManOrg, x => x.Municipality);
            container.Add<ManagingOrgRealityObject>("Дома УО").AddComparer(x => x.RealityObject, x => x.ManagingOrganization);
            container.Add<ManOrgContractRelation>("Связи договоров управления").AddComparer(x => x.Children, x => x.Parent).AddComparer(x => x.TypeRelation);
            container.Add<ManOrgContractRealityObject>("Связь договора управления с домом").AddComparer(x => x.ManOrgContract, x => x.RealityObject);
            container.Add<ManagingOrgDocumentation>("Организационно-техническая документация (Управляющие организации)").AddComparer(x => x.ManagingOrganization, x => x.DocumentName).AddComparer(x => x.DocumentNum);
            container.Add<ManagingOrgWorkMode>("Режим работы управляющей организации").AddComparer(x => x.ManagingOrganization, x => x.TypeMode).AddComparer(x => x.TypeDayOfWeek);
            container.Add<ManagingOrgMembership>("Членство в объединениях").AddComparer(x => x.ManagingOrganization, x => x.Name).AddComparer(x => x.DateStart);
            container.Add<ManOrgBaseContract>("Базовый контракт").IsBase();

            container.Add<ManOrgContractOwners>("Управление домом, договор УК с собственниками").HasBase<ManOrgBaseContract>().AddComparer(x => x.ManagingOrganization).AddComparer(x => x.StartDate, x => x.EndDate)
                .AddComplexComparer<ManOrgContractRealityObject, RealityObject>(x => (ManOrgContractOwners)x.ManOrgContract, x => x.RealityObject);

            container.Add<ManOrgJskTsjContract>("Управление домами (ТСЖ / ЖСК)").HasBase<ManOrgBaseContract>().AddComparer(x => x.ManagingOrganization).AddComparer(x => x.StartDate, x => x.EndDate)
                .AddComplexComparer<ManOrgContractRealityObject, RealityObject>(x => (ManOrgJskTsjContract)x.ManOrgContract, x => x.RealityObject);
            
            container.Add<ManOrgContractTransfer>("Управление домами, договор УК с ЖСК/ТСЖ").HasBase<ManOrgBaseContract>().AddComparer(x => x.ManagingOrganization).AddComparer(x => x.StartDate, x => x.EndDate)
                .AddComplexComparer<ManOrgContractRealityObject, RealityObject>(x => (ManOrgContractTransfer)x.ManOrgContract, x => x.RealityObject);

            container.Add<RealityObjectDirectManagContract>("Договор непосредственного управления жилым домом").HasBase<ManOrgBaseContract>().AddComparer(x => x.ManagingOrganization).AddComparer(x => x.StartDate, x => x.EndDate)
                .AddComplexComparer<ManOrgContractRealityObject, RealityObject>(x => (RealityObjectDirectManagContract)x.ManOrgContract, x => x.RealityObject);

            container.Add<TypeService>("Тип обслуживания").AddComparer(x => x.Name);

            container.Add<SupplyResourceOrg>("Поставщики коммунальных услуг").AddComparer(x => x.Contragent);
            container.Add<SupplyResourceOrgMunicipality>("Связь Поставщика коммунальных услуг с МО").AddComparer(x => x.SupplyResourceOrg, x => x.Municipality);
            container.Add<SupplyResourceOrgRealtyObject>("Связь Поставщика коммунальных услуг с Жилым домом").AddComparer(x => x.SupplyResourceOrg, x => x.RealityObject);
            container.Add<SupplyResourceOrgDocumentation>("Организационно-техническая документация (Поставщики коммунальных услуг)")
                .AddComparer(x => x.SupplyResourceOrg, x => x.DocumentName).AddComparer(x => x.DocumentNum).AddComparer(x => x.DocumentDate);
            container.Add<SupplyResourceOrgService>("Услуга Поставщика коммунальных услуг").AddComparer(x => x.SupplyResourceOrg, x => x.TypeService);

            container.Add<ServiceOrganization>("Поставщики жилищных услуг").AddComparer(x => x.Contragent);
            container.Add<ServiceOrgMunicipality>("Связь поставщика жил. услуг с МО").AddComparer(x => x.ServOrg, x => x.Municipality);
            container.Add<ServiceOrgRealityObject>("Жилой дом организации поставщика жил. услуг").AddComparer(x => x.ServiceOrg, x => x.RealityObject);
            container.Add<ServiceOrgRealityObjectContract>("Жилой дом договора организации поставщика жил. услуг").AddComparer(x => x.RealityObject, x => x.ServOrgContract);
            container.Add<ServiceOrgContract>("Базовый класс договоров управления (Поставщики жилищных услуг)")
                .AddComparer(x => x.ServOrg, x => x.DateStart)
                .AddComplexComparer<ServiceOrgRealityObjectContract, RealityObject>(x => x.ServOrgContract, x => x.RealityObject);
            container.Add<ServiceOrgDocumentation>("Организационно-техническая документация (Поставщики коммунальных услуг)")
                .AddComparer(x => x.ServiceOrganization, x=> x.DocumentName).AddComparer(x => x.DocumentNum).AddComparer(x => x.DocumentDate);
            container.Add<ServiceOrgService>("Услуга обслуживающей организации").AddComparer(x => x.ServiceOrganization, x => x.TypeService);

            container.Add<Builder>("Подрядные организации").AddComparer(x => x.Contragent);
            container.Add<BuilderDocumentType>("Справочник \"Документы подрядных организаций\"").AddComparer(x => x.Name);
            container.Add<BuilderDocument>("Документы подрядчиков").AddComparer(x => x.Contragent, x => x.Builder).AddComparer(x => x.BuilderDocumentType);
            container.Add<BuilderFeedback>("Отзывы заказчиков о подрядчиках").AddComparer(x => x.TypeAssessment, x => x.Builder).AddComparer(x => x.TypeAuthor);
            container.Add<BuilderLoan>("Займ подрядчика").AddComparer(x => x.Builder, x => x.Lender).AddComparer(x => x.DocumentName, x => x.Amount);
            container.Add<BuilderLoanRepayment>("График погашения займа подрядчика").AddComparer(x => x.BuilderLoan, x => x.Name).AddComparer(x => x.RepaymentAmount);
            container.Add<KindEquipment>("Вид оснащения").AddComparer(x => x.Name);
            container.Add<BuilderProductionBase>("Производственная база подрядчика").AddComparer(x => x.Builder, x => x.KindEquipment);
            container.Add<BuilderSroInfo>("Сведения об участии в СРО (Подрядные организации)").AddComparer(x => x.Builder, x => x.Work);
            container.Add<BuilderTechnique>("Техника, Инструменты подрядчика").AddComparer(x => x.Builder, x => x.Name);
            container.Add<BuilderWorkforce>("Состав трудовых ресурсов (Подрядные организации)")
                .AddComparer(x => x.Builder, x => x.DocumentNum).AddComparer(x => x.Specialty, x => x.Institutions);
            container.Add<Specialty>("Специальность").AddComparer(x => x.Name);
            container.Add<Institutions>("Учебное заведение").AddComparer(x => x.Name);
        }
    }
}