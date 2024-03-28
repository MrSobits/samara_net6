Ext.define('B4.controller.LicenseWithHouseByType', {
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

    models: ['LicenseWithHouseByType'],
    stores: ['LicenseWithHouseByType'],
    views: [
        'LicenseWithHouseByTypeGrid'
    ],

    mainView: 'LicenseWithHouseByTypeGrid',
    mainViewSelector: 'licensewithhousebytypegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'licensewithhousebytypegrid'
        }
    ],


    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('licensewithhousebytypegrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});