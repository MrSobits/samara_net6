Ext.define('B4.model.actcheck.ExplanationAction', {
    extend: 'B4.model.actcheck.Action',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ExplanationAction'
    },
    fields: [
        { name: 'ContrPersType' },
        { name: 'ContrPersContragent' },
        { name: 'AttachmentName' },
        { name: 'AttachmentFile' },
        { name: 'Explanation' }
    ]
});