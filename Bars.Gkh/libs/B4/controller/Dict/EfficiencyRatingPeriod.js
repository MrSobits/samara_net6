Ext.define('B4.controller.dict.EfficiencyRatingPeriod', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.EfficiencyRatingPeriod'],
    stores: ['dict.EfficiencyRatingPeriod'],
    views: ['dict.efficiencyratingperiod.Grid'],

    mainView: 'dict.efficiencyratingperiod.Grid',
    mainViewSelector: 'efficiencyratingperiodGrid',

    refs: [{
        ref: 'mainView',
        selector: 'efficiencyratingperiodGrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'efficiencyratingperiodGrid',
            permissionPrefix: 'Gkh.Dictionaries.EfficiencyRating.Period'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'efficiencyratingperiodAspect',
            modelName: 'dict.EfficiencyRatingPeriod',
            gridSelector: 'efficiencyratingperiodGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('efficiencyratingperiodGrid');
        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    }
});