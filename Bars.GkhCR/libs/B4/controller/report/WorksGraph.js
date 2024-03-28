Ext.define('B4.controller.report.WorksGraph', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.WorksGraphPanel',
    mainViewSelector: '#worksGraphPanel',

    views: [
        'report.WorksGraphPanel'
    ],

    requires: [
        'B4.form.ComboBox'
    ],

    refs: [
        {
            ref: 'SfProgramCr',
            selector: '#worksGraphPanel #sfProgramCr'
        }
    ],

    validateParams: function () {
        var programmField = this.getSfProgramCr();
        return (programmField && programmField.isValid());
    },

    getParams: function() {
        var programmField = this.getSfProgramCr();

        return {
            programCrId: (programmField ? programmField.getValue() : null)
        };
    }
});