/*
* Документы
*/
Ext.define('B4.controller.workscr.Document', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.typeworkcr.Document'
    ],

    models: [
        'objectcr.DocumentWorkCr',
        'objectcr.MonitoringSmr'
    ],

    stores: ['objectcr.DocumentWorkCr'],

    views: [
        'objectcr.DocumentWorkCrEditWindow',
        'objectcr.DocumentWorkCrGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'objectcr.DocumentWorkCrGrid',
    mainViewSelector: 'objectcrdocumentgrid',

    aspects: [
        {
            /**
            * Аспект взаимодействия таблицы и формы редактирования документов
            */
            xtype: 'grideditwindowaspect',
            name: 'documentWorkCrGridAspect',
            storeName: 'objectcr.DocumentWorkCr',
            modelName: 'objectcr.DocumentWorkCr',
            gridSelector: 'objectcrdocumentgrid',
            editFormSelector: 'objectcrdocumentwin',
            editWindowView: 'objectcr.DocumentWorkCrEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('ObjectCr', asp.controller.getObjectId());
                        record.set('TypeWork', asp.controller.getTypeWorkId());
                    }
                }
            }
        },
        {
            xtype: 'documenttypeworkcrperm',
            name: 'documentTypeWorkCrPerm',
            editFormAspectName: 'documentWorkCrGridAspect'
        }
    ],

    index: function(id, objectId) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('objectcrdocumentgrid');

            view.getStore().on('beforeload',
                function(s, operation) {
                    operation.params.twId = id;
                    operation.params.objectCrId = objectId;
                });
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();
    },

    onMainViewAfterRender: function() {
        var me = this,
            aspect = me.getAspect('documentTypeWorkCrPerm');

        me.mask('Загрузка', me.getMainView());

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

    getTypeWorkId: function() {
        var me = this;
        return me.getContextValue(me.getMainView(), 'twId');
    },

    getObjectId: function() {
        var me = this;
        return me.getContextValue(me.getMainView(), 'objectId');
    }
});