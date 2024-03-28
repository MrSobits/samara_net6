Ext.define('B4.controller.report.TypeWorkReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.TypeWorkReportPanel',
    mainViewSelector: 'typeworkreportpanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.PublishedProgramReportPanel'
    ],

    refs: [
        {
            ref: 'StartYearField',
            selector: 'typeworkreportpanel [name=StartYear]'
        },
        {
            ref: 'EndYearField',
            selector: 'typeworkreportpanel [name=EndYear]'
        }
    ],

    validateParams: function () {
        return true;
    },

    getParams: function () {
        var startYearField = this.getStartYearField();
        var endYearField = this.getEndYearField();

        return {
            startYear: (startYearField ? startYearField.getValue() : null),
            endYear: (endYearField ? endYearField.getValue() : null)
        };
    }
});