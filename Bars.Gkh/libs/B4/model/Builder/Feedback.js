Ext.define('B4.model.builder.Feedback', {
    extend: 'B4.base.Model',
    requires: ['B4.enums.TypeAssessment',
            'B4.enums.TypeAuthor'],
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuilderFeedback'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Builder', defaultValue: null },
        { name: 'Content' },
        { name: 'DocumentName' },
        { name: 'FeedbackDate' },
        { name: 'OrganizationName' },
        { name: 'TypeAssessment', defaultValue: 10 },
        { name: 'TypeAuthor', defaultValue: 10 },
        { name: 'File', defaultValue: null }
    ]
});