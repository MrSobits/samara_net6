Ext.define('B4.model.realityobj.Block', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectBlock'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Number' },
        { name: 'AreaLiving' },
        { name: 'AreaTotal' },
        { name: 'CadastralNumber' }
    ]
});