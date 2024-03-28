Ext.define('B4.controller.report.NotificationFormFundReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.NotificationFormFundReportPanel',
    mainViewSelector: '#notifformfundreportpanel',

    requires: [],

    stores: [],

    views: [],
    
    aspects: [],

    validateParams: function () {
        return !!this.getMainView().down('numberfield[name=Year]').getValue();
    },

    getParams: function () {
        var yearField = this.getMainView().down('numberfield[name=Year]');

        return {year: (yearField ? yearField.getValue() : null) };
    }
});