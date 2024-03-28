Ext.define('B4.store.baselicenseapplicants.ForLicenseRequest', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'InspectionNumber' },
        { name: 'RealObjAddresses' },
        { name: 'DisposalDate' },
        { name: 'DisposalNumber' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseLicenseApplicants',
        listAction: 'ListByLicenseReq'
    }
});