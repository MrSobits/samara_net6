Ext.define('B4.model.resolution.Dispute', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionDispute'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Resolution', defaultValue: null },
        { name: 'Court', defaultValue: null },
        { name: 'Instance', defaultValue: null },
        { name: 'CourtVerdict', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'ProsecutionProtest', defaultValue: false },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Appeal', defaultValue: 30 },
        { name: 'Lawyer', defaultValue: null },
        { name: 'Description' }
    ]
});