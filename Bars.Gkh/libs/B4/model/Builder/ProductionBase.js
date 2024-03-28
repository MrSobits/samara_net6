Ext.define('B4.model.builder.ProductionBase', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuilderProductionBase'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Builder', defaultValue: null },
        { name: 'KindEquipment', defaultValue: null },
        { name: 'Notation' },
        { name: 'Volume' },
        { name: 'DocumentRight', defaultValue: null }
    ]
});