Ext.define('B4.controller.report.RaionChargeReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RaionChargeReportPanel',
    mainViewSelector: 'raionChargeReportPanel',

    stores: [
        'dict.municipality.MoArea',
        'MunicipalityByParent',
        'regop.ClosedChargePeriod'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalitySelectField',
            selector: 'raionChargeReportPanel b4selectfield[name=ParentMu]'
        },
        {
            ref: 'SettlementSelectField',
            selector: 'raionChargeReportPanel b4selectfield[name=Municipality]'
        },
        {
            ref: 'PeriodSelectField',
            selector: 'raionChargeReportPanel b4selectfield[name=ChargePeriod]'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            "raionChargeReportPanel b4selectfield[name=ParentMu]": {
                change: me.onMrChange
            },
            "raionChargeReportPanel b4selectfield[name=Municipality]": {
                beforeload: me.beforeMoLoad
            }
        });

        me.callParent(arguments);
    },
    
    onMrChange: function (field, newValue) {
        var me = this,
            moField = me.getSettlementSelectField();

        moField.setDisabled(!newValue);
        moField.setValue(null);
    },
       
    beforeMoLoad: function (field, operation) {
        var mrField = this.getMunicipalitySelectField();
        operation.params = operation.params || {};
        operation.params.parentId = mrField.getValue();
    },
        
    validateParams: function () {
        return true;
    },

    getParams: function () {
        var me = this,
            mrField = me.getMunicipalitySelectField(),
            moField = me.getSettlementSelectField(),
            periodField = me.getPeriodSelectField();
            
        return {
            periodId: (periodField ? periodField.getValue() : null),
            moIds: (moField ? moField.getValue() : null),
            mrId: (mrField ? mrField.getValue() : null)
        };
    }
});