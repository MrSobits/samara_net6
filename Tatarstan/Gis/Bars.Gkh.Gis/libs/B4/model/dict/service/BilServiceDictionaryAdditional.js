Ext.define('B4.model.dict.service.BilServiceDictionaryAdditional', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BilServiceDictionary',
        listAction: 'ListAdditionalService'
    },
    fields: [
        { name: 'Id' },
        { name: 'ServiceName' },
        { name: 'MeasureName' }
    ]
});