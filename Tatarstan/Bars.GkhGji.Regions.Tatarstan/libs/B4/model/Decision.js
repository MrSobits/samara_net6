Ext.define('B4.model.Decision', {
    extend: 'B4.model.Disposal',

    proxy: {
        type: 'b4proxy',
        controllerName: 'TatarstanDecision'
    },
    fields: [
        { name: 'DecisionPlace' },
        { name: 'SubmissionDate' },
        { name: 'ReceiptDate' },
        { name: 'UsingMeansRemoteInteraction' },
        { name: 'InfoUsingMeansRemoteInteraction' },
        { name: 'InformationAboutHarm'}
    ]
});