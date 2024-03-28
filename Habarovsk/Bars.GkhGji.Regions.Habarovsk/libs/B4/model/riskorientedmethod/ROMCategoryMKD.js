Ext.define('B4.model.riskorientedmethod.ROMCategoryMKD', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ROMCategoryMKD'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ROMCategory' },
        { name: 'RealityObject' },
        { name: 'RealityObjectArea' },
        { name: 'Municipality' },
        { name: 'DateStart' },
    ]
});