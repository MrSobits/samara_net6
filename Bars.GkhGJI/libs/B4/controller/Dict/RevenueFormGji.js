Ext.define('B4.controller.dict.RevenueFormGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.RevenueFormGji'],
    stores: ['dict.RevenueFormGji'],

    views: ['dict.revenueformgji.Grid'],

    mainView: 'dict.revenueformgji.Grid',
    mainViewSelector: 'revenueFormGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'revenueFormGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'revenueFormGjiGrid',
            permissionPrefix: 'GkhGji.Dict.RevenueForm'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'revenueFormGjiGridAspect',
            storeName: 'dict.RevenueFormGji',
            modelName: 'dict.RevenueFormGji',
            gridSelector: 'revenueFormGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('revenueFormGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.RevenueFormGji').load();
    }
});