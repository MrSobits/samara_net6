Ext.define('B4.model.dict.PaymentDocInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentDocInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'MoSettlement' },
        { name: 'LocalityAoGuid' },
        { name: 'LocalityName' },
        { name: 'RealityObject' },
        { name: 'FundFormationType' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'Information' },
        { name: 'IsForRegion' }
    ]
});