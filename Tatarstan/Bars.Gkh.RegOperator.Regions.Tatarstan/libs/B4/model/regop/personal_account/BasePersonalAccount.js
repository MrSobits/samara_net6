﻿Ext.define('B4.model.regop.personal_account.BasePersonalAccount', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'Room' },
        { name: 'State' },
        { name: 'Municipality' },
        { name: 'Settlement' },
        { name: 'AccountOwner' },
        { name: 'PersonalAccountNum' },
        { name: 'AreaShare' },
        { name: 'ChargedSum' },
        { name: 'PaidSum' },
        { name: 'PenaltySum' },
        { name: 'RealityObject' },
        { name: 'OpenDate' },
        { name: 'CloseDate' },
        { name: 'Tariff' },
        { name: 'RoomNum' },
        { name: 'Rooms' },
        { name: 'ContractNumber' },
        { name: 'ContractDate' },
        { name: 'ContractSendDate' },
        { name: 'ContractDocument' },
        { name: 'OwnershipDocumentType' },
        { name: 'DocumentNumber' },
        { name: 'DocumentRegistrationDate' },
        { name: 'RoomId' },
        { name: 'Summary' },
        { name: 'HasCharges' },
        { name: 'AccuralByOwnersDecision' },
        { name: 'PersAccNumExternalSystems' },
        { name: 'OwnerType' },
        { name: 'OwnerName' },
        { name: 'RoomAddress' },
        { name: 'RoomArea' },
        { name: 'SaldoIn' },
        { name: 'ContributionsInArrearsTariff' },
        { name: 'OwnershipType' },
        { name: 'ServiceType' },
        { name: 'ChargeBalance' },
        { name: 'ChargedBaseTariff' },
        { name: 'ChargedDecisionTariff' },
        { name: 'ChargedPenalty' },
        { name: 'TotalCharge' },
        { name: 'PaymentBaseTariff' },
        { name: 'PaymentDecisionTariff' },
        { name: 'PaymentPenalty' },
        { name: 'TotalPayment' },
        { name: 'DebtBaseTariff' },
        { name: 'DebtDecisionTariff' },
        { name: 'DebtPenalty' },
        { name: 'TotalDebt' },
        { name: 'CashPayCenter' },
        { name: 'PrivilegedCategoryPercent' },
        { name: 'RealArea' },
        { name: 'ChamberNum' },
        { name: 'FiasAddress' },

        { name: 'SaldoOut' },
        { name: 'CreditedWithPenalty' },
        { name: 'PaidWithPenalty' },
        { name: 'RecalculationWithPenalty' },
        { name: 'AccountFormVariant' },
        { name: 'PrivilegedCategory' },

        { name: 'PerfWorkChargeBalance' },

        { name: 'RoPayAccountNum' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePersonalAccount',
        timeout: 3 * 60 * 1000 //3 минуты
    }
});