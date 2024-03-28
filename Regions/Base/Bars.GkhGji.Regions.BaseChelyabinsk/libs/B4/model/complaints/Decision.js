Ext.define('B4.model.complaints.Decision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVComplaintsDecision'
    },
    fields: [
        { name: 'Code'},
        { name: 'Name' },
        { name: 'Number' },
        { name: 'FullName' },
        { name: 'CompleteReject', defaultValue: 0 }       
    ]
});