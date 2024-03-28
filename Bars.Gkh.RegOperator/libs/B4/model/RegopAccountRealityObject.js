Ext.define('B4.model.RegopAccountRealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CalcAccountRealityObject',
        listAction: 'ListForRegop'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'AccountNumber' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});