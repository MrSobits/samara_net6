Ext.define('B4.controller.objectoutdoorcr.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateContextButton',
        'B4.aspects.permission.ObjectOutdoorCrEdit'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'objectoutdoorcr.ObjectOutdoorCr'
    ],

    views: [
        'objectoutdoorcr.EditPanel'
    ],

    stores: [
        'objectoutdoorcr.ObjectOutdoorCr'
    ],

    mainView: 'objectoutdoorcr.EditPanel',
    mainViewSelector: 'objectoutdoorcreditpanel',

    aspects: [
        {
            xtype: 'objectoutdoorcreditstateperm',
            editFormAspectName: 'objectOutdoorCrEditPanelAspect'
        },
        {
            xtype: 'gkheditpanel',
            name: 'objectOutdoorCrEditPanelAspect',
            editPanelSelector: 'objectoutdoorcreditpanel',
            modelName: 'objectoutdoorcr.ObjectOutdoorCr',
            listeners: {
                aftersetpaneldata: function (asp, rec) {
                    this.controller.getAspect('objectOutdoorCrStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statecontextbuttonaspect',
            name: 'objectOutdoorCrStateButtonAspect',
            stateButtonSelector: 'objectoutdoorcreditpanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    asp.controller.getAspect('objectOutdoorCrEditPanelAspect').setData(entityId);
                }
            }
        },
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view, 'object_outdoor_cr_info');
        me.getAspect('objectOutdoorCrEditPanelAspect').setData(id);
    }
});