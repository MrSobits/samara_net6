Ext.define('B4.controller.dict.ResolveGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ResolveGji'],
    stores: ['dict.ResolveGji'],

    views: ['dict.resolvegji.Grid'],

    mainView: 'dict.resolvegji.Grid',
    mainViewSelector: 'resolveGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'resolveGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'resolveGjiGrid',
            permissionPrefix: 'GkhGji.Dict.Resolve'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'resolveGjiGridAspect',
            storeName: 'dict.ResolveGji',
            modelName: 'dict.ResolveGji',
            gridSelector: 'resolveGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('resolveGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ResolveGji').load();
    }
});