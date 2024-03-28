Ext.define('B4.model.decision.ControlList', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionControlList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Decision', defaultValue: null },
        { name: 'ControlList', defaultValue: null },
        { name: 'Description' }
    ]
});