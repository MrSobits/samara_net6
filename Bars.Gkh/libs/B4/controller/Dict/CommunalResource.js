Ext.define('B4.controller.dict.CommunalResource', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.CommunalResource'],
    stores: ['dict.CommunalResource'],
    views: [
        'dict.communalresource.Grid'
    ],
    mixins: {
        context: 'B4.mixins.Context'
    },
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'communalResourceGrid',
            permissionPrefix: 'Gkh.Dictionaries.CommunalResource'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'communalResourceGridWindowAspect',
            gridSelector: 'communalResourceGrid',
            storeName: 'dict.CommunalResource',
            modelName: 'dict.CommunalResource'
        }
    ],

    mainView: 'dict.communalresource.Grid',
    mainViewSelector: 'communalResourceGrid',

    refs: [
    {
        ref: 'mainView',
        selector: 'communalResourceGrid'
    }],

    index: function () {
        var view = this.getMainView() || Ext.widget('communalResourceGrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});