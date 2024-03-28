Ext.define('B4.model.priorityparam.Quant', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'QuantPriorityParam'
    },
    fields: [
        { name: 'Id' },
        { name: 'MaxValue' },
        { name: 'MinValue' },
        { name: 'Point' },
        { name: 'Code' }
    ]
});