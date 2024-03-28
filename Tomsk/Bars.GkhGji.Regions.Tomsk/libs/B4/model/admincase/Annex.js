Ext.define('B4.model.admincase.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdministrativeCaseAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AdministrativeCase', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' }
    ]
});