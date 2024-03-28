Ext.define('B4.model.BaseProsClaim', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseProsClaim'
    },
    fields: [
        { name: 'Municipality', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'IssuedClaim' },
        { name: 'ProsClaimDateCheck' },
        { name: 'ProsClaimDate' },
        { name: 'DocumentDate' },
        { name: 'DocumentName' },
        { name: 'DocumentNumber' },
        { name: 'InspectionNumber' },
        { name: 'DocumentDescription' },
        { name: 'TypeBase', defaultValue: 50 },
        { name: 'TypeForm', defaultValue: 10 },
        { name: 'TypeBaseProsClaim', defaultValue: 10 },
        { name: 'RealityObjectCount' },
        { name: 'InspectorNames' },
        { name: 'File', defaultValue: null },
        { name: 'ContragentName' },
        { name: 'State', defaultValue: null },
        { name: 'IsResultSent' },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'ControlType', defaultValue: 10 },
        { name: 'Inn'}
    ]
});