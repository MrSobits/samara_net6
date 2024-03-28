Ext.define('B4.model.administration.LogOperation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LogOperation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'User' },
        { name: 'StartDate' },
        { name: 'EndDate' },
        { name: 'Comment' },
        { name: 'LogFile' },
        { name: 'OperationType' }
    ]
});