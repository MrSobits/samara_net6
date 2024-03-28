Ext.define('B4.controller.report.InformationCrByResolution', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.InformationCrByResolutionPanel',
    mainViewSelector: '#informationCrByResolutionPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'ProgramCrField',
            selector: '#informationCrByResolutionPanel #sfProgramCr'
        }
    ],

    aspects: [
    ],

    validateParams: function () {
        var program = this.getProgramCrField();
        
        return (program && program.isValid());
    },

    getParams: function () {
        
        var programCrField = this.getProgramCrField();

        return {
            programCr: (programCrField ? programCrField.getValue() : null)
        };
    }
});