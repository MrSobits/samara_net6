Ext.define('B4.model.account.Accruals', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AccrualsAccount'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Number' },
        { name: 'OpenDate' },
        { name: 'CloseDate' },
        { name: 'TotalIncome' },
        { name: 'TotalOut' },
        { name: 'Balance' },
        { name: 'LastOperationDate' },
        { name: 'OpeningBalance' }
    ]
});