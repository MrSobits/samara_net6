Ext.define('B4.controller.dict.KindRisk', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.KindRisk'],
    stores: ['dict.KindRisk'],
    views: ['dict.kindrisk.Grid'],

    mainView: 'dict.kindrisk.Grid',
    mainViewSelector: 'kindRiskGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'kindRiskGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'kindRiskGrid',
            permissionPrefix: 'Gkh.Dictionaries.KindRisk'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'kindRiskGridAspect',
            storeName: 'dict.KindRisk',
            modelName: 'dict.KindRisk',
            gridSelector: 'kindRiskGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('kindRiskGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.KindRisk').load();
    }
});