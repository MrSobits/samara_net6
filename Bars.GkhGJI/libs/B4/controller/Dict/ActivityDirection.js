Ext.define('B4.controller.dict.ActivityDirection', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['ActivityDirection'],
    stores: ['dict.ActivityDirection'],

    views: ['dict.activitydirection.Grid'],

    mainView: 'dict.activitydirection.Grid',
    mainViewSelector: 'activitydirectiongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'activitydirectiongrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'activitydirectiongrid',
            permissionPrefix: 'GkhGji.Dict.ActivityDirection'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inlineGridAspect',
            storeName: 'dict.ActivityDirection',
            modelName: 'ActivityDirection',
            gridSelector: 'activitydirectiongrid'
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('activitydirectiongrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});