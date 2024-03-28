Ext.define('B4.controller.manorglicense.EditLicenseNotificationGis', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody',
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    stores: [
        'manorglicense.LicenseGis',
        'manorglicense.LicenseNotificationGis'
    ],

    models: [
        'manorglicense.LicenseGis',
        'manorglicense.LicenseNotificationGis'
    ],

    views: [
        'manorglicense.EditLicenseNotificationGisPanel',
        'manorglicense.LicenseNotificationGisGrid',
        'manorglicense.LicenseNotificationGisEditWindow'
    ],

    mainView: 'manorglicense.EditLicenseNotificationGisPanel',
    mainViewSelector: 'manOrgLicenseNotificationGisEditPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'manOrgLicenseNotificationGisEditPanel'
        },
        {
            ref: 'notificationGisGrid',
            selector: 'manorglicensegridgis'
        },
        {
            ref: 'notificationGisEditWindow',
            selector: 'manorglicensenotificationgiseditwindow'
        }
    ],

    aspects: [
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'manOrgLicenseNotificationPrintAspect',
            buttonSelector: 'manorglicensenotificationgiseditwindow #btnPrint',
            codeForm: 'ManOrgLicenseNotification',
            getUserParams: function () {
                debugger;
                var me = this,
                    param = { Id: me.controller.getContextValue(me.controller.getMainView(), 'licNotId') };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
         {
             xtype: 'gkheditpanel',
             name: 'manOrgLicenseNotificationGisEditPanelAspect',
             editPanelSelector: 'manOrgLicenseNotificationGisEditPanel',
             // storeName: 'manorglicense.LicenseGis',
             modelName: 'manorglicense.LicenseGis',

             listeners: {

                 aftersetpaneldata: function (asp, rec, panel) {

                     var me = this,
                         notificationGisGrid = panel.down('manorglicensnotificationgisgrid'),
                         docStore = notificationGisGrid.getStore();

                     docStore.clearFilter(true);
                     docStore.filter('mcid', me.objectId);
                 }
             }
         },
         {
             xtype: 'grideditwindowaspect',
             name: 'licenseNotificationGisGridWindowAspect',
             gridSelector: 'manorglicensnotificationgisgrid',
             editFormSelector: 'manorglicensenotificationgiseditwindow',
             modelName: 'manorglicense.LicenseNotificationGis',
             editWindowView: 'manorglicense.LicenseNotificationGisEditWindow',
             onSaveSuccess: function () {
                 // перекрываем чтобы окно незакрывалось после сохранения
                 B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
             },
             listeners: {
                 getdata: function (asp, record) {
                     var me = this;
                     if (!record.data.Id) {
                         record.data.ManagingOrgRealityObject = me.controller.getContextValue(me.controller.getMainView(), 'mcid');
                     }
                 },
                 aftersetformdata: function (asp, rec, form) {
                     var me = this;
                     me.controller.setContextValue(me.controller.getMainView(), 'licNotId', rec.internalId);
                     me.controller.getAspect('manOrgLicenseNotificationPrintAspect').loadReportStore();
                 }
             }
         }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('manOrgLicenseNotificationGisEditPanel');
        
        me.bindContext(view);
        me.setContextValue(view, 'mcid', id);
        me.application.deployView(view, 'licensegis_info');
        me.getAspect('manOrgLicenseNotificationGisEditPanelAspect').setData(id);
    },

    init: function () {
        var me = this,
            actions = {};

        me.callParent(arguments);
    }
});