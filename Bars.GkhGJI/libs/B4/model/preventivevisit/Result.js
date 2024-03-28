Ext.define('B4.model.preventivevisit.Result', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveVisitResult'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PreventiveVisit', defaultValue: null },
        { name: 'ProfVisitResult', defaultValue: 0 },
        { name: 'RealityObject', defaultValue: null },
        { name: 'InformText' },
        { name: 'Address' },
        { name: 'ViolCount' },
        { name: 'Municipality' }
    ]
});