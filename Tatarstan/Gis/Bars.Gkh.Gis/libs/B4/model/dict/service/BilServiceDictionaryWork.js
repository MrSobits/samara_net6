Ext.define('B4.model.dict.service.BilServiceDictionaryWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BilServiceDictionary',
        listAction: 'ListServiceWork'
    },
    fields: [
        { name: 'Id' },
        { name: 'ServiceName' },
        { name: 'MeasureName' },
        { name: 'ServiceCode' }
    ]
});