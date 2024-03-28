Ext.define('B4.controller.report.ActReviseInspectionHalfYear', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ActReviseInspectionHalfYearPanel',
    mainViewSelector: 'actReviseInspectionHalfYearPanel',


    views: [
        'report.ActReviseInspectionHalfYearPanel'
    ],

    refs: [
        {
            ref: 'YearField',
            selector: 'actReviseInspectionHalfYearPanel [name=Year]'
        },
        {
            ref: 'HalfYearField',
            selector: 'actReviseInspectionHalfYearPanel [name=HalfYear]'
        }
    ],


    getParams: function () {
        var yearField = this.getYearField();
        var halfYearField = this.getHalfYearField();

        return {
            year: (yearField ? yearField.getValue() : null),
            halfYear: (halfYearField ? halfYearField.getValue() : null)
        };
    }
});