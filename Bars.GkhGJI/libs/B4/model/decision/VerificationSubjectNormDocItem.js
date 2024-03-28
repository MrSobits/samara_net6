Ext.define('B4.model.decision.VerificationSubjectNormDocItem', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionVerificationSubjectNormDocItem'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Number' },
        { name: 'NormativeDocItem' },
        { name: 'DecisionVerificationSubject' },
        { name: 'NormativeDoc' },
        { name: 'Text' }
    ]
});