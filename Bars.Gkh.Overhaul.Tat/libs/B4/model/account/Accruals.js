Ext.define('B4.model.account.Accruals', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AccrualsAccount'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Number' },
        { name: 'OpenDate' },
        { name: 'CloseDate' },
        { name: 'TotalDebit' },
        { name: 'TotalCredit' },
        { name: 'Balance' },
        { name: 'LastOperationDate' },
        { name: 'OpeningBalance' },
        { name: 'RealityObject', defaultValue: null }
    ]
});