Ext.define('B4.controller.workscr.Smr', {
    /*
    * Контроллер раздела график выполнения работ
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateButton',
        'B4.Ajax', 'B4.Url'
    ],

    models: ['objectcr.MonitoringSmr'],
    stores: ['objectcr.NavigationMenu'],
    views: ['objectcr.MonitoringSmrEditPanel'],

    mainView: 'objectcr.MonitoringSmrEditPanel',
    mainViewSelector: 'monitoringsmrpanel',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /*
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'monitoringSmrStateButtonAspect',
            stateButtonSelector: 'monitoringsmrpanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('monitoringSmrEditPanelAspect').setData(entityId);
                    asp.controller.getStore('objectcr.NavigationMenu').load();
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела объекта КР
            */
            xtype: 'gkheditpanel',
            name: 'editpanel',
            editPanelSelector: 'monitoringsmrpanel',
            modelName: 'objectcr.MonitoringSmr',
            listeners: {
                aftersetpaneldata: function(asp, rec) {
                    this.controller.getAspect('monitoringSmrStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            }
        }
    ],

    index: function(id, objectId) {
        var me = this,
            view = me.getMainView() || Ext.widget('monitoringsmrpanel');

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');
        me.loadSmr();
    },

    loadSmr: function() {
        var me = this,
            aspect = me.getAspect('editpanel');

        me.mask('Загрузка', me.getMainView());
        B4.Ajax.request(B4.Url.action('SaveByObjectCrId', 'MonitoringSmr', {
            objectCrId: me.getObjectId()
        })).next(function(response) {
            var obj = Ext.JSON.decode(response.responseText);
            aspect.setData(obj.MonitoringSmrId);
            me.unmask();
            return true;
        }).error(function() {
            me.unmask();
            Ext.Msg.alert('Сообщение', 'Произошла ошибка');
        });
    },

    getObjectId: function() {
        var me = this;
        me.getContextValue(me.getMainView(), 'objectId');
    }
});