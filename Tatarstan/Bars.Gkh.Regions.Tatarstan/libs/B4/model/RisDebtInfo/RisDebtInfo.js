Ext.define('B4.model.RisDebtInfo.RisDebtInfo', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DebtInfo',
        readAction: 'Get'
    },
    fields: [
        { name: 'NeedResponse' },
        { name: 'NotSentResponse' },
        { name: 'SentResponse'}
    ]
});