Ext.define('B4.controller.specialobjectcr.WorkersCountWork', {
/*
* Контроллер раздела количество рабочих
*/
    extend: 'B4.controller.MenuItemController',
    views: [
        'specialobjectcr.WorkersCountWorkGrid'
    ],

    params: {},

    requires:
    [
         'B4.aspects.InlineGrid',
         'B4.aspects.permission.specialobjectcr.WorkersCount'
    ],

    models: [
        'specialobjectcr.TypeWorkCr',
        'specialobjectcr.MonitoringSmr'
    ],
    stores: [
        'specialobjectcr.WorkersCountWork'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'specialobjectcr.WorkersCountWorkGrid',
    mainViewSelector: 'specialobjectcrworkersgrid',

    parentCtrlCls: 'B4.controller.specialobjectcr.Navi',

    aspects: [
        {
            xtype: 'workerscountspecialobjectcrperm',
            name: 'workersCountWorkObjectCrPerm',
            editFormAspectName: 'workersCountWorkGridAspect'
        },
        {
        /*
        * Аспект взаимодействия таблицы и формы редактирования раздела количество рабочих
        */
            xtype: 'inlinegridaspect',
            name: 'workersCountWorkGridAspect',
            modelName: 'specialobjectcr.TypeWorkCr',
            gridSelector: 'specialobjectcrworkersgrid'
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

        B4.Ajax.request(B4.Url.action('GetByObjectCrId', 'SpecialMonitoringSmr', {
            objectCrId: objectcrId
        })).next(function (response) {
            var obj = Ext.JSON.decode(response.responseText);
            var model = this.getModel('specialobjectcr.MonitoringSmr');

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
            view = me.getMainView() || Ext.widget('specialobjectcrworkersgrid'),
            store;

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'specialobjectcr_info');
        
        store = view.getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);
    }
});