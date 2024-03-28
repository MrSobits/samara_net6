Ext.define('B4.controller.report.FundFormationStimulReport', {    
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.FundFormationStimulPanel',
    mainViewSelector: '#fundFormationStimulPanel',
    
    init: function() {
        var me = this;

        me.control();

        me.callParent(arguments);
    },
    
    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.ux.button.Update'
    ],
    
    views: [
        'SelectWindow.MultiSelectWindow',
        'B4.view.report.FundFormationStimulPanel'
    ],
    
    refs: [
        {
            ref: 'MunicipalityRTriggerField',
            selector: '#fundFormationStimulPanel #municipalityR'
        },
        {
            ref: 'MethodFormFundComboBox',
            selector: '#fundFormationStimulPanel #mffMethodFormFund'
        },
        {
            ref: 'DateFromDateField',
            selector: '#fundFormationStimulPanel #dfDateFrom'
        },
        {
            ref: 'DateToDateField',
            selector: '#fundFormationStimulPanel #dfDateTo'
        }
    ],
    
    validateParams: function () {
        return true;
    },

    getParams: function () {
        var municipalityR = this.getMunicipalityRTriggerField();
        var methodForm = this.getMethodFormFundComboBox();
        var dateFrom = this.getDateFromDateField();
        var dateTo = this.getDateToDateField();

        return {
            municipality: (municipalityR ? municipalityR.getValue() : null),
            formationType: (methodForm ? methodForm.getValue() : null),
            dateFrom: (dateFrom ? dateFrom.getValue() : null),
            dateTo: (dateTo ? dateTo.getValue() : null)
        };
    }
});