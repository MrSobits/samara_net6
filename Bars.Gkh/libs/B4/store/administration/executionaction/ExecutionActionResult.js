Ext.define('B4.store.administration.executionaction.ExecutionActionResult', {
    extend: 'B4.base.Store',
    autoLoad: false,
    sorters: [
        {
            property: 'StartDate',
            direction: 'DESC'
        }
    ],
    sortOnLoad: true,
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'StartDate' },
        { name: 'EndDate' },
        { name: 'Status' },
        { name: 'Result' },
        { name: 'Duration' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ExecutionActionResult'
    }
});