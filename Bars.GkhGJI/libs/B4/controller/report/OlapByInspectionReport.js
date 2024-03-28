Ext.define('B4.controller.report.OlapByInspectionReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.OlapByInspectionReportPanel',
    mainViewSelector: '#olapbyinspectionreportpanel',

    requires: [
        'B4.form.ComboBox'
    ], 

    refs: [
        {
            ref: 'StartDateField',
            selector: '#olapbyinspectionreportpanel #dfDateStart'
        },
        {
            ref: 'EndDateField',
            selector: '#olapbyinspectionreportpanel #dfDateEnd'
        }
    ],

    validateParams: function () {
        var me = this,
            dateStart = me.getStartDateField().getValue(),
            dateEnd = me.getEndDateField().getValue();
        
        if (dateEnd < dateStart) {
            return "Конечная дата должна быть позже начальной";
        }
    },

    getParams: function () {
        var me = this,
            dateStartField = me.getStartDateField(),
            dateEndField = me.getEndDateField();
        
        return {
            startDate: (dateStartField ? dateStartField.getRawValue() : null),
            endDate: (dateEndField ? dateEndField.getRawValue() : null)
        };
    }
});