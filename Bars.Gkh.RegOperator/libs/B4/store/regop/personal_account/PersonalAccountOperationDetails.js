Ext.define('B4.store.regop.personal_account.PersonalAccountOperationDetails', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: ['Id', 'OperationDate', 'OperationName',  'SaldoChange', 'Guid', 'Period' ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonAccountDetalization',
        listAction: 'ListOperationDetails'
    }
});