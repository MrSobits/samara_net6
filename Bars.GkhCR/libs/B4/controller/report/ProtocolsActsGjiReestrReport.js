Ext.define('B4.controller.report.ProtocolsActsGjiReestrReport', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.ProtocolsActsGjiReestrReportPanel',
    mainViewSelector: '#protocolsActsGjiReestrReportPanel',

    requires: [
        'B4.form.ComboBox'
    ],

    views: [
        'report.ProtocolsActsGjiReestrReportPanel'
    ],

    refs: [
        {
            ref: 'ProgramCrSelectField',
            selector: '#protocolsActsGjiReestrReportPanel #sfProgramCr'
        }
    ],
   

    validateParams: function () {
        var programmField = this.getProgramCrSelectField();
        return (programmField && programmField.isValid());
    },

    init: function () {
        this.control({
            '#protocolsActsGjiReestrReportPanel #sfProgramCr': {
                beforeload: function (store, operation) {
                    operation.params = {};
                    operation.params.notOnlyHidden = true;
                }
            }
        });
        this.callParent(arguments);
    },

    getParams: function () {
        var programmField = this.getProgramCrSelectField();

        //получаем компонент
        return {
            programCrId: (programmField ? programmField.getValue() : null)
        };
    }
});