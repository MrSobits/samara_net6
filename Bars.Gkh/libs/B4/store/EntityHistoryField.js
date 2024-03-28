Ext.define('B4.store.EntityHistoryField', {
    extend: 'B4.base.Store',
    autoLoad: false,
    sortOnLoad: true,
    proxy: {
        type: 'b4proxy',
        controllerName: 'EntityHistoryField',
        listAction: 'list',
        createAction: '',
        getAction: '',
        updateAction: '',
        deleteAction: ''
    },
    fields: [
        { name: 'Id' },
        { name: 'EntityHistoryInfo' },
        { name: 'FieldName' },
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