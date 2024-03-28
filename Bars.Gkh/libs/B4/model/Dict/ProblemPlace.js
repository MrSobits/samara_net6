Ext.define('B4.model.dict.ProblemPlace', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProblemPlace'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});