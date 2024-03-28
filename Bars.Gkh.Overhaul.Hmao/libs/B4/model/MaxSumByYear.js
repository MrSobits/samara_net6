Ext.define('B4.model.MaxSumByYear', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MaxSumByYear'
    },
    fields: [
        { name: 'Municipality' },
        { name: 'Program' },
        { name: 'Year' },
        { name: 'Sum' },
    ]
});