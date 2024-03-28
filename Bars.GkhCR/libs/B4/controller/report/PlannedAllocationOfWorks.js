Ext.define('B4.controller.report.PlannedAllocationOfWorks', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.PlannedAllocationOfWorksPanel',
    mainViewSelector: '#plannedAllocationOfWorksPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
    ],

    views: [
    ],

    refs: [
        {
            ref: 'ProgramCrSelectField',
            selector: '#plannedAllocationOfWorksPanel #sfProgramCr'
        }
    ], 

    aspects: [
    ],

    validateParams: function () {
        var programField = this.getProgramCrSelectField();
        return (programField && programField.isValid());
    },

    getParams: function () {

        var programField = this.getProgramCrSelectField();
        
        return {
            programCrId: (programField ? programField.getValue() : null)
        };
    }
});