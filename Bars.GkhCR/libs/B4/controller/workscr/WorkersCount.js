Ext.define('B4.controller.workscr.WorkersCount', {
    /*
    * Контроллер раздела количество рабочих
    */
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.InlineGrid',
         'B4.aspects.permission.typeworkcr.WorkersCount'
    ],

    models: ['objectcr.TypeWorkCr', 'objectcr.MonitoringSmr'],
    stores: ['objectcr.WorkersCountWork'],
    views: ['objectcr.WorkersCountWorkGrid'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'objectcr.WorkersCountWorkGrid',
    mainViewSelector: 'objectcrworkersgrid',

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'workersCountWorkGridAspect',
            storeName: 'objectcr.WorkersCountWork',
            modelName: 'objectcr.TypeWorkCr',
            gridSelector: 'objectcrworkersgrid'
        },
        {
            xtype: 'workerscounttypeworkcrperm',
            name: 'workersCountWorkObjectCrPerm',
            editFormAspectName: 'workersCountWorkGridAspect'
        }
    ],

    index: function(id, objectId) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('objectcrworkersgrid');

            view.getStore().on('beforeload',
                function(s, operation) {
                    operation.params.twId = id;
                    operation.params.objectCrId = objectId;
                }, me);
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();
    },

    onMainViewAfterRender: function() {
        var me = this,
            aspect;
        
        aspect = me.getAspect('workersCountWorkObjectCrPerm');

        me.mask('Загрузка', me.getMainComponent());
        B4.Ajax.request(B4.Url.action('GetByObjectCrId', 'MonitoringSmr', {
            objectCrId: me.getObjectId()
        })).next(function(response) {
            var obj = Ext.JSON.decode(response.responseText),
                model = me.getModel('objectcr.MonitoringSmr');

            model.load(obj.MonitoringSmrId, {
                success: function(rec) {
                    aspect.setPermissionsByRecord(rec);
                },
                scope: me
            });
            me.unmask();
            return true;
        }).error(function() {
            me.unmask();
            Ext.Msg.alert('Сообщение', 'Произошла ошибка');
        });
    },
    
    getObjectId: function () {
        var me = this;
        return me.getContextValue(me.getMainView(), 'objectId');
    }
});