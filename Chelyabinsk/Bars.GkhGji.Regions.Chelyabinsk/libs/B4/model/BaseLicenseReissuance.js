Ext.define('B4.model.BaseLicenseReissuance', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseLicenseReissuance'
    },
    fields: [
        { name: 'ContragentName', defaultValue: null },
        { name: 'ContragentInn', defaultValue: null },
        { name: 'TypeBase', defaultValue: 135 },
        { name: 'Municipality' },
        { name: 'LicNumGJI' },
        { name: 'RealityObjectCount' },
        { name: 'FormCheck', defaultValue: 10 },
        { name: 'InspectionNumber' },
        { name: 'IsDisposal' },
        { name: 'RealObjAddresses' },
        { name: 'RegisterNum' },
        { name: 'State', defaultValue: null },
        { name: 'LicenseReissuance', defaultValue: null },
        { name: 'MoSettlement' },
        { name: 'PlaceName' }
    ]
});