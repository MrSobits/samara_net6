Ext.define('B4.model.smev.SMEVSocialHire', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVSocialHire'
    },
    fields: [
        { name: 'Inspector' },
        { name: 'CalcDate' },
        { name: 'RealityObject' },
        { name: 'Municipality' },
        { name: 'Room' },
        { name: 'ContractNumber' },
        { name: 'ContractType' },
        { name: 'Name' },
        { name: 'Purpose' },
        { name: 'TotalArea' },
        { name: 'LiveArea' },
        { name: 'LastName' },
        { name: 'FirstName' },
        { name: 'MiddleName' },
        { name: 'Birthday' },
        { name: 'Birthplace' },
        { name: 'Citizenship' },
        { name: 'DocumentType' },
        { name: 'DocumentNumber' },
        { name: 'DocumentSeries' },
        { name: 'DocumentDate' },
        { name: 'IsContractOwner' },
        { name: 'AnswerDistrict' },
        { name: 'AnswerCity' },
        { name: 'AnswerStreet' },
        { name: 'AnswerHouse' },
        { name: 'AnswerFlat' },
        { name: 'AnswerRegion' },
        { name: 'RequestState' },
        { name: 'Answer' },
        { name: 'MessageId' },
        { name: 'ReqId' }
    ]
});