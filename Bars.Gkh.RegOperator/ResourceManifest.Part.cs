namespace Bars.Gkh.RegOperator
{
    using System;
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.ClaimWork.Dto;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enum;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Imports.Ches;
    using Bars.Gkh.RegOperator.Imports.Ches.File;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Entity;
    using Bars.Gkh.Utils;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            ResourceManifest.RegisterModels(container);
            ResourceManifest.RegisterEnums(container);
            ResourceManifest.RegisterServices(container);
        }

        private static void RegisterModels(IResourceManifestContainer container)
        {
            container.RegisterExtJsModel<ComputingProcess>()
                .Controller("ComputingProcess")
                .AddProperty<object>("Data");

            container.RegisterExtJsModel<ChesNotMatchAddress>("import.chesimport.ChesNotMatchAddress").Controller("ChesNotMatchAddress");

            container.RegisterExtJsModel<ChesMatchAccountOwner>("import.chesimport.ChesMatchAccountOwner").Controller("ChesMatchAccountOwner");
            container.RegisterExtJsModel<ChesMatchLegalAccountOwner>("import.chesimport.ChesMatchLegalAccountOwner")
                .Controller("ChesMatchLegalAccountOwner")
                .Extend("B4.model.import.chesimport.ChesMatchAccountOwner")
                .AddProperty<string>("ExternalName")
                .AddProperty<string>("ExternalInn")
                .AddProperty<string>("ExternalKpp");

            container.RegisterExtJsModel<ChesMatchIndAccountOwner>("import.chesimport.ChesMatchIndAccountOwner")
                .Controller("ChesMatchIndAccountOwner")
                .Extend("B4.model.import.chesimport.ChesMatchAccountOwner")
                .AddProperty<string>("ExternalName")
                .AddProperty<string>("ExternalAccountNumber");

            container.RegisterExtJsModel<ChesNotMatchAccountOwner>("import.chesimport.ChesNotMatchAccountOwner").Controller("ChesNotMatchAccountOwner");
            container.RegisterExtJsModel<ChesNotMatchLegalAccountOwner>("import.chesimport.ChesNotMatchLegalAccountOwner").Controller("ChesNotMatchLegalAccountOwner").Extend("B4.model.import.chesimport.ChesNotMatchAccountOwner");
            container.RegisterExtJsModel<ChesNotMatchIndAccountOwner>("import.chesimport.ChesNotMatchIndAccountOwner").Controller("ChesNotMatchIndAccountOwner").Extend("B4.model.import.chesimport.ChesNotMatchAccountOwner");

            container.RegisterExtJsModel<RealityObjectChargeAccount>()
                .Controller("RealityObjectChargeAccount")
                .AddProperty<string>("AccountNum")
                .AddProperty<string>("BankAccountNum")
                .AddProperty<decimal>("ChargeLastPeriod")
                .AddProperty<DateTime>("DateLastOperation")
                .AddProperty<decimal>("PaymentLastPeriod");

            container.RegisterExtJsModel<RealityObjectChargeAccountOperation>().Controller("RealityObjectChargeAccountOperation");
            container.RegisterExtJsModel<RealityObjectPaymentAccount>()
                .Controller("RealityObjectPaymentAccount")
                .AddProperty<string>("AccountNum")
                .AddProperty<string>("Municipality")
                .AddProperty<string>("BankAccountNumber")
                .AddProperty<CrFundFormationType>("CrFundType");

            container.RegisterExtJsModel<RealityObjectPaymentAccountOperation>().Controller("RealityObjectPaymentAccountOperation");
            container.RegisterExtJsModel<RealityObjectSupplierAccount>().Controller("RealityObjectSupplierAccount");
            container.RegisterExtJsModel<RealityObjectSupplierAccountOperation>().Controller("RealityObjectSupplierAccountOperation").AddProperty<string>("WorkName");
            container.RegisterExtJsModel<LocationCode>().Controller("LocationCode");
            container.RegisterExtJsModel<PersonalAccountPeriodSummary>().Controller("PersonalAccountPeriodSummary");
            container.RegisterExtJsModel<PersonalAccountChange>().Controller("PersonalAccountChange");
            container.RegisterExtJsModel<Room>().Controller("Room").AddProperty<string>("AccountsNum").SetModelValue("proxy.timeout", 60000 * 5);
            container.RegisterExtJsModel<RealityObjectSubsidyAccountOperation>().Controller("RealityObjectSubsidyAccountOperation");
            container.RegisterExtJsModel<ProgramCrType>().Controller("ProgramCrType");
            container.RegisterExtJsModel<PersonalAccountOperationLogEntry>("PersonalAccountOperationLog")
                .Controller("BasePersonalAccount")
                .ListAction("GetOperationLog");

            container.RegisterExtJsModel<PaymentOrderDetail>()
                .Controller("PaymentOrderDetail")
                .AddProperty<long>("WalletId")
                .AddProperty<string>("WalletName")
                .AddProperty<string>("WalletGuid")
                .AddProperty<decimal>("Balance")
                .IdProperty("WalletGuid");

            container.RegisterExtJsModel<ViewDebtor>().Controller("JurJournalDebtor").ListAction("List");

            container.RegisterExtJsModel<SubsidyIncome>().Controller("SubsidyIncome");
            container.RegisterExtJsModel<SubsidyIncomeDetail>()
                .Controller("SubsidyIncomeDetail")
                .AddProperty<string>("Municipality")
                .AddProperty<string>("Address")
                .AddProperty<string>("PayAccNum")
                .AddProperty<string>("SubsidyDistrName")
                .AddProperty<string>("ConfirmStatus")
                .AddProperty<bool>("IsDefined");

            #region Payment doc cache
            
            container.RegisterExtJsModel<AccountPaymentInfoSnapshot>().Controller("AccountPaymentInfoSnapshot");

            #endregion

            container.RegisterExtJsModel<RestructDebtScheduleParam>("claimwork.restructdebt.ScheduleParam")
                .Controller("RestructDebtSchedule")
                .UpdateAction("CreateRestructSchedule")
                .ReadAction(string.Empty)
                .DestroyAction(string.Empty)
                .ListAction(string.Empty);

            container.RegisterExtJsModel<ViewDocumentClw>("claimwork.DocumentRegister")
                .Controller("ViewDocumentClw")
                .UpdateAction(string.Empty)
                .ReadAction(string.Empty)
                .DestroyAction(string.Empty);
        }

        private static void RegisterEnums(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/regop/PersonalAccountOwnerType.js",
                new ExtJsEnumResource<PersonalAccountOwnerType>("B4.enums.regop.PersonalAccountOwnerType"));
            container.Add("libs/B4/enums/regop/TypeCalcAccount.js",
                new ExtJsEnumResource<TypeCalcAccount>("B4.enums.regop.TypeCalcAccount"));
            container.Add("libs/B4/enums/regop/IdentityType.js",
                new ExtJsEnumResource<IdentityType>("B4.enums.regop.IdentityType"));
            container.Add("libs/B4/enums/regop/ComputingProcessType.js",
                new ExtJsEnumResource<ComputingProcessType>("B4.enums.regop.ComputingProcessType"));
            container.Add("libs/B4/enums/regop/ComputingProcessStatus.js",
                new ExtJsEnumResource<ComputingProcessStatus>("B4.enums.regop.ComputingProcessStatus"));
            container.Add("libs/B4/enums/regop/LoanFormationType.js",
                new ExtJsEnumResource<LoanFormationType>("B4.enums.regop.LoanFormationType"));
            container.Add("libs/B4/enums/regop/PaymentOperationType.js",
                new ExtJsEnumResource<PaymentOperationType>("B4.enums.regop.PaymentOperationType"));
            container.Add("libs/B4/enums/regop/OperationStatus.js",
                new ExtJsEnumResource<OperationStatus>("B4.enums.regop.OperationStatus"));
            container.Add("libs/B4/enums/regop/CachPaymentCenterConnectionType.js",
                new ExtJsEnumResource<CachPaymentCenterConnectionType>("B4.enums.regop.CachPaymentCenterConnectionType"));
            container.Add("libs/B4/enums/regop/SplitAccountDistributionType.js",
                new ExtJsEnumResource<SplitAccountDistributionType>("B4.enums.regop.SplitAccountDistributionType"));
            container.Add("libs/B4/enums/regop/DistributionCode.js",
                new ExtJsEnumResource<DistributionCode>("B4.enums.regop.DistributionCode"));
            container.Add("libs/B4/enums/regop/FileType.js",
                new ExtJsEnumResource<FileType>("B4.enums.regop.FileType"));

            container.Add("libs/B4/enums/regop/UsageRealEstateType.js",
                new ExtJsEnumResource<UsageRealEstateType>("B4.enums.regop.UsageRealEstateType"));
            container.Add("libs/B4/enums/claimwork/RepaymentType.js",
                new ExtJsEnumResource<RepaymentType>("B4.enums.claimwork.RepaymentType"));
            container.Add("libs/B4/enums/LawSuitDebtWorkType.js",
                new ExtJsEnumResource<LawSuitDebtWorkType>("B4.enums.LawSuitDebtWorkType"));

            container.RegisterGkhEnum("regop.WalletType", WalletType.Unknown);
            container.RegisterExtJsEnum<SuspenseAccountTypePayment>();
            container.RegisterExtJsEnum<SuspenseAccountStatus>();
            container.RegisterExtJsEnum<BankDocumentImportStatus>();
            container.RegisterExtJsEnum<BankDocumentImportCheckState>();
            container.RegisterExtJsEnum<PaymentType>();
            container.RegisterExtJsEnum<TypeLoanProcess>();
            container.RegisterExtJsEnum<StatusPaymentDocumentHousesType>();
            container.RegisterExtJsEnum<PersAccServiceType>();
            container.RegisterExtJsEnum<BenefitsCategoryImportIdentificationType>();
            container.RegisterExtJsEnum<DistributionState>();
            container.RegisterExtJsEnum<DebtorLogicalOperands>();
            container.RegisterExtJsEnum<RestructAmicAgrPaymentState>();

            container.RegisterExtJsEnum<SuspenseAccountDistributionParametersView>();
            container.RegisterExtJsEnum<SuspenseAccountDistributionParametersType>();
            container.RegisterExtJsEnum<PersonalAccountChangeType>();
            container.RegisterExtJsEnum<AccountManagementType>();
            container.RegisterExtJsEnum<MoneyDirection>();
            container.RegisterExtJsEnum<SuspenseActPaymentType>();
            container.RegisterExtJsEnum<ImportedPaymentState>();
            container.RegisterExtJsEnum<MobileAccountComparsionDecision>();

            container.RegisterExtJsEnum<ImportedPaymentType>();
            container.RegisterExtJsEnum<PaymentOrChargePacketState>();
            container.RegisterExtJsEnum<FundFormationContractType>();
            container.RegisterExtJsEnum<RegopServiceMethodType>();
            container.RegisterExtJsEnum<TypeAccountNumber>();
            container.RegisterExtJsEnum<TypeSourceLoan>();
            container.RegisterExtJsEnum<PaymentDocumentFormat>();
            container.RegisterExtJsEnum<CoreDecisionType>();
            container.RegisterExtJsEnum<KindPayment>();
            container.RegisterExtJsEnum<PrintType>();
            container.RegisterExtJsEnum<PrintMonth>();
            container.RegisterExtJsEnum<DebtSumType>();
            container.RegisterExtJsEnum<RecipientType>();
            container.RegisterExtJsEnum<FundFormationType>();
            container.RegisterExtJsEnum<DistributeOn>();
            container.RegisterExtJsEnum<SubsidyIncomeDefineType>();
            container.RegisterExtJsEnum<SubsidyIncomeStatus>();
            container.RegisterExtJsEnum<TypeCalculationNds>();
            container.RegisterExtJsEnum<TypeTransferSource>();
            container.RegisterExtJsEnum<PersonalAccountDeterminationState>();
            container.RegisterExtJsEnum<PaymentConfirmationState>();
            container.RegisterExtJsEnum<ImportedPaymentPaymentConfirmState>();
            container.RegisterExtJsEnum<ImportedPaymentPersAccDeterminateState>();
            container.RegisterExtJsEnum<PerformedWorkFundsDistributionType>();
            container.RegisterExtJsEnum<PeriodCloseCheckStateType>();
            container.RegisterExtJsEnum<AccountFilterHasCharges>();
            container.RegisterExtJsEnum<PerfWorkChargeType>();
            container.RegisterExtJsEnum<BanRecalcType>();
            container.RegisterExtJsEnum<TariffSourceType>();
            container.RegisterExtJsEnum<ChesImportState>();
            container.RegisterExtJsEnum<ChesAnalysisState>();
            container.RegisterExtJsEnum<PaymentDocumentPaymentState>();
            container.RegisterExtJsEnum<AccountRegistryMode>();
            container.RegisterExtJsEnum<PaymentDocumentSendingEmailState>();
            container.RegisterExtJsEnum<PersonalAccountNotDeterminationStateReason>();
            container.RegisterExtJsEnum<SaldoChangeOperationType>();
            container.RegisterExtJsEnum<SaldoChangeSaldoFromType>();
            container.RegisterExtJsEnum<SaldoChangeSaldoToType>();
            container.RegisterExtJsEnum<ImportPaymentType>();
            container.RegisterExtJsEnum<ChesImportPaymentsState>();
            container.RegisterExtJsEnum<DebtorState>();
            container.RegisterGkhEnum(CheckState.None);
        }

        private static void RegisterServices(IResourceManifestContainer container)
        {
            container.Add("services/personalaccount.svc", "Bars.Gkh.RegOperator.dll/Bars.Gkh.RegOperator.Wcf.PersonalAccountService.svc");
            container.Add("WS/ClientBankService.svc", "Bars.Gkh.RegOperator.dll/Bars.Gkh.RegOperator.Wcf.ClientBankService.svc");

            container.Add("WS/RegOpService.svc", "Bars.Gkh.RegOperator.dll/Bars.Gkh.RegOperator.Services.Service.svc");
        }
    }
}