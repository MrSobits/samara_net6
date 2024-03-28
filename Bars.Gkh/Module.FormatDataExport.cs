namespace Bars.Gkh
{
    using System.Collections.Generic;

    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.Domain.Impl;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.ExportableFile;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Impl;
    using Bars.Gkh.FormatDataExport.FormatProvider;
    using Bars.Gkh.FormatDataExport.FormatProvider.Converter;
    using Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat;
    using Bars.Gkh.FormatDataExport.NetworkWorker;
    using Bars.Gkh.FormatDataExport.NetworkWorker.Impl;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.FormatDataExport.ProxySelectors.Impl;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    using Bars.Gkh.FormatDataExport.Scheduler;
    using Bars.Gkh.FormatDataExport.Scheduler.Impl;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel.Registration;

    public partial class Module
    {
        private void RegisterFormatDataExport()
        {
            this.Container.RegisterTransient<IExportFormatProviderBuilder, ExportFormatProviderBuilder>();
            this.Container.Register(Component.For<CsvFormatProvider>().LifestyleTransient());
            this.Container.Register(Component.For<NetCsvFormatProvider>().LifestyleTransient());
            this.Container.Register(Component.For<FormatDataExportJob>().LifestyleTransient());

            this.Container.RegisterTransient<IExportFormatConverter, ExportFormatConverter>();
            this.Container.RegisterTransient<IFormatDataTransferService, FormatDataTransferService>();
            this.Container.RegisterTransient<IProxySelectorFactory, ProxySelectorFactory>();
            this.Container.RegisterTransient<IFormatDataExportFilterService, FormatDataExportFilterService>();
            this.Container.RegisterTransient<IFormatDataExportSchedulerService, FormatDataExportSchedulerService>();
            this.Container.RegisterTransient<IFormatDataExportIncrementalService, FormatDataExportIncrementalService>();

            this.Container.RegisterService<IExportableEntityResolver, ExportableEntityResolver>();
            this.Container.RegisterService<IFormatDataExportRoleService, FormatDataExportRoleService>();

            ContainerHelper.RegisterFileInfoExportableEntity<FilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<InfoExportableEntity>(this.Container);

            ContainerHelper.RegisterExportableEntity<ContragentAreaExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<ContragentExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DictMeasureExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DictUslugaExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<HouseExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoAddressExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoObjectExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoObjectQualityExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoObjectOtherQualityExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoUslugaResExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DuExportableEntity>(this.Container);
            //ContainerHelper.RegisterExportableEntity<DuChargeExportableEntity>(this.Container);
            //ContainerHelper.RegisterExportableEntity<DuChargeProtExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DuFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DuOuExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<EntranceExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<KapremDecisionsExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<KapremDecisionsFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<KvarOpenReasonExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<LiftExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<NpaExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<OgvExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<OmsExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<PoiExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<PremisesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<RsoExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<SotrudExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<UoExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<UstavExportableEntity>(this.Container);
            //ContainerHelper.RegisterExportableEntity<UstavChargeExportableEntity>(this.Container);
            //ContainerHelper.RegisterExportableEntity<UstavChargeFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<UstavFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<UstavOuExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<WorkTimeExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<ContragentRoleExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DuvotproFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<HouseDocExportableEntity>(this.Container);

            ContainerHelper.RegisterProxySelectorService<ActualManOrgByRealityObject, ActualManOrgByRealityObjectSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<RealityObjectByContract, RealityObjectByContractSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<HouseProxy, HouseSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DictMeasureProxy, DictMeasureSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DrsoAddressProxy, DrsoAddressSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DrsoObjectProxy, DrsoObjectSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DrsoObjectQualityProxy, DrsoObjectQualitySelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DuProxy, DuSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<UoProxy, UoSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<UstavProxy, UstavSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DictUslugaProxy, DictUslugaSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<PremisesProxy, PremisesSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<EntranceProxy, EntranceSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<HouseDocProxy, HouseDocSelectorService>(this.Container);

            ContainerHelper.RegisterFormatDataExportRepository<RealityObject, FormatDataExportRealityObjectRepository>();
            ContainerHelper.RegisterFormatDataExportRepository<ManOrgBaseContract, FormatDataExportManOrgBaseContractRepository>();

            this.RegisterEntityGroups();
        }

        private void RegisterEntityGroups()
        {
            this.RegisterEntityGroup("DuEntityGroup",
                "Договоры управления",
                new List<string>
                {
                    "DU",
                    "DUFILES",
                    "DUVOTPROFILES",
                    "DUOU",
                    "CONTRAGENTROLE",
                },
                FormatDataExportType.Du);

            this.RegisterEntityGroup("UoEntityGroup",
                "Управляющие организации",
                new List<string>
                {
                    "UO"
                },
                FormatDataExportType.Du);

            this.RegisterEntityGroup("UstavEntityGroup",
                "Уставы",
                new List<string>
                {
                    "USTAV",
                    "USTAVFILES",
                    "USTAVOU"
                },
                FormatDataExportType.Ustav);

            this.RegisterEntityGroup("BankEntityGroup",
                "Банки",
                new List<string>
                {
                    "BANK"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("OplataEntityGroup",
                "Извещения о принятии к исполнению распоряжений",
                new List<string>
                {
                    "KVAR",
                    "KVARACCOM",
                    "KVISOL",
                    "KVISOLSERVICE",
                    "OPLATA",
                    "OPLATAPACK"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("EpdEntityGroup",
                "Начисления / ЕПД",
                new List<string>
                {
                    "EPD",
                    "EPDCAPITAL"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("NpaEntityGroup",
                "Основание открытия лицевых счетов",
                new List<string>
                {
                    "KVAR",
                    "KVAROPENREASON",
                    "VOTPROCONT",
                    "REGOPSCHET",
                    "REGOP",
                    "REGOPSCHET"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("ProtocolossEntityGroup",
                "Протоколы общего собрания собственников",
                new List<string>
                {
                    "PROTOCOLOSS",
                    "VOTPROCONT",
                    "PROTOCOLOSSFILES",
                    "SOLUTIONOSS"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("ContragentRschetEntityGroup",
                "Расчетные счета",
                new List<string>
                {
                    "CONTRAGENTRSCHET"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("RegOpAccountsEntityGroup",
                "Расчетные счета регионального оператора",
                new List<string>
                {
                    "REGOPSCHET",
                    "KAPREMDECISIONS",
                    "KAPREMDECISIONSFILES"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("RegOpEntityGroup",
                "Региональные операторы капитального ремонта",
                new List<string>
                {
                    "REGOP"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("KapremDecisionsEntityGroup",
                "Решения о выборе способа формирования фонда капитального ремонта",
                new List<string>
                {
                    "KAPREMDECISIONS",
                    "KAPREMDECISIONSFILES"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("KvarEntityGroup",
                "Сведения о лицевых счетах",
                new List<string>
                {
                    "KVAR",
                    "KVARACCOM"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("IndEntityGroup",
                "Физические лица",
                new List<string>
                {
                    "IND"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("KvaraccomEntityGroup",
                "Связь помещений плательщиков с лицевыми счетами",
                new List<string>
                {
                    "KVARACCOM"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("SpecialAccountsEntityGroup",
                "Специальные счета капитального ремонта",
                new List<string>
                {
                    "REGOPSCHET"
                },
                FormatDataExportType.Regop);

            this.RegisterEntityGroup("ActWorkDogovEntityGroup",
                "Акты выполненных работ по договору на выполнение работ (оказание услуг) по капитальному ремонту",
                new List<string>
                {
                    "ACTWORKDOGOV",
                    "ACTWORK",
                    "ACTWORKDOGOVFILES"
                },
                FormatDataExportType.Pkr);

            this.RegisterEntityGroup("DogovorPkrEntityGroup",
                "Договоры на выполнение работ (оказание услуг) по капитальному ремонту",
                new List<string>
                {
                    "DOGOVORPKR",
                    "DOGOVORPKRFILES",
                    "WORKDOGOV",
                    "PAYDOGOV",
                    "PAYDOGOVWORK"
                },
                FormatDataExportType.Pkr);

            this.RegisterEntityGroup("CreditContractEntityGroup",
                "Кредитные договоры",
                new List<string>
                {
                    "CREDITCONTRACT",
                    "CREDITCONTRACTFILES"
                },
                FormatDataExportType.Pkr);

            this.RegisterEntityGroup("KprEntityGroup",
                "Программы капитального ремонта",
                new List<string>
                {
                    "WORKKPRTYPE",
                    "PKR",
                    "PKRDOC",
                    "PKRDOM",
                    "PKRDOMWORK"
                },
                FormatDataExportType.Pkr);

            this.RegisterEntityGroup("CrFundSizeEntityGroup",
                "Размер фонда капитального ремонта",
                new List<string>
                {
                    "CRFUNDSIZE",
                    "CRFUNDSIZEPREMISES"
                },
                FormatDataExportType.Pkr);

            this.RegisterEntityGroup("AuditPlanEntityGroup",
                "Планы проверок",
                new List<string>
                {
                    "AUDIT",
                    "AUDITPLAN",
                    "AUDITOBJECT",
                    "AUDITPLACE",
                    "AUDITEVENT",
                    "GJI"
                },
                FormatDataExportType.Gji);

            this.RegisterEntityGroup("AuditEntityGroup",
                "Проверки",
                new List<string>
                {
                    "AUDIT",
                    "AUDITOBJECT",
                    "AUDITEVENT",
                    "AUDITPLACE",
                    "AUDITFILES",
                    "AUDITRESULTFILES",
                    "AUDITRESULT",
                    "GJI",
                    "PRECEPTHOUSE",
                    "PRECEPTAUDIT",
                    "PRECEPTFILES",
                    "PROTOCOLAUDIT",
                    "PROTOCOLFILES"
                },
                FormatDataExportType.Gji);

            this.RegisterEntityGroup("HouseEntityGroup",
                "Дома",
                new List<string>
                {
                    "HOUSE",
                    "HOUSEDOC"
                },
                FormatDataExportType.Tp);
        }

        private void RegisterEntityGroup(string code, string description, IList<string> inheritedEtities, FormatDataExportProviderFlags allowProviderFlags)
        {
            this.Container.Register(Component.For<IExportableEntityGroup>()
                .ImplementedBy<ExportableEntityGroup>()
                .UsingFactoryMethod(() => new ExportableEntityGroup(code, description, inheritedEtities, allowProviderFlags))
                .LifestyleSingleton()
                .Named(code));
        }

        private void RegisterEntityGroup(string code, string description, IList<string> inheritedEtities, FormatDataExportType exportType)
        {
            this.Container.Register(Component.For<IExportableEntityGroup>()
                .ImplementedBy<ExportableEntityGroup>()
                .UsingFactoryMethod(() => new ExportableEntityGroup(code, description, inheritedEtities, exportType))
                .LifestyleSingleton()
                .Named(code));
        }
    }
}