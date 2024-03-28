Ext.define('B4.model.BaseLicenseApplicants', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseLicenseApplicants'
    },
    fields: [
        { name: 'ContragentName', defaultValue: null },
        { name: 'TypeBase', defaultValue: 130 },
        { name: 'Municipality' },
        { name: 'RealityObjectCount' },
        { name: 'TypeForm', defaultValue: 20 },
        { name: 'InspectionType', defaultValue: 10 },
        { name: 'InspectionNumber' },
        { name: 'IsDisposal' },
        { name: 'RealObjAddresses' },
        { name: 'ReqNumber' },
        { name: 'State', defaultValue: null },
        { name: 'ManOrgLicenseRequest', defaultValue: null },
        { name: 'MoSettlement' },
        { name: 'PlaceName' }
    ]
});