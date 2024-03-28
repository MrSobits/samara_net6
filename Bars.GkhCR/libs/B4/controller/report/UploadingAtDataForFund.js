Ext.define('B4.controller.report.UploadingAtDataForFund', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.UploadingAtDataForFundPanel',
    mainViewSelector: '#uploadingAtDataForFundPanel',
    
    validateParams: function () {
        return true;
    },

    getParams: function () {
        return {
            municipality: null
        };
    }
});