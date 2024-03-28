Ext.define('B4.model.regop.personal_account.PersonalAccountCalcDebt', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountCalcDebt'
    },

    fields: [
        { name: 'Id' },
        { name: 'PersonalAccount' },
        { name: 'PreviousOwner' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'AgreementNumber' },
        { name: 'Document' }
    ]
});