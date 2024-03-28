Ext.define('B4.model.regop.report.PaymentDocumentTemplate', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'TemplateCode' },
        { name: 'ReportName' },
        { name: 'Period' },
        { name: 'PeriodName' },
        { name: 'HasSnapshots' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentDocumentTemplate',
        timeout: 1000000
    }
});