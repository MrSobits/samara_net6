Ext.define('B4.controller.report.AnalyticalReportBySubject', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.AnalyticalReportBySubjectPanel',
    mainViewSelector: 'analyticalreportbysubjectpanel',


    views: [
        'report.AnalyticalReportBySubjectPanel'
    ],

    refs: [
        {
            ref: 'YearField',
            selector: 'analyticalreportbysubjectpanel [name=Year]'
        }
    ],


    getParams: function () {

        var yearField = this.getYearField();

        return {
            year: (yearField ? yearField.getValue() : null)
        };
    }
});