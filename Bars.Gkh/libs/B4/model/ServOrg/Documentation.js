Ext.define('B4.model.servorg.Documentation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ServiceOrgDocumentation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ServiceOrganization', defaultValue: null },
        { name: 'Description' },
        { name: 'DocumentName' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'File', defaultValue: null }
    ]
});