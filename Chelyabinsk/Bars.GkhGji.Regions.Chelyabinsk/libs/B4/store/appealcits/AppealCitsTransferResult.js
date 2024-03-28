Ext.define('B4.store.appealcits.AppealCitsTransferResult', {
    extend: 'B4.base.Store',
    autoLoad: false,

    idProperty: 'Id',

    fields: [
        { name: 'Id' },
        { name: 'Type' },
        { name: 'AppealCitsId' },
        { name: 'AppealCitsNumber' },
        { name: 'StartDate', useNull: true },
        { name: 'EndDate', useNull: true },
        { name: 'Status' },
        { name: 'User', useNull: true },
        { name: 'LogFile', useNull: true }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsTransferResult'
    },
});