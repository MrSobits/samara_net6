Ext.define('B4.model.resolpros.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolProsRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ResolPros', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'RealityObjectId' },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'Area' }
    ]
});