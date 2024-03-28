Ext.define('B4.controller.dict.ArticleLawGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ArticleLawGji'],
    stores: ['dict.ArticleLawGji'],

    views: ['dict.articlelawgji.Grid'],

    mainView: 'dict.articlelawgji.Grid',
    mainViewSelector: 'articleLawGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'articleLawGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'articleLawGjiGrid',
            permissionPrefix: 'GkhGji.Dict.ArticleLaw'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'articleLawGjiGridAspect',
            storeName: 'dict.ArticleLawGji',
            modelName: 'dict.ArticleLawGji',
            gridSelector: 'articleLawGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('articleLawGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ArticleLawGji').load();
    }
});