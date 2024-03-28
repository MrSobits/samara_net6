Ext.define('B4.controller.dict.RedtapeFlagGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.RedtapeFlagGji'],
    stores: ['dict.RedtapeFlagGji'],

    views: ['dict.redtapeflaggji.Grid'],

    mainView: 'dict.redtapeflaggji.Grid',
    mainViewSelector: 'redtapeFlagGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'redtapeFlagGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'redtapeFlagGjiGrid',
            permissionPrefix: 'GkhGji.Dict.RedtapeFlag'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'redtapeFlagGjiGridAspect',
            storeName: 'dict.RedtapeFlagGji',
            modelName: 'dict.RedtapeFlagGji',
            gridSelector: 'redtapeFlagGjiGrid'
        }
],

    index: function () {
        var view = this.getMainView() || Ext.widget('redtapeFlagGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.RedtapeFlagGji').load();
    }
});