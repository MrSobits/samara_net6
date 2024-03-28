Ext.define('B4.controller.report.JurPersonInspectionPlan', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.JurPersonInspectionPlanPanel',
    mainViewSelector: 'jurPersonInspectionPlanPanel',


    views: [
        'report.JurPersonInspectionPlanPanel'
    ],

    refs: [
        {
            ref: 'YearField',
            selector: 'jurPersonInspectionPlanPanel [name=Year]'
        }
    ],


    getParams: function () {
        var yearField = this.getYearField();

        return {
            year: (yearField ? yearField.getValue() : null)
        };
    }
});