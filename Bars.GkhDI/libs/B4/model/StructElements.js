Ext.define('B4.model.StructElements', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StructElements'
    },
    fields: [
        { name: 'Id', useNull: true },

        { name: 'BasementType', defaultValue: null },
        { name: 'BasementArea', defaultValue: null },

        { name: 'TypeFloor', defaultValue: null },
        { name: 'TypeWalls', defaultValue: null },

        { name: 'ConstructionChute', defaultValue: null },
        { name: 'ChutesNumber', defaultValue: null }
    ]
});