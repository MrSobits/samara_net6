Ext.define('B4.model.realityobj.RealityObjectCategoryMKD', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectCategoryMKD'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'CategoryCSMKD', defaultValue: null  }
    ]
});