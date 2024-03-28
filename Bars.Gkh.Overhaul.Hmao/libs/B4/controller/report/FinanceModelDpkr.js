Ext.define('B4.controller.report.FinanceModelDpkr', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.FinanceModelDpkrPanel',
    mainViewSelector: 'reportFinanceModelDpkrPanel',


    refs: [
        {
            ref: 'MunicipalitySelectField',
            selector: 'reportFinanceModelDpkrPanel [name=Municipality]'
        },
        {
            ref: 'PeriodField',
            selector: 'reportFinanceModelDpkrPanel [name=Period]'
        }
    ],

    validateParams: function() {
        return true;
    },

    getParams: function() {
        var muField = this.getMunicipalitySelectField(),
            periodField = this.getPeriodField();

        return {
            municipalityId: (muField ? muField.getValue() : null),
            groupPeriod: (periodField ? periodField.getValue() : null)
        };
    }
});