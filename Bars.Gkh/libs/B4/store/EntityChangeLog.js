Ext.define('B4.store.EntityChangeLog', {
    extend: 'B4.base.Store',
    autoLoad: false,
    sortOnLoad: true,
    proxy: {
        type: 'b4proxy',
        controllerName: 'EntityChangeLog',
        listAction: 'list',
        createAction: '',
        getAction: '',
        updateAction: '',
        deleteAction: ''
    },
    fields: [
        { name: 'Id' },
        { name: 'User' },
        { name: 'ChangeDate' },
        { name: 'ActionKind' },
        { name: 'Entity' },
        { name: 'Property' },
        { name: 'OldValue' },
        { name: 'NewValue' }
    ],
    sorters: [
        {
            property: 'Id',
            direction: 'DESC'
        }
    ]
});