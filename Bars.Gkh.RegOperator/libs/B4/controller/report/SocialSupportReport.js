Ext.define('B4.controller.report.SocialSupportReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.SocialSupportReportPanel',
    mainViewSelector: 'socialSupportReportPanel',

    requires: [
        'B4.ux.button.Update'
    ],

    refs: [
        {
            ref: 'StartDateField',
            selector: 'socialSupportReportPanel [name=StartDate]'
        },
        {
            ref: 'EndDateField',
            selector: 'socialSupportReportPanel [name=EndDate]'
        }
    ],

    validateParams: function () {
        var dateStartField = this.getStartDateField().getValue();
        var dateEndField = this.getEndDateField().getValue();

        if (dateStartField == null || dateStartField == Date.min) {
            return "Не указан параметр \"Дата с\"";
        }

        if (dateEndField === null || dateEndField == Date.min) {
            return "Не указан параметр \"Дата по\"";
        }

        if (dateEndField < dateStartField) {
            return "Конечная дата должна быть позже начальной";
        }
        
        return true;
    },

    getParams: function () {

        var dateStartField = this.getStartDateField();
        var dateEndField = this.getEndDateField();


        return {
            startDate: (dateStartField ? dateStartField.getValue() : null),
            endDate: (dateEndField ? dateEndField.getValue() : null)
        };
    }
});