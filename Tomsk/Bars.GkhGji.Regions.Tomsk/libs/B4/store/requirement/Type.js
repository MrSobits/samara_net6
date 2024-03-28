Ext.define('B4.store.requirement.Type', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: ['Value', 'Display'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeRequirement',
        listAction: 'GetItemsByDoc'
    }
});