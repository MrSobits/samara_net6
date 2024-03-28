Ext.define('B4.controller.dict.LicenseRegistrationReason', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: ['dict.licenseregistrationreason.Grid'],

    mainView: 'dict.licenseregistrationreason.Grid',
    mainViewSelector: 'licenseregistrationreasongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'licenseregistrationreasongrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'licenseregistrationreasongrid',
            permissionPrefix: 'Gkh.Dictionaries.LicenseRegistrationReason'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inlineGridAspect',
            storeName: 'dict.LicenseRegistrationReason',
            modelName: 'dict.LicenseRegistrationReason',
            gridSelector: 'licenseregistrationreasongrid'
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('licenseregistrationreasongrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});