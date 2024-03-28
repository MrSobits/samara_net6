Ext.define('B4.controller.workscr.ProgressExecution', {
    /*
    * Контроллер раздела ход выполнения работы
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: [
        'objectcr.TypeWorkCr',
        'objectcr.MonitoringSmr'
    ],
    
    stores: ['objectcr.ProgressExecutionWork'],
    
    views: [
        'objectcr.ProgressExecutionWorkGrid',
        'objectcr.ProgressExecutionWorkEditWindow'
    ],

    mainView: 'objectcr.ProgressExecutionWorkGrid',
    mainViewSelector: 'progressexecutionworkgrid',
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'progressExecutionWorkTypeWorkCrPerm',
            editFormAspectName: 'progressExecutionWorkGridAspect',
            permissions: [
                {
                    name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.CalcPercentOfCompletion',
                    applyTo: 'button[name=CalcPercentOfCompletion]',
                    selector: 'progressexecutionworkgrid'
                },
                { name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Edit', applyTo: 'b4savebutton', selector: 'progressexecutionworkeditwin' },
                { name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_Edit', applyTo: '[name=StageWorkCr]', selector: 'progressexecutionworkeditwin' },
                {
                    name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_View',
                    applyTo: '[name=StageWorkCr]',
                    selector: 'progressexecutionworkeditwin',
                    applyBy: function (component, allowed) {
                        if (component)
                            component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_Edit',
                    applyTo: '[name=ManufacturerName]',
                    selector: 'progressexecutionworkeditwin'
                },
                {
                    name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_View',
                    applyTo: '[name=ManufacturerName]',
                    selector: 'progressexecutionworkeditwin',
                    applyBy: function (component, allowed) {
                        if (component)
                            component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Column.StageWorkCr',
                    applyTo: '[dataIndex=StageWorkCrName]',
                    selector: 'progressexecutionworkgrid',
                    applyBy: function (component, allowed) {
                        if (component)
                            component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Column.Manufacturer',
                    applyTo: '[dataIndex=ManufacturerName]',
                    selector: 'progressexecutionworkgrid',
                    applyBy: function (component, allowed) {
                        if (component)
                            component.setVisible(allowed);
                    }
                }
            ]
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела ход выполнения работы
            */
            xtype: 'grideditwindowaspect',
            name: 'progressExecutionWorkGridAspect',
            storeName: 'objectcr.ProgressExecutionWork',
            modelName: 'objectcr.TypeWorkCr',
            gridSelector: 'progressexecutionworkgrid',
            editFormSelector: 'progressexecutionworkeditwin',
            editWindowView: 'objectcr.ProgressExecutionWorkEditWindow',

            otherActions: function(actions) {
                var me = this;

                actions[me.editFormSelector + ' #sflStageWorkCr'] = { 'beforeload': { fn: me.controller.onBeforeLoadStageSfl, scope: me } };
                actions[me.gridSelector + ' button[name=CalcPercentOfCompletion]'] = { 'click': { fn: me.onCalcPercentOfCompletion, scope: me } };
            },

            onCalcPercentOfCompletion: function () {
                var me = this,
                    grid = me.getGrid();

                B4.Ajax.request({
                    url: B4.Url.action('CalcPercentOfCompletion', 'TypeWorkCr'),
                    params: {
                        objectId: me.controller.getObjectId()
                    },
                    timeout: 9999999
                }).next(function() {
                    me.controller.unmask();

                    grid.getStore().load();
                    B4.QuickMsg.msg('Успешно', 'Расчет процента выполнения произведен успешно!', 'success');
                }).error(function(e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка!', (e.message || 'Во время выполнения расчета процента выполнения произошла ошибка'));
                });
            },

            listeners: {
                getdata: function (asp, record) {
                    if (asp.controller.params && !record.get('Id')) {
                        record.set('ObjectCr', asp.controller.getObjectId());
                    }
                },
                aftersetformdata: function (asp, record) {
                    asp.controller.workId = record.get('Work').Id;
                }
            }
        }
    ],

    index: function (id, objectId) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('progressexecutionworkgrid');

            view.getStore().on('beforeload',
                function (s, operation) {
                    operation.params.twId = id;
                    operation.params.objectCrId = objectId;
                }, me);
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();
        me.setStatePermissions(objectId);
    },

    getObjectId: function () {
        var me = this;
        me.getContextValue(me.getMainView(), 'objectId');
    },

    setStatePermissions: function (objectId) {
        var me = this,
            aspect = me.getAspect('progressExecutionWorkTypeWorkCrPerm');

        me.mask('Загрузка', me.getMainComponent());
        
        B4.Ajax.request(B4.Url.action('GetByObjectCrId', 'MonitoringSmr', {
            objectCrId: objectId
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

    onBeforeLoadStageSfl: function (field, options) {
        options.params = {};
        options.params.workId = this.controller.workId;
    }
});