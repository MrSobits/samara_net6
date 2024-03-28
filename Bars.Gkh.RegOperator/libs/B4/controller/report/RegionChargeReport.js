Ext.define('B4.controller.report.RegionChargeReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RegionChargeReportPanel',
    mainViewSelector: 'regionChargeReportPanel',

    stores: [
        'dict.municipality.MoArea'
    ],

    requires: [
       'B4.store.regop.ClosedChargePeriod'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalitySelectField',
            selector: 'regionChargeReportPanel b4selectfield[name=ParentMu]'
        },
        {
            ref: 'PeriodSelectField',
            selector: 'regionChargeReportPanel b4selectfield[name=ChargePeriod]'
        }
    ],

    validateParams: function () {
        return true;
    },

    getParams: function () {
        var me = this,
            mrField = me.getMunicipalitySelectField(),
            periodField = me.getPeriodSelectField();
            
        return {
            periodId: (periodField ? periodField.getValue() : null),
            mrIds: (mrField ? mrField.getValue() : null)
        };
    }
});