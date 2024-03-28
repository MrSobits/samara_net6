Ext.define('B4.controller.dict.RevenueSourceGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.RevenueSourceGji'],
    stores: ['dict.RevenueSourceGji'],

    views: ['dict.revenuesourcegji.Grid'],

    mainView: 'dict.revenuesourcegji.Grid',
    mainViewSelector: 'revenueSourceGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'revenueSourceGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'revenueSourceGjiGrid',
            permissionPrefix: 'GkhGji.Dict.RevenueSource'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'revenueSourceGjiGridAspect',
            storeName: 'dict.RevenueSourceGji',
            modelName: 'dict.RevenueSourceGji',
            gridSelector: 'revenueSourceGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('revenueSourceGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.RevenueSourceGji').load();
    }
});