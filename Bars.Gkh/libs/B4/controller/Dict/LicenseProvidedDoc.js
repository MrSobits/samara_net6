Ext.define('B4.controller.dict.LicenseProvidedDoc', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.LicenseProvidedDoc'],
    stores: ['dict.LicenseProvidedDoc'],
    views: ['dict.licenseprovideddoc.Grid'],

    mainView: 'dict.licenseprovideddoc.Grid',
    mainViewSelector: 'licenseprovideddocgrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'licenseprovideddocgrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'licenseprovideddocgrid',
            permissionPrefix: 'Gkh.Dictionaries.LicenseProvidedDoc'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'licenseProvidedDocGridAspect',
            storeName: 'dict.LicenseProvidedDoc',
            modelName: 'dict.LicenseProvidedDoc',
            gridSelector: 'licenseprovideddocgrid'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('licenseprovideddocgrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});