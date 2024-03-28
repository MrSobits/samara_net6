Ext.define('B4.model.dict.service.BilServiceDictionaryCommunal', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BilServiceDictionary',
        listAction: 'ListCommunalService'
    },
    fields: [
        { name: 'Id' },
        { name: 'ServiceName' },
        { name: 'IsOdnService' },
        { name: 'OrderNumber' }
    ]
});