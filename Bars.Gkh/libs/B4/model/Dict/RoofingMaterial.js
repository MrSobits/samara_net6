Ext.define('B4.model.dict.RoofingMaterial', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'roofingmaterial'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});