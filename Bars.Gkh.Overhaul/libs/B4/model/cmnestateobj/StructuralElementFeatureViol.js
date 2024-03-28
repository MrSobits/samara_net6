Ext.define('B4.model.cmnestateobj.StructuralElementFeatureViol', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StructuralElementFeatureViol'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'FeatureViol', defaultValue: null },
        { name: 'StructuralElement', defaultValue: null },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'GkhReformCode' }
    ]
});