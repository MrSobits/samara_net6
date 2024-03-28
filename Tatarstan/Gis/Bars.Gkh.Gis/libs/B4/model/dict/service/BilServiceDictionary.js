Ext.define('B4.model.dict.service.BilServiceDictionary', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BilServiceDictionary'
    },
    fields: [
        { name: 'Id' },
        { name: 'DataBank' },
        { name: 'Organization' },
        { name: 'Service' },
        { name: 'ServiceName' },
        { name: 'ServiceCode' },
        { name: 'ServiceTypeName' },
        { name: 'RelatedService' }
    ]
});