Ext.define('B4.model.importexport.UnloadCounterValues', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'UnloadCounterValues',
        listAction: 'GetList',
        timeout: 9999999
    },
    fields: [
        { name: 'FormationDate' },
        { name: 'Month' },
        { name: 'User' },
        { name: 'OrganizationName' },
        { name: 'OrganizationCode' },
        { name: 'TypeStatus' },
        { name: 'File' },
        { name: 'Log' }
    ]
});