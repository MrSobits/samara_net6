Ext.define('B4.controller.dict.KindWorkNotifGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.KindWorkNotifGji'],
    stores: ['dict.KindWorkNotifGji'],

    views: ['dict.kindworknotifgji.Grid'],

    mainView: 'dict.kindworknotifgji.Grid',
    mainViewSelector: 'kindWorkNotifGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'kindWorkNotifGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'kindWorkNotifGjiGrid',
            permissionPrefix: 'GkhGji.Dict.KindWorkNotif'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'kindWorkNotificationGjiGridAspect',
            storeName: 'dict.KindWorkNotifGji',
            modelName: 'dict.KindWorkNotifGji',
            gridSelector: 'kindWorkNotifGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('kindWorkNotifGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.KindWorkNotifGji').load();
    }
});