Ext.define('B4.model.manorglicense.LicenseResolutionGis', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicenseGis',
        listAction: 'GetResolutionsByMCID',
        timeout:100000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocNum' },
        { name: 'DocDate' },
        { name: 'Executant' },
        { name: 'TypeBase' },
        { name: 'InspectionId' },
        { name: 'TypeDocumentGji' }
       // { name: 'LawDate' }
    ]
});