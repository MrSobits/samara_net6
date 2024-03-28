Ext.define('B4.controller.objectcr.MonitoringSmr', {
/*
* Контроллер раздела график выполнения работ
*/
    extend: 'B4.controller.MenuItemController',
    requires:
    [
        'B4.aspects.StateContextButton',
        'B4.aspects.StateButton',
        'B4.Ajax',
        'B4.Url'
    ],

    models: ['objectcr.MonitoringSmr'],
    stores: ['objectcr.NavigationMenu'],
    views: ['objectcr.MonitoringSmrEditPanel'],
    
    mainView: 'objectcr.MonitoringSmrEditPanel',
    mainViewSelector: 'monitoringsmrpanel',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [
      {
          /*
          *Вешаем аспект смены статуса в карточке редактирования
          */
          xtype: 'statebuttonaspect',
          name: 'statecontextbuttonaspect',
          stateButtonSelector: 'monitoringsmrpanel #btnState',
          listeners: {
              transfersuccess: function (asp, entityId) {
                  //После успешной смены статуса запрашиваем по Id актуальные данные записи
                  //и обновляем панель
                  asp.controller.getAspect('monitoringSmrEditPanelAspect').setData(entityId);
              }
          }
      },
      {
          /*
          * Аспект взаимодействия таблицы и формы редактирования раздела объекта КР
          */
          xtype: 'gkheditpanel',
          name: 'monitoringSmrEditPanelAspect',
          editPanelSelector: 'monitoringsmrpanel',
          modelName: 'objectcr.MonitoringSmr',
          listeners: {
              aftersetpaneldata: function (asp, rec) {
                  this.controller.getAspect('monitoringSmrStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
              }
          }
      }
    ],

    init: function () {

        var actions = {};
        actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('monitoringsmrpanel');

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');
    },

    onMainViewAfterRender: function () {
        if (this.params) {
            var aspect = this.getAspect('monitoringSmrEditPanelAspect'),
                objectcrId = me.getContextValue(me.getMainView(), 'objectcrId');

            this.mask('Загрузка', this.getMainComponent());
            B4.Ajax.request(B4.Url.action('SaveByObjectCrId', 'MonitoringSmr', {
                objectCrId: objectcrId
            })).next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);
                aspect.setData(obj.MonitoringSmrId);
                this.unmask();
                return true;
            }, this).error(function () {
                this.unmask();
                Ext.Msg.alert('Сообщение', 'Произошла ошибка');
            }, this);
        }
    }
});