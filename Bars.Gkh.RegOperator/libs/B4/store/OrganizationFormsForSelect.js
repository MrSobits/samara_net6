Ext.define('B4.store.OrganizationFormsForSelect', {
    extend: 'B4.store.State',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'organizationForm',
        listAction: 'List'
    }
});