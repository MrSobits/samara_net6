Ext.define('B4.controller.report.AnalysOfRealizByProgram', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.AnalysOfRealizByProgramPanel',
    mainViewSelector: '#analysOfRealizByProgramPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],
    views: [
        'SelectWindow.MultiSelectWindow',
        'report.AnalysOfRealizByProgramPanel'
    ],

    refs: [
        {
            ref: 'ProgramCrField',
            selector: '#analysOfRealizByProgramPanel #sfProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#analysOfRealizByProgramPanel #dfReportDate'
        }
    ],
    
    validateParams: function () {
        var prCrId = this.getProgramCrField();
        var date = this.getReportDateField();
        return (prCrId && prCrId.isValid() && date && date.isValid());
    },

    getParams: function () {

        var programCrField = this.getProgramCrField();
        var reportDateField = this.getReportDateField();

        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null)
        };
    }
});