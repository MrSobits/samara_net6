Ext.define('B4.model.realityobj.StructuralElementWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElementWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Work', useNull: true, defaultValue: null },
        { name: 'StructuralElement', useNull: true, defaultValue: null },

        { name: 'LastOverhaulYear' },
        { name: 'VolumeRepair' },
        { name: 'TypeRepair' },
        { name: 'Description' }
    ]
});