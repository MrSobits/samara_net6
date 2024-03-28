Ext.define('B4.model.manorglicense.CourtDecisionGis', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CourtDecision'
     },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DecisionDate' },
        { name: 'DecisionEntryDate' },
        { name: 'DecisionNumber' },
        { name: 'Inspector' },
        { name: 'JudicalOffice' },
        { name: 'Resolution' },
        { name: 'ProtocolNumber' },
        { name: 'Violation' },
        { name: 'CourtDecisionType' }
    ]
});