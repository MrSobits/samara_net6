Ext.define('B4.controller.dict.TariffByPeriodForClaimWork', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.TariffByPeriodForClaimWork'],
    stores: ['dict.TariffByPeriodForClaimWork'],
    views: ['dict.tariffbyperiodforclaimwork.Grid'],

    mainView: 'dict.tariffbyperiodforclaimwork.Grid',
    mainViewSelector: 'tariffByPeriodForClaimWorkGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'tariffByPeriodForClaimWorkGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'tariffByPeriodForClaimWorkGridAspect',
            storeName: 'dict.TariffByPeriodForClaimWork',
            modelName: 'dict.TariffByPeriodForClaimWork',
            gridSelector: 'tariffByPeriodForClaimWorkGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('tariffByPeriodForClaimWorkGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TariffByPeriodForClaimWork').load();
    }
});