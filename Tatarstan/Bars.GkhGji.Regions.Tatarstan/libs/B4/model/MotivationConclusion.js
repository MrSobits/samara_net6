Ext.define('B4.model.MotivationConclusion', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivationConclusion'
    },
    fields: [
        { name: 'BaseDocumentId' },
        { name: 'Autor' },
        { name: 'Executant' },
        { name: 'Inspectors' },
        { name: 'State' },
        { name: 'DisposalNumber' },

        { name: 'MunicipalityId' },
        { name: 'MunicipalityName' },
        { name: 'InspectionBasis' },
        { name: 'ContragentName' },
        { name: 'PersonInspection' },
        { name: 'PersonInspectionName' },
        { name: 'RealityObjectCount' },
        { name: 'RealityObjectIds' },
        { name: 'Inspectors' },
        { name: 'InspectionId' },
        { name: 'TypeBase' }
    ]
});