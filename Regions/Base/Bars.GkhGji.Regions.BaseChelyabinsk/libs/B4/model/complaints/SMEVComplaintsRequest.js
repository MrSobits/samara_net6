Ext.define('B4.model.complaints.SMEVComplaintsRequest', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVComplaintsRequest'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate' },
        { name: 'Inspector'},
        { name: 'RequestState' },
        { name: 'Answer' },
        { name: 'ComplaintId', defaultValue: null },
        { name: 'TypeComplainsRequest', defaultValue: 0 },
        { name: 'MessageId' },
        { name: 'TextReq' },
        { name: 'FileInfo' }
    ]
});