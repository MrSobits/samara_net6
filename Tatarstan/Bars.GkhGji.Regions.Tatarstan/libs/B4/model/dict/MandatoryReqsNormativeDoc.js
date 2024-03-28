Ext.define('B4.model.dict.MandatoryReqsNormativeDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MandatoryReqsNormativeDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Npa', defaultValue: null },
    ]
});