Ext.define('B4.model.calcaccount.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CalcAccountRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Account' },
        { name: 'RealityObject' },
        { name: 'StartDate' },
        { name: 'EndDate' },
        { name: 'Address' }
    ]
});