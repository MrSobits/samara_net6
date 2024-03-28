Ext.define('B4.model.realityobj.MissingCeo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectMissingCeo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject' },
        { name: 'MissingCommonEstateObject' }
    ]
});