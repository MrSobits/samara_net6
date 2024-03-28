Ext.define('B4.model.manorglicense.RequestAnnex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgRequestAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'LicRequest' },
        { name: 'Name' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'Description' },
        { name: 'File', defaultValue: null }
    ]
});