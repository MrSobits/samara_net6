Ext.define('B4.model.manorg.Documentation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManagingOrgDocumentation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'Description' },
        { name: 'DocumentName' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'FileName' },
        { name: 'File', defaultValue: null }
    ]
});