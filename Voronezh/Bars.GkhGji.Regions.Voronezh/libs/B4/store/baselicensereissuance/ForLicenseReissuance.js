Ext.define('B4.store.baselicensereissuance.ForLicenseReissuance', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'InspectionNumber' },
        { name: 'RealObjAddresses' },
        { name: 'DisposalDate' },
        { name: 'DisposalNumber' },
        { name: 'AppealNumber' },
        { name: 'AppealDate' },
        { name: 'InspectionResult' },
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseLicenseApplicants',
        listAction: 'ListByLicenseReiss'
    }
});