Ext.define('B4.model.disposal.ControlObjectInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionControlObjectInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Decision' },
        { name: 'InspGjiRealityObject' },
        { name: 'ControlObjectType' },
        { name: 'ControlObjectKind' },
    ]
});