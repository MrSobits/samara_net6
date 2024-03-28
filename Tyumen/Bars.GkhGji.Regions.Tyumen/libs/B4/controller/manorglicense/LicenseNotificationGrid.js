Ext.define('B4.controller.manorglicense.LicenseNotificationGrid', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
         'B4.aspects.GkhGridEditForm'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody',
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    stores: [
        'manorglicense.LicenseNotificationGis'
    ],

    models: [
        'manorglicense.LicenseNotificationGis'
    ],

    views: [
        'manorglicense.LicenseNotificationGrid',
        'manorglicense.LicenseNotificationEditWindow'
    ],

    mainView: 'manorglicense.LicenseNotificationGrid',
    mainViewSelector: 'manorglicensnotificationgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'manorglicensnotificationgrid'
        },
        {
            ref: 'notificationEditWindow',
            selector: 'manorglicensenotificationeditwindow'
        }
    ],

    aspects: [
         {
             xtype: 'gkhbuttonprintaspect',
             name: 'manOrgLicenseNotificationPrintAspect',
             buttonSelector: 'manorglicensenotificationeditwindow #btnPrint',
             codeForm: 'ManOrgLicenseNotification',
             getUserParams: function () {
                 var me = this,
                     param = { Id: me.controller.getContextValue(me.controller.getMainView(), 'licNotId') };

                 me.params.userParams = Ext.JSON.encode(param);
             }
         },
         {
             xtype: 'grideditwindowaspect',
             name: 'licenseNotificationGridWindowAspect',
             gridSelector: 'manorglicensnotificationgrid',
             editFormSelector: 'manorglicensenotificationeditwindow',
             modelName: 'manorglicense.LicenseNotificationGis',
             editWindowView: 'manorglicense.LicenseNotificationEditWindow',
             onSaveSuccess: function () {
                 // перекрываем чтобы окно незакрывалось после сохранения
                 B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
             },
             listeners: {
                 aftersetformdata: function (asp, rec, form) {
                     var me = this;
                     me.controller.setContextValue(me.controller.getMainView(), 'licNotId', rec.internalId);
                     me.controller.getAspect('manOrgLicenseNotificationPrintAspect').loadReportStore();
                 }
             }
         }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('manorglicensnotificationgrid');

        me.bindContext(view);
        this.application.deployView(view);
        //me.getAspect('manOrgLicenseNotificationGisEditPanelAspect').setData(id);
        this.getStore('manorglicense.LicenseNotificationGis').load();
    },

    init: function () {
        var me = this,
            actions = {};

        me.callParent(arguments);
    }
});