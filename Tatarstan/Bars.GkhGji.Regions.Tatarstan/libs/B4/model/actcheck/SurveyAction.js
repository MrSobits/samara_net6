Ext.define('B4.model.actcheck.SurveyAction', {
    extend: 'B4.model.actcheck.Action',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SurveyAction'
    },
    fields: [
        { name: 'ContinueDate' },
        { name: 'ContinueStartTime' },
        { name: 'ContinueEndTime' },
        { name: 'ProtocolReaded' },
        { name: 'HasRemark' }
    ]
});