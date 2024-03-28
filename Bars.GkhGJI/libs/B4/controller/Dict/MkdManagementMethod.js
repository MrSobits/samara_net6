Ext.define('B4.controller.dict.MkdManagementMethod', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.MkdManagementMethod'],
    stores: ['dict.MkdManagementMethod'],

    views: ['dict.mkdmanagementmethod.Grid'],

    mainView: 'dict.mkdmanagementmethod.Grid',
    mainViewSelector: 'mkdmanagementmethodgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'mkdmanagementmethodgrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'mkdmanagementmethodgrid',
            permissionPrefix: 'GkhGji.Dict.MkdManagementMethod'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inlineGridAspect',
            storeName: 'dict.MkdManagementMethod',
            modelName: 'dict.MkdManagementMethod',
            gridSelector: 'mkdmanagementmethodgrid'
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('mkdmanagementmethodgrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});