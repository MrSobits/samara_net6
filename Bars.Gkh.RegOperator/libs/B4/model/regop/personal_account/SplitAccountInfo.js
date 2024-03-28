Ext.define('B4.model.regop.personal_account.SplitAccountInfo', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'memory'
    },

    fields: [
        { name: 'Id' },
        { name: 'PersonalAccountNum' },
        { name: 'OpenDate' },
        { name: 'OwnerType' },
        { name: 'OwnerName' },
        { name: 'AccountOwner' },
        { name: 'AreaShare', defaultValue: 0 },
		{ name: 'NewAreaShare' },
		{ name: 'OwnershipType' },
		{ name: 'OwnershipTypeNewLs' },
		{ name: 'Room' },

        // ФЛ
        { name: 'FirstName' },
        { name: 'Surname' },
        { name: 'SecondName' },

        // ЮЛ
        { name: 'Contragent' },
        { name: 'Inn' },
        { name: 'Kpp' },

        // Задолженности
        { name: 'BaseTariffDebt', defaultValue: 0 },
        { name: 'DecisionTariffDebt', defaultValue: 0 },
        { name: 'PenaltyDebt', defaultValue: 0 },
        { name: 'NewBaseTariffDebt', defaultValue: 0 },
        { name: 'NewDecisionTariffDebt', defaultValue: 0 },
        { name: 'NewPenaltyDebt', defaultValue: 0 }
    ]
});