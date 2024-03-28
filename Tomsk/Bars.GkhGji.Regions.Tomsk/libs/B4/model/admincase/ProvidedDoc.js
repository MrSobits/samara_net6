Ext.define('B4.model.admincase.ProvidedDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdministrativeCaseProvidedDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AdministrativeCase', defaultValue: null },
        { name: 'ProvidedDoc', defaultValue: null }
    ]
});