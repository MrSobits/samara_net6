Ext.define('B4.model.smev.SMEVERULReqNumber', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVERULReqNumber'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate'},
        { name: 'Inspector'},
        { name: 'RequestState' },       
        { name: 'Answer' },
        { name: 'MessageId' },
        { name: 'ManOrgLicense' },
        { name: 'DateDisposal' },
        { name: 'ERULRequestType', defaultvalue: 10},
        { name: 'DisposalNumber' },
        { name: 'Contragent' },
        { name: 'Inn' },
        { name: 'Ogrn' },
    ]
});