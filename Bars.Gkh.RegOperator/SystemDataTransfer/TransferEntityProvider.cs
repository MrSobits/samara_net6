namespace Bars.Gkh.RegOperator.SystemDataTransfer
{
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.SystemDataTransfer.Meta;

    internal class TransferEntityProvider : ITransferEntityProvider
    {
        /// <inheritdoc />
        public void FillContainer(TransferEntityContainer container)
        {
            container.Add<ChargePeriod>("Периоды начислений").AddComparer(x => x.Name);
            container.Add<FederalStandardFeeCr>("Справочник \"Федеральный стандарт взноса на КР\"")
                .AddComparer(x => x.DateStart, x => x.DateEnd)
                .AddComparer(x => x.Value);

            container.Add<PersonalAccountOwner>("Абонент").IsBase();
            container.Add<LegalAccountOwner>("Абонент ЮЛ").HasBase<PersonalAccountOwner>()
                .IgnoreProperty(x => x.Name)
                .IgnoreProperty(x => x.Inn)
                .IgnoreProperty(x => x.Kpp)
                .IgnoreProperty(x => x.Address)
                .AddComparer(x => x.Contragent, x => x.TotalAccountsCount)
                .AddComparer(x => x.ObjectCreateDate)
                .AddComplexComparer<BasePersonalAccount, string>(x => (LegalAccountOwner)x.AccountOwner, x => x.PersonalAccountNum)
                .Filter(x => x.TotalAccountsCount > 0);

            container.Add<IndividualAccountOwner>("Абонент ФЛ").HasBase<PersonalAccountOwner>()
                .IgnoreProperty(x => x.Name)
                .AddComparer(x => x.FirstName, x => x.Surname)
                .AddComparer(x => x.SecondName, x => x.TotalAccountsCount)
                .AddComparer(x => x.ObjectCreateDate)
                .AddComplexComparer<BasePersonalAccount, string>(x => (IndividualAccountOwner)x.AccountOwner, x => x.PersonalAccountNum)
                .Filter(x => x.TotalAccountsCount > 0);

            container.Add<AccountOwnershipHistory>("История принадлежности лс абоненту")
                .AddComparer(x => x.AccountOwner, x => x.PersonalAccount)
                .AddComparer(x => x.Date);

            container.Add<BasePersonalAccount>("Лицевой счет").AddComparer(x => x.PersonalAccountNum);
            container.Add<PersonalAccountPeriodSummary>("Ситуация по ЛС на период").AddComparer(x => x.PersonalAccount, x => x.Period);
            container.Add<PersonalAccountChange>("Изменения ЛС")
                .AddComparer(x => x.PersonalAccount, x => x.ChargePeriod)
                .AddComparer(x => x.ActualFrom, x => x.ChangeType)
                .AddComparer(x => x.Date);

            container.Add<Wallet>("Кошелек").AddComparer(x => x.WalletGuid);
            container.Add<PrivilegedCategory>("Категория льгот").AddComparer(x => x.Code);

            container.Add<CashPaymentCenter>("Расчетно-кассовый центр").AddComparer(x => x.Contragent);
            container.Add<CashPaymentCenterMunicipality>("Связь расчетно-кассвого центра с МО")
                .AddComparer(x => x.CashPaymentCenter, x => x.Municipality);
            container.Add<CashPaymentCenterPersAcc>("Лицевой счет расчетно-кассового центра")
                .AddComparer(x => x.CashPaymentCenter, x => x.PersonalAccount).AddComparer(x => x.DateStart, x => x.DateEnd);
            container.Add<CashPaymentCenterRealObj>("Жилой дом расчетно-кассового центра")
                .AddComparer(x => x.CashPaymentCenter, x => x.RealityObject).AddComparer(x => x.DateStart, x => x.DateEnd);
            container.Add<CashPaymentCenterManOrg>("Обслуживаемая УК расчетно-кассового центра")
                .AddComparer(x => x.CashPaymentCenter, x => x.ManOrg).AddComparer(x => x.DateStart, x => x.DateEnd);
            container.Add<CashPaymentCenterManOrgRo>("Дом обслуживаемой УК расчетно-кассового центра")
                .AddComparer(x => x.CashPaymentCenterManOrg, x => x.RealityObject).AddComparer(x => x.DateStart, x => x.DateEnd);

            container.Add<RealityObjectPaymentAccount>("Счет оплат дома").AddComparer(x => x.RealityObject);
            container.Add<RealityObjectChargeAccount>("Счет начислений дома").AddComparer(x => x.RealityObject);
            container.Add<RealityObjectChargeAccountOperation>("Начисления по счету начисления дома (группировка по периодам)")
                .AddComparer(x => x.Account, x => x.Period);
            container.Add<RealityObjectSupplierAccount>("Счет расчета с поставщиками").AddComparer(x => x.RealityObject);
            container.Add<RealityObjectSupplierAccountOperation>("Операции счета по рассчета с поставщиками")
                .AddComparer(x => x.Account, x => x.OperationType)
                .AddComparer(x => x.Date);
            container.Add<RealityObjectSubsidyAccount>("Счет cубсидий дома").AddComparer(x => x.RealityObject);
            container.Add<RealityObjectSubsidyAccountOperation>("Фактическое поступление субсидий")
                .AddComparer(x => x.Account, x => x.OperationType)
                .AddComparer(x => x.Date, x => x.OperationSum);

            container.Add<SpecialCalcAccount>("Специальный расчетный счет").AddComparer(x => x.AccountNumber);
            container.Add<RegopCalcAccount>("Расчетный счет регоператора").AddComparer(x => x.AccountNumber);

            container.Add<GovDecision>("Протокол решения органов гос. власти")
                .AddComparer(x => x.RealityObject, x => x.ProtocolNumber)
                .AddComparer(x => x.DateStart, x => x.ProtocolDate);

            container.Add<RealityObjectDecisionProtocol>("Протокол решений собственников")
                .AddComparer(x => x.RealityObject, x => x.DocumentNum)
                .AddComparer(x => x.DateStart, x => x.ProtocolDate);

            container.Add<AccountManagementDecision>("Решение по ведение лицевого счета").AddComparer(x => x.Protocol);
            container.Add<AccountOwnerDecision>("Решение о владельце счета").AddComparer(x => x.Protocol);
            container.Add<AccumulationTransferDecision>(" Решение о переводе накоплений").AddComparer(x => x.Protocol);
            container.Add<CreditOrgDecision>("Решение о выборе кредитной организации").AddComparer(x => x.Protocol);
            container.Add<CrFundFormationDecision>("Решение о формировании фонда КР").AddComparer(x => x.Protocol);
            container.Add<GenericDecision>("Решение, принятое по протоколу").AddComparer(x => x.Protocol, x => x.DecisionCode);
            container.Add<JobYearDecision>("Решения по году работ").AddComparer(x => x.Protocol);
            container.Add<MinFundAmountDecision>("Размер минимального фонда на КР").AddComparer(x => x.Protocol);
            container.Add<MkdManagementDecision>("Решение о выборе управления").AddComparer(x => x.Protocol);
            container.Add<MonthlyFeeAmountDecHistory>("Размер ежемесячного взноса на КР").AddComparer(x => x.Protocol);
            container.Add<DecisionNotification>("Уведомление о решении").AddComparer(x => x.Protocol);

            container.Add<TariffApprovalDecisionProtocol>("Протокол об утверждение тарифа на содержание и ремонт жилья").AddComparer(x => x.RealityObject, x => x.ProtocolNumber).AddComparer(x => x.DateStart);
            container.Add<CrFundDecisionProtocol>("Протокол о формировании фонда капитального ремонта").AddComparer(x => x.RealityObject, x => x.ProtocolNumber).AddComparer(x => x.DateStart);
            container.Add<ManagementOrganizationDecisionProtocol>("Протокол о выборе управляющей компании для дома").AddComparer(x => x.RealityObject, x => x.ProtocolNumber).AddComparer(x => x.DateStart);
            container.Add<MkdManagementTypeDecisionProtocol>("Протокол о выборе формы управления многоквартирным домом").AddComparer(x => x.RealityObject, x => x.ProtocolNumber).AddComparer(x => x.DateStart);
            container.Add<OoiManagementDecisionProtocol>("Протокол по вопросам, связанных с эксплуатацией и управлением общим имуществом здания").AddComparer(x => x.RealityObject, x => x.ProtocolNumber).AddComparer(x => x.DateStart);
        }
    }
}