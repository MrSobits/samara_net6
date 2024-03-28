Ext.define('B4.model.shortprogram.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ShortProgramRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address', defaultValue: null },
        { name: 'State', defaultValue: null }
    ]
});