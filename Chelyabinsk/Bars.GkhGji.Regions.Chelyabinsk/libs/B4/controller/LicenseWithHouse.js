Ext.define('B4.controller.LicenseWithHouse', {
    extend: 'B4.base.Controller',
    requires: [
        //'B4.aspects.GkhGridEditForm',
        'B4.aspects.StateContextMenu',
        'B4.controller.manorglicense.Navi',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    models: ['LicenseWithHouse'],
    stores: ['LicenseWithHouse'],
    views: [
        'LicenseWithHouseGrid'
    ],

    mainView: 'LicenseWithHouseGrid',
    mainViewSelector: 'licensewithhousegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'licensewithhousegrid'
        }
    ],


    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('licensewithhousegrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});