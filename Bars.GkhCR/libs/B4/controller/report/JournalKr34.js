Ext.define('B4.controller.report.JournalKr34', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.JournalKr34Panel',
    mainViewSelector: '#journalKr34Panel',

    requires: [
        
    ],

    stores: [
    ],

    views: [
    ],

    refs: [
        {
            ref: 'ProgramCrSelectField',
            selector: '#journalKr34Panel #sfProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#journalKr34Panel #dfReportDate'
        },
        {
            ref: 'ReportTypeWork',
            selector: '#journalKr34Panel #cbTypeWork'
        }
    ],

    aspects: [
    ],

    validateParams: function () {
        var date = this.getReportDateField();
        var prCrId = this.getProgramCrSelectField();
        var workId = this.getReportTypeWork();
        return (prCrId && prCrId.isValid() && date && date.isValid() && workId && workId.isValid());
    },

    getParams: function () {
        var programmField = this.getProgramCrSelectField();
        var dateField = this.getReportDateField();
        var typeWork = this.getReportTypeWork();
        return {
            programCrId: (programmField ? programmField.getValue() : null),
            reportDate: (dateField ? dateField.getValue() : null),
            typeWork: (typeWork ? typeWork.getValue() : null)
        };
    }
});