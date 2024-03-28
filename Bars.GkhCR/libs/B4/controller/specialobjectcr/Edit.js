Ext.define('B4.controller.specialobjectcr.Edit', {
    /* 
    * Контроллер формы редактирования объектов капремонта
    */
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateContextButton',
        'B4.aspects.permission.SpecialObjectCr'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
   
    models: [
        'specialobjectcr.SpecialObjectCr'
    ],
    views: [
        'specialobjectcr.EditPanel'
    ],
    
    mainView: 'specialobjectcr.EditPanel',
    mainViewSelector: 'specialobjectcreditpanel',

    aspects: [
        {
            xtype: 'specialobjectcrstateperm',
            editFormAspectName: 'objectCrEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statecontextbuttonaspect',
            name: 'objectCrStateButtonAspect',
            stateButtonSelector: 'specialobjectcreditpanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('objectCrEditPanelAspect').setData(entityId);
                    asp.controller.getStore('specialobjectcr.NavigationMenu').load();
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела объекта КР
            */
            xtype: 'gkheditpanel',
            name: 'objectCrEditPanelAspect',
            editPanelSelector: 'specialobjectcreditpanel',
            modelName: 'specialobjectcr.SpecialObjectCr',
            listeners: {
                aftersetpaneldata: function (asp, rec) {
                    this.controller.getAspect('objectCrStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            }
        }
    ],

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('specialobjectcreditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', 'id');
        me.application.deployView(view, 'specialobjectcr_info');

        me.getAspect('objectCrEditPanelAspect').setData(id);
    }
});