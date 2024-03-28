Ext.define('B4.controller.objectcr.Edit', {
    /* 
    * Контроллер формы редактирования объектов капремонта
    */
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateContextButton',
        'B4.aspects.permission.ObjectCr'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
   
    models: ['ObjectCr'],
    views: ['objectcr.EditPanel'],
    
    mainView: 'objectcr.EditPanel',
    mainViewSelector: 'objectCrEditPanel',

    aspects: [
        {
            xtype: 'objectcrstateperm',
            editFormAspectName: 'objectCrEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statecontextbuttonaspect',
            name: 'objectCrStateButtonAspect',
            stateButtonSelector: 'objectCrEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('objectCrEditPanelAspect').setData(entityId);
                    asp.controller.getStore('objectcr.NavigationMenu').load();
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела объекта КР
            */
            xtype: 'gkheditpanel',
            name: 'objectCrEditPanelAspect',
            editPanelSelector: 'objectCrEditPanel',
            modelName: 'ObjectCr',
            listeners: {
                aftersetpaneldata: function (asp, rec) {
                    this.controller.getAspect('objectCrStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            }
        }
    ],

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('objectCrEditPanel');

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', 'id');
        me.application.deployView(view, 'objectcr_info');

        me.getAspect('objectCrEditPanelAspect').setData(id);
    }
});