Ext.define('B4.model.dict.BuildingFeature', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuildingFeature'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code'},
        { name: 'Name' }   
    ]
});