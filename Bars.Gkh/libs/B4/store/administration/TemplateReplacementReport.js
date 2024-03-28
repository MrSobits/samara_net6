Ext.define('B4.store.administration.TemplateReplacementReport', {
    extend: 'B4.base.Store',
    requires: ['B4.model.administration.TemplateReplacementReport'],
    autoLoad: false,
    storeId: 'templateReplacementReportStore',
    model: 'B4.model.administration.TemplateReplacementReport',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TemplateReplacement',
        listAction: 'ListReports'
    }
});