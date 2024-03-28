Ext.define('B4.controller.dict.LicenseRejectReason', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: ['dict.licenserejectreason.Grid'],

    mainView: 'dict.licenserejectreason.Grid',
    mainViewSelector: 'licenserejectreasongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'licenserejectreasongrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'licenserejectreasongrid',
            permissionPrefix: 'Gkh.Dictionaries.LicenseRejectReason'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inlineGridAspect',
            storeName: 'dict.LicenseRejectReason',
            modelName: 'dict.LicenseRejectReason',
            gridSelector: 'licenserejectreasongrid'
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('licenserejectreasongrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});