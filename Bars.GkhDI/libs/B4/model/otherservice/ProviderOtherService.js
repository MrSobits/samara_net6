Ext.define('B4.model.otherservice.ProviderOtherService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProviderOtherService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'OtherService', defaultValue: null },
        { name: 'Provider', defaultValue: null },
        { name: 'Name' },
        { name: 'DateStartContract', defaultValue: null },
        { name: 'Description' },
        { name: 'NumberContract' }
    ]
});