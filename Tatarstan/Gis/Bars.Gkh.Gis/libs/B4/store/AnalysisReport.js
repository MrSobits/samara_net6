Ext.define('B4.store.AnalysisReport', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.AnalysisReport'],
    autoLoad: false,
    storeId: 'analysisreport',
    model: 'B4.model.AnalysisReport',
    root: {
        Title: 'Root'
    }
});