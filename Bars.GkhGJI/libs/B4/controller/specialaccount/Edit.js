Ext.define('B4.controller.specialaccount.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGrid'
    ],

    afterset: null,

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody',
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    stores: [
        'specialaccount.SpecialAccountRow',
        'specialaccount.SpecialAccountReport'
    ],

    models: [
        'specialaccount.SpecialAccountRow',
        'specialaccount.SpecialAccountReport'
    ],

    views: [
        'specialaccount.EditPanel',
        'specialaccount.RowGrid',
        'specialaccount.Grid',
    ],

    mainView: 'specialaccount.EditPanel',
    mainViewSelector: 'specialaccountEditPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'specialaccountEditPanel'
        },
      
        {
            ref: 'specialAccountRowGrid',
            selector: 'specialaccountrowgrid'
        },
        //{
        //    ref: 'notificationGisEditWindow',
        //    selector: 'manorglicensenotificationgiseditwindow'
        //}
    ],

    aspects: [    
         {
             xtype: 'gkheditpanel',
             name: 'specialaccountEditPanelAspect',
             editPanelSelector: 'specialaccountEditPanel',
             storeName: 'specialaccount.SpecialAccountReport',
             modelName: 'specialaccount.SpecialAccountReport',

             listeners: {

                 aftersetpaneldata: function (asp, rec, panel) {
                     
                     var me = this,
                         notificationGisGrid = panel.down('specialaccountrowgrid'),
                         docStore = notificationGisGrid.getStore();
                     docStore.clearFilter(true);
                     docStore.filter('specialAccountReportId', me.objectId);
                     docStore.load();
                     debugger;
                     if (rec.raw.Sertificate != 'Не подписан' && rec.raw.Sertificate != 'Отчет сдан в письменном виде')
                         panel.down('specialaccountrowgrid').setDisabled(true);
                     else if (!afterset){
                         Ext.Msg.alert('Внимание!', 'Убедитесь что размерность рубли/тысячи указано верно и соответствует внесенным суммам!');
                     }
                     afterset = true;
                 }
             }
         },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'specialaccountRowGridAspect',
            storeName: 'specialaccount.SpecialAccountRow',
            modelName: 'specialaccount.SpecialAccountRow',
            gridSelector: 'specialaccountrowgrid'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('specialaccountEditPanel');

        me.bindContext(view);
        me.setContextValue(view, 'id', id);
        debugger;
        me.application.deployView(view);
        me.getAspect('specialaccountEditPanelAspect').setData(id);
        
    },

    init: function () {
        var me = this,
            actions = {};
        afterset = false;
        me.callParent(arguments);

    },
    reporteditor: function (id) {
        debugger;
        var me = this,
            view = me.getMainView() || Ext.widget('specialaccountEditPanel', {
                controllerName: 'SpecialAccountRow',
                passportControllerName: 'SpecialAccountReport',
                passpId: id
            });

        me.bindContext(view);
        me.application.deployView(view);
    },
});