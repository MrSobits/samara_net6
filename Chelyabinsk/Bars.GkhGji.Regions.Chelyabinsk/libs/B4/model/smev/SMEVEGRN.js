Ext.define('B4.model.smev.SMEVEGRN', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVEGRN'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate' },
        { name: 'Inspector'},
        { name: 'RequestState' },
        { name: 'Answer' },
        { name: 'DeclarantId' },
        { name: 'RegionCode' },
        { name: 'PersonPatronymic' },
        { name: 'DocumentNumber' },
        { name: 'DocumentSerial' },
        { name: 'EGRNApplicantType' },
        { name: 'EGRNObjectType' },
        { name: 'IdDocumentRef' },
        { name: 'PersonName' },
        { name: 'Declarant' },
        { name: 'PersonSurname' },
        { name: 'CadastralNUmber' },
        { name: 'RequestType' },
        { name: 'MessageId' },
        { name: 'RealityObject' },
        { name: 'Room' },
        { name: 'QualityPhone' },
        { name: 'Municipality' }
    ]
});