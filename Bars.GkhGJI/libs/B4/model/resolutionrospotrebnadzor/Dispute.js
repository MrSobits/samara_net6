Ext.define('B4.model.resolutionrospotrebnadzor.Dispute', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionRospotrebnadzorDispute'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'File', defaultValue: null },
        { name: 'ProsecutionProtest', defaultValue: false },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Appeal' },
        { name: 'Description' },
        { name: 'Resolution', useNull: false },
        { name: 'Court', defaultValue: null },
        { name: 'Instance', defaultValue: null },
        { name: 'CourtVerdict', defaultValue: null },
        { name: 'Lawyer', defaultValue: null }
    ]
});