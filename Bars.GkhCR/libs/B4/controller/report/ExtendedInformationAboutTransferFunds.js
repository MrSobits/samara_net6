Ext.define('B4.controller.report.ExtendedInformationAboutTransferFunds', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ExtendedInformationAboutTransferFundsPanel',
    mainViewSelector: '#extendedInformationAboutTransferFundsPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],
    
    views: [
        'SelectWindow.MultiSelectWindow',
        'report.DefectListPanel'
    ],

    refs: [
        {
            ref: 'ProgramCrField',
            selector: '#extendedInformationAboutTransferFundsPanel #sfProgramCr'
        },
        {
            ref: 'DateStartField',
            selector: '#extendedInformationAboutTransferFundsPanel #dfDateStart'
        },
        {
            ref: 'DateEndField',
            selector: '#extendedInformationAboutTransferFundsPanel #dfDateEnd'
        },
        {
            ref: 'TypeFinGroupField',
            selector: '#extendedInformationAboutTransferFundsPanel #cbxTypeFinGroup'
        }
    ],
    
    validateParams: function () {
        var prCrId = this.getProgramCrField();
        var startDate = this.getDateStartField();
        var endDate = this.getDateEndField();
        var finId = this.getTypeFinGroupField();
        return (prCrId && prCrId.isValid() && startDate && startDate.isValid() && endDate && endDate.isValid() && finId && finId.isValid());
    },

    init: function () {
        var actions = {};
        actions['#extendedInformationAboutTransferFundsPanel #sfProgramCr'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    onBeforeLoadProgramCr: function (field, options) {
        options.params = {};
        options.params.notOnlyHidden = true;
    },

    getParams: function () {

        var programCrField = this.getProgramCrField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();
        var typeFinGroupField = this.getTypeFinGroupField();

        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null),
            typeFinGroup: (typeFinGroupField ? typeFinGroupField.getValue() : null)
        };
    }
});