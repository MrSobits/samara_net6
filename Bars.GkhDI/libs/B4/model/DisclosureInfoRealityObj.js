Ext.define('B4.model.DisclosureInfoRealityObj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisclosureInfoRealityObj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PeriodDI', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },

        //Общие сведения о доме
        { name: 'ClaimCompensationDamage', defaultValue: null },
        { name: 'ClaimReductionPaymentNonService', defaultValue: null },
        { name: 'ClaimReductionPaymentNonDelivery', defaultValue: null },
        { name: 'ExecutionWork' },
        { name: 'ExecutionObligation' },
        { name: 'DescriptionServiceCatalogRepair' },
        { name: 'DescriptionTariffCatalogRepair' },
        { name: 'FileExecutionWork', defaultValue: null },
        { name: 'FileExecutionObligation', defaultValue: null },
        { name: 'FileServiceCatalogRepair', defaultValue: null },
        { name: 'FileTariffCatalogRepair', defaultValue: null },
        

        //Фин показатели по ремонту содержанию дома
        { name: 'WorkRepair', defaultValue: null },
        { name: 'WorkLandscaping', defaultValue: null },
        { name: 'Subsidies', defaultValue: null },
        { name: 'Credit', defaultValue: null },
        { name: 'FinanceLeasingContract', defaultValue: null },
        { name: 'FinanceEnergServContract', defaultValue: null },
        { name: 'OccupantContribution', defaultValue: null },
        { name: 'OtherSource', defaultValue: null },

        { name: 'ReceivedPretensionCount' },
        { name: 'ApprovedPretensionCount' },
        { name: 'NoApprovedPretensionCount' },
        { name: 'PretensionRecalcSum' },

        { name: 'SentPretensionCount' },
        { name: 'SentPetitionCount' },
        { name: 'ReceiveSumByClaimWork' },

        { name: 'ReductionPayment', defaultValue: 30 },
        { name: 'NonResidentialPlacement', defaultValue: 30 },
        { name: 'PlaceGeneralUse', defaultValue: 30 },

        //Единичные поля над гридами разделов
        { name: 'ReductionPayment', defaultValue: 30 },
        { name: 'NonResidentialPlacement', defaultValue: 30 },
        { name: 'PlaceGeneralUse', defaultValue: 30 },
        { name: 'AdvancePayments', defaultValue: null }, // Авансовые платежи на старт периода
        { name: 'CarryOverFunds', defaultValue: null }, // переходящие остатки средств
        { name: 'Debt', defaultValue: null }, // задолженность на начало периода
        { name: 'ChargeForMaintenanceAndRepairsAll', defaultValue: null }, // начисление за содержание и текущий ремонт
        { name: 'ChargeForMaintenanceAndRepairsMaintanance', defaultValue: null },
        { name: 'ChargeForMaintenanceAndRepairsRepairs', defaultValue: null },
        { name: 'ChargeForMaintenanceAndRepairsManagement', defaultValue: null },
        { name: 'ReceivedCashAll', defaultValue: null }, // получено денежных средств
        { name: 'ReceivedCashFromOwners', defaultValue: null },
        { name: 'ReceivedCashFromOwnersTargeted', defaultValue: null },
        { name: 'ReceivedCashAsGrant', defaultValue: null },
        { name: 'ReceivedCashFromUsingCommonProperty', defaultValue: null },
        { name: 'ReceivedCashFromOtherTypeOfPayments', defaultValue: null },
        { name: 'CashBalanceAll', defaultValue: null },
        { name: 'CashBalanceAdvancePayments', defaultValue: null },
        { name: 'CashBalanceCarryOverFunds', defaultValue: null },
        { name: 'CashBalanceDebt', defaultValue: null },

        { name: 'ComServStartAdvancePay', defaultValue: null },
        { name: 'ComServStartCarryOverFunds', defaultValue: null },
        { name: 'ComServStartDebt', defaultValue: null },
        { name: 'ComServEndAdvancePay', defaultValue: null },
        { name: 'ComServEndCarryOverFunds', defaultValue: null },
        { name: 'ComServEndDebt', defaultValue: null },
        { name: 'ComServReceivedPretensionCount', defaultValue: null },
        { name: 'ComServApprovedPretensionCount', defaultValue: null },
        { name: 'ComServNoApprovedPretensionCount', defaultValue: null },
        { name: 'ComServPretensionRecalcSum', defaultValue: null }
    ]
});