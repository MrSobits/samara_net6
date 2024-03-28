Ext.define('B4.model.decision.ControlMeasures', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionControlMeasures'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Decision', defaultValue: null },
        { name: 'ControlActivity', defaultValue: null },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'Description' }
    ]
});