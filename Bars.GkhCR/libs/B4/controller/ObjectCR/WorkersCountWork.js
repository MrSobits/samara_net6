Ext.define('B4.controller.objectcr.WorkersCountWork', {
/*
* Контроллер раздела количество рабочих
*/
    extend: 'B4.controller.MenuItemController',
    views: ['objectcr.WorkersCountWorkGrid'],

    params: {},

    requires:
    [
         'B4.aspects.InlineGrid',
         'B4.aspects.permission.objectcr.WorkersCount'
    ],

    models: ['objectcr.TypeWorkCr', 'objectcr.MonitoringSmr'],
    stores: ['objectcr.WorkersCountWork'],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'objectcr.WorkersCountWorkGrid',
    mainViewSelector: 'objectcrworkersgrid',

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [
        {
            xtype: 'workerscountobjectcrperm',
            name: 'workersCountWorkObjectCrPerm',
            editFormAspectName: 'workersCountWorkGridAspect'
        },
        {
        /*
        * Аспект взаимодействия таблицы и формы редактирования раздела количество рабочих
        */
            xtype: 'inlinegridaspect',
            name: 'workersCountWorkGridAspect',
            modelName: 'objectcr.TypeWorkCr',
            gridSelector: 'objectcrworkersgrid'
        }
    ],

    init: function () {
        var actions = {};
        actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    onMainViewAfterRender: function () {
        var me = this,
            objectcrId = me.getContextValue(me.getMainComponent(), 'objectcrId'),
            aspect = me.getAspect('workersCountWorkObjectCrPerm');

            this.mask('Загрузка', this.getMainComponent());
            B4.Ajax.request(B4.Url.action('GetByObjectCrId', 'MonitoringSmr', {
                objectCrId: objectcrId
            })).next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);
                var model = this.getModel('objectcr.MonitoringSmr');

                model.load(obj.MonitoringSmrId, {
                    success: function (rec) {
                        aspect.setPermissionsByRecord(rec);
                    },
                    scope: this
                });
                me.unmask();
                return true;
            }, me).error(function () {
                me.unmask();
                Ext.Msg.alert('Сообщение', 'Произошла ошибка');
            }, me);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('objectcrworkersgrid'),
            store;

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');
        
        store = view.getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);
    }
});