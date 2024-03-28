Ext.define('B4.model.appealcits.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsRealObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'RealityObjectId' },
        { name: 'Municipality' },
        { name: 'Address' }
    ]
});