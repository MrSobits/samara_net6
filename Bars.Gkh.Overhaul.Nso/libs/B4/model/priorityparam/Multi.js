Ext.define('B4.model.priorityparam.Multi', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MultiPriorityParam'
    },
    fields: [
        { name: 'Id' },
        { name: 'Code' },
        { name: 'Value' },
        { name: 'Point' },
        { name: 'StoredValues' },
        { name: 'Names' },
        { name: 'Codes' }
    ]
});