Ext.define('B4.controller.report.OwnerChargeReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.OwnerChargeReportPanel',
    mainViewSelector: 'ownerChargeReportPanel',

    stores: [
        'dict.municipality.MoArea',
        'RealtyObjectByMo',
        'MunicipalityByParent',
        'regop.ClosedChargePeriod',
        'regop.personal_account.PersonalAccountsByRo'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalitySelectField',
            selector: 'ownerChargeReportPanel treeselectfield[name=Municipality]'
        },
        {
            ref: 'AddressSelectField',
            selector: 'ownerChargeReportPanel b4selectfield[name=RealityObject]'
        },
        {
            ref: 'OwnerSelectField',
            selector: 'ownerChargeReportPanel b4selectfield[name=Owners]'
        },
        {
            ref: 'PeriodSelectField',
            selector: 'ownerChargeReportPanel b4selectfield[name=ChargePeriod]'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            "ownerChargeReportPanel treeselectfield[name=Municipality]": {
                change: me.onMrChange
            },

            "ownerChargeReportPanel b4selectfield[name=RealityObject]": {
                beforeload: me.beforeRoLoad,
                change: me.onRoChange
            },
            "ownerChargeReportPanel b4selectfield[name=Owners]": {
                beforeload: me.beforeOwnerLoad
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
    
    onRoChange: function (field, newValue) {
        var me = this,
            moField = me.getOwnerSelectField();

        moField.setDisabled(!newValue);
        moField.setValue(null);
    },
   
    beforeRoLoad: function (field, operation) {
        var moField = this.getMunicipalitySelectField();
        operation.params = operation.params || {};
        operation.params.settlementId = moField.getValue();
        operation.params.isDecisionRegOp = true;
    },
    
    beforeOwnerLoad: function (field, operation) {
        var roField = this.getAddressSelectField();
        operation.params = operation.params || {};
        operation.params.roId = roField.getValue();
    },
    
    validateParams: function () {
        return true;
    },

    getParams: function () {
        var me = this,
            roField = me.getAddressSelectField(),
            owField = me.getOwnerSelectField(),
            periodField = me.getPeriodSelectField();
            
        return {
            periodId: (periodField ? periodField.getValue() : null),
            accountIds: (owField ? owField.getValue() : null),
            roId: (roField ? roField.getValue() : null)
        };
    }
});