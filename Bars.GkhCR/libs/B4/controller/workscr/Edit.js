/* 
* Контроллер формы редактирования объектов капремонта
*/
Ext.define('B4.controller.workscr.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.permission.TypeWorkCr',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateButton'
    ],

    models: ['objectcr.TypeWorkCr'],
    stores: ['dict.Official'],
    views: ['workscr.EditPanel'],

    mainView: 'workscr.EditPanel',
    mainViewSelector: 'workscreditpanel',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    parentCtrlCls: 'B4.controller.workscr.Navi',

    aspects: [
        {
            xtype: 'typeworkcrstateperm',
            editFormAspectName: 'workcrEditPanel'
        },
        {
            /*
             Вешаем аспект смены статуса в карточке редактирования
             */
            xtype: 'statebuttonaspect',
            name: 'workscrStateButtonAspect',
            stateButtonSelector: 'workscreditpanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    asp.controller.getAspect('workcrEditPanel').setData(entityId);
                }
            }
        },
        {
            xtype: 'gkheditpanel',
            name: 'workcrEditPanel',
            editPanelSelector: 'workscreditpanel',
            modelName: 'objectcr.TypeWorkCr',
            listeners: {
                aftersetpaneldata: function (asp, rec) {
                    asp.controller.getAspect('workscrStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            }
        }
    ],

    index: function(id, objectId) {
        var me = this,
            view = me.getMainView() || Ext.widget('workscreditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        me.getAspect('workcrEditPanel').setData(id);
    }
});