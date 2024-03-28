Ext.define('B4.model.smev.SMEVEDO', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVEDO'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate'},
        { name: 'Inspector'},
        { name: 'RequestState' },
        { name: 'Answer' },
        { name: 'CalcDate' },
        { name: 'TextReq' },
        { name: 'MessageId' }
    ]
});