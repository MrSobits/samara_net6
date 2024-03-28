Ext.define('B4.model.HouseParam', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseParam'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'ValPrm' }
    ]
});