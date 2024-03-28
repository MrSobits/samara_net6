Ext.define('B4.controller.dict.NotificationCause', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.NotificationCause'],
    stores: ['dict.NotificationCause'],

    views: ['dict.notificationcause.Grid'],

    mainView: 'dict.notificationcause.Grid',
    mainViewSelector: 'notificationcausegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'notificationcausegrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'notificationcausegrid',
            permissionPrefix: 'GkhGji.Dict.NotificationCause'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inlineGridAspect',
            storeName: 'dict.NotificationCause',
            modelName: 'dict.NotificationCause',
            gridSelector: 'notificationcausegrid'
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('notificationcausegrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});