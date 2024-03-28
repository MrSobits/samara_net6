Ext.define('B4.model.mkdlicrequest.Source', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MKDLicRequestSource'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MKDLicRequest', defaultValue: null },
        { name: 'RevenueSource', defaultValue: null },
        { name: 'RevenueForm', defaultValue: null },
        { name: 'RevenueSourceNumber' },
        { name: 'SSTUDate' },
        { name: 'RevenueDate' }
    ]
});