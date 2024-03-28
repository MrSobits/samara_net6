Ext.define('B4.controller.dict.FixedPeriodCalcPenalties', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'dict.FixedPeriodCalcPenalties'
    ],
    stores: [
        'dict.FixedPeriodCalcPenalties'
    ],
    views: [
        'dict.fixedperiodcalcpenalties.Grid',
        'dict.fixedperiodcalcpenalties.EditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.fixedperiodcalcpenalties.Grid',
    mainViewSelector: 'fixedperiodcalcpenaltiesGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'fixedperiodcalcpenaltiesGrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'fixedperiodcalcpenaltiesGridWindowAspect',
            gridSelector: 'fixedperiodcalcpenaltiesGrid',
            editFormSelector: 'fixedperiodcalcpenaltieseditwindow',
            storeName: 'dict.FixedPeriodCalcPenalties',
            modelName: 'dict.FixedPeriodCalcPenalties',
            editWindowView: 'dict.fixedperiodcalcpenalties.EditWindow'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('fixedperiodcalcpenaltiesGrid');
        me.bindContext(view);
        me.getStore('dict.FixedPeriodCalcPenalties').load();
    }
});