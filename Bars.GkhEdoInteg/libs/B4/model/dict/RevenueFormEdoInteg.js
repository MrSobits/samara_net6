Ext.define('B4.model.dict.RevenueFormEdoInteg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RevenueFormCompareEdo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CompareId', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'CodeEdo' },
        { name: 'RevenueForm', defaultValue: null }
        
    ]
});