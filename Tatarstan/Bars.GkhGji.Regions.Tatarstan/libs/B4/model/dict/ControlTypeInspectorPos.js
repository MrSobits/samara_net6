Ext.define('B4.model.dict.ControlTypeInspectorPos', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlTypeInspectorPositions'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ControlType' },
        { name: 'InspectorPosition' },
        { name: 'IsIssuer' },
        { name: 'IsMember' },
        { name: 'ErvkId' }
    ]
});