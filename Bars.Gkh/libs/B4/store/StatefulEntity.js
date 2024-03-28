Ext.define('B4.store.StatefulEntity', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'State',
        listAction: 'FiltredStatefulEntityList'
    },
    fields: [
        { name: 'TypeId' },
        { name: 'Name' }
    ]
});