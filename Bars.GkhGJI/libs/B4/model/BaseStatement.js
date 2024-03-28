Ext.define('B4.model.BaseStatement', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseStatement',
        timeout: 60000
    },
    fields: [
        { name: 'RequestType' },
        { name: 'ContragentName', defaultValue: null },
        { name: 'TypeBase', defaultValue: 20 },
        { name: 'Municipality' },
        { name: 'RealityObjectCount' },
        { name: 'TypeForm', defaultValue: 10 },
        { name: 'InspectionNumber' },
        { name: 'IsDisposal' },
        { name: 'RealObjAddresses' },
        { name: 'State', defaultValue: null },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'ControlType', defaultValue: 10 },
        { name: 'MotivationConclusions' },
        { name: 'Inn'}
    ]
});