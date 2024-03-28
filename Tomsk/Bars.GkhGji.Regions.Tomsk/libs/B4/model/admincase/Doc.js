Ext.define('B4.model.admincase.Doc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdministrativeCaseDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AdministrativeCase', defaultValue: null },
        { name: 'EntitiedInspector', defaultValue: null },
        { name: 'TypeAdminCaseDoc', defaultValue: 10 },
        { name: 'DocumentNumber' },
        { name: 'DocumentNum' },
        { name: 'DocumentDate' },
        { name: 'NeedTerm' },
        { name: 'RenewalTerm' },
        { name: 'DescriptionSet' }
    ]
});