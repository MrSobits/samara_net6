Ext.define('B4.model.service.ProviderService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProviderService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseService', defaultValue: null },
        { name: 'Provider', defaultValue: null },
        { name: 'Name' },
        { name: 'DateStartContract', defaultValue: null },
        { name: 'Description' },
        { name: 'IsActive', defaultValue: true },
        { name: 'NumberContract' }
    ]
});