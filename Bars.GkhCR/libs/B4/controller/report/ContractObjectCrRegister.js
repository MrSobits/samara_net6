Ext.define('B4.controller.report.ContractObjectCrRegister', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ContractObjectCrRegisterPanel',
    mainViewSelector: '#contractObjectCrRegisterPanel',

    views: [
        'report.ContractObjectCrRegisterPanel'
    ],

    requires: [
        'B4.form.ComboBox'
    ],

    refs: [
        {
            ref: 'SfProgramCr',
            selector: '#contractObjectCrRegisterPanel #sfProgramCr'
        }
    ],

    validateParams: function () {
        var value = this.getSfProgramCr().getValue();
        if (value == null || value === "") {
            return "Выберите программу кап.ремонта!";
        } else {
            return true;
        }
    },
    
    init: function () {
        this.control({
            '#contractObjectCrRegisterPanel #sfProgramCr': {
                beforeload: function (store, operation) {
                    operation.params = {};
                    operation.params.notOnlyHidden = true;
                }
            }
        });
        this.callParent(arguments);
    },

    getParams: function () {
        var programmField = this.getSfProgramCr();

        return {
            programCrId: (programmField ? programmField.getValue() : null)
        };
    }
});