Ext.define('B4.controller.report.MunicipalityChargeReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.MunicipalityChargeReportPanel',
    mainViewSelector: 'municipalityChargeReportPanel',

    stores: [
        'dict.municipality.MoArea',
        'RealtyObjectByMo',
        'MunicipalityByParent',
        'regop.ClosedChargePeriod'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalitySelectField',
            selector: 'municipalityChargeReportPanel treeselectfield[name=Municipality]'
        },
        {
            ref: 'AddressSelectField',
            selector: 'municipalityChargeReportPanel b4selectfield[name=RealityObject]'
        },
        {
            ref: 'PeriodSelectField',
            selector: 'municipalityChargeReportPanel b4selectfield[name=ChargePeriod]'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            "municipalityChargeReportPanel treeselectfield[name=Municipality]": {
                change: me.onMrChange
            },
            "municipalityChargeReportPanel b4selectfield[name=RealityObject]": {
                beforeload: me.beforeRoLoad
            }
        });

        me.callParent(arguments);
    },
    
    onMrChange: function (field, newValue) {
        var me = this,
           addressField = me.getAddressSelectField();

        addressField.setDisabled(!newValue);
        addressField.setValue(null);
    },
    
    beforeRoLoad: function (field, operation) {
        var moField = this.getMunicipalitySelectField();
        operation.params = operation.params || {};
        operation.params.settlementId = moField.getValue();
        operation.params.isDecisionRegOp = true;
    },
        
    validateParams: function () {
        return true;
    },

    getParams: function () {
        var me = this,
            roField = me.getAddressSelectField(),
            moField = me.getMunicipalitySelectField(),
            periodField = me.getPeriodSelectField();
            
        return {
            periodId: (periodField ? periodField.getValue() : null),
            moId: (moField ? moField.getValue() : null),
            roIds: (roField ? roField.getValue() : null)
        };
    }
});