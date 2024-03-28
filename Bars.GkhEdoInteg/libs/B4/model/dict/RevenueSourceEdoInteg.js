Ext.define('B4.model.dict.RevenueSourceEdoInteg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RevenueSourceCompareEdo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'CodeEdo' },
        { name: 'RevenueSource', defaultValue: null }
    ]
});