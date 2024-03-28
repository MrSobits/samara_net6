Ext.define('B4.controller.report.JournalKr1', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.JournalKr1Panel',
    mainViewSelector: '#journalKr1Panel',

    requires: [
        'B4.form.ComboBox'
    ],

    views: [
        'report.JournalKr1Panel'
    ],

    refs: [
        {
            ref: 'ProgramCrField',
            selector: '#journalKr1Panel #sfProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#journalKr1Panel #dfReportDate'
        }
    ],

    validateParams: function () {
        var prCrId = this.getProgramCrField();
        var date = this.getReportDateField();
        return (prCrId && prCrId.isValid() && date && date.isValid());
    },

    init: function () {
        var actions = {};
        actions['#journalKr1Panel #sfProgramCr'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    onBeforeLoadProgramCr: function (field, options) {
        options.params = {};
        options.params.notOnlyHidden = true;
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