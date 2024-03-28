Ext.define('B4.model.smev2.SMEVValidPassport', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVValidPassport'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate'},
        { name: 'Inspector'},
        { name: 'RequestState', defaultValue: 0 },
        { name: 'DocSerie' },
        { name: 'DocNumber' },
        { name: 'DocIssueDate' },
        { name: 'DocStatus' },
        { name: 'InvalidityReason' },
        { name: 'InvaliditySince' },
        { name: 'CalcDate' },
        { name: 'MessageId' },
        { name: 'SerNumber' }
    ]
});