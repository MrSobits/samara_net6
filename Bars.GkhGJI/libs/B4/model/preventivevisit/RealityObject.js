Ext.define('B4.model.preventivevisit.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveVisitRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PreventiveVisit', defaultValue: null },
        { name: 'Municipality' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Address' }
    ]
});