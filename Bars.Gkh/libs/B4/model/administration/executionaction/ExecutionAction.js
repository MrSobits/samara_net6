Ext.define('B4.model.administration.executionaction.ExecutionAction', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Description' },
    ],
    idProperty: 'Code'
});