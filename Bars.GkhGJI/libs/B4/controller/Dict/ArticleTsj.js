Ext.define('B4.controller.dict.ArticleTsj', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ArticleTsj'],
    stores: ['dict.ArticleTsj'],

    views: ['dict.articletsj.Grid'],

    mainView: 'dict.articletsj.Grid',
    mainViewSelector: 'articleTsjGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'articleTsjGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'articleTsjGrid',
            permissionPrefix: 'GkhGji.Dict.ArticleTsj'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'articleTsjGridAspect',
            storeName: 'dict.ArticleTsj',
            modelName: 'dict.ArticleTsj',
            gridSelector: 'articleTsjGrid'
        }
],

    index: function () {
        var view = this.getMainView() || Ext.widget('articleTsjGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ArticleTsj').load();
    }
});