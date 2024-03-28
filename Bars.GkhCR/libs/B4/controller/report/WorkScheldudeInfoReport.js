Ext.define('B4.controller.report.WorkScheldudeInfoReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.WorkScheldudeInfoReportPanel',
    mainViewSelector: '#workScheldudeInfoReportPanel',

    requires: [
        'B4.form.ComboBox'
    ],

    views: [
        'report.WorkScheldudeInfoReportPanel'
    ],

    refs: [
        {
            ref: 'ProgramCrField',
            selector: '#workScheldudeInfoReportPanel #sfProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#workScheldudeInfoReportPanel #dfReportDate'
        }
    ],
    
    validateParams: function () {
        var programCrField = this.getProgramCrField();
        var reportDateField = this.getReportDateField();
        if (!programCrField.getValue() || !reportDateField.getValue()) {
            return false;
        }
        
        return true;
    },
    
    init: function () {
        var actions = {};
        actions['#workScheldudeInfoReportPanel #sfProgramCr'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    onBeforeLoadProgramCr: function (field, options) {
        options.params = {};
        options.params.notOnlyHidden = true;
        options.params.allowBlank = false;
    },
    
    getParams: function () {
        var programCrField = this.getProgramCrField();
        var reportDateField = this.getReportDateField();
        
        //if (!programCrField.getValue()) {
        //    Ext.Msg.alert('Ошибка построения отчета', 'Параметр "Программа" обязателен для заполнения!');
        //}
        
        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null)
        };
    }
});