Ext.define('B4.controller.constructionobject.Smr', {

    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateButton',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['constructionobject.Smr'],
    stores: ['constructionobject.NavigationMenu'],
    views: ['constructionobject.smr.EditPanel'],

    mainView: 'constructionobject.smr.EditPanel',
    mainViewSelector: 'constructionobjsmreditpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjsmreditpanel'
        }
    ],

    aspects: [
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'constructionobjsmrStateButtonAspect',
            stateButtonSelector: 'constructionobjsmreditpanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('constructionobjSmrEditPanelAspect').setData(entityId);
                    asp.controller.getStore('constructionobject.NavigationMenu').load();
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела объекта КР
            */
            xtype: 'gkheditpanel',
            name: 'constructionobjSmrEditPanelAspect',
            editPanelSelector: 'constructionobjsmreditpanel',
            modelName: 'constructionobject.Smr',
            listeners: {
                aftersetpaneldata: function(asp, rec) {
                    this.controller.getAspect('constructionobjsmrStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            }
        }
    ],

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('constructionobjsmreditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'constructionObjectId', id);
        me.application.deployView(view, 'construction_object_info');

        me.onMainViewAfterRender();
    },

    onMainViewAfterRender: function() {
        var aspect = this.getAspect('constructionobjSmrEditPanelAspect');

        this.mask('Загрузка', this.getMainView());
        B4.Ajax.request(B4.Url.action('SaveByConstructObjectId', 'ConstructObjMonitoringSmr', {
            constructObjId: this.getContextValue(this.getMainView(), 'constructionObjectId')
        })).next(function(response) {
            var obj = Ext.JSON.decode(response.responseText);
            aspect.setData(obj.MonitoringSmrId);
            this.unmask();
            return true;
        }, this).error(function() {
            this.unmask();
            Ext.Msg.alert('Сообщение', 'Произошла ошибка');
        }, this);
    }
});