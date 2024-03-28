Ext.define('B4.controller.specialobjectcr.ProgressExecutionWork', {
/*
* Контроллер раздела ход выполнения работы
*/
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: [
        'specialobjectcr.TypeWorkCr',
        'specialobjectcr.MonitoringSmr'
    ],
    stores: [
        'specialobjectcr.ProgressExecutionWork'
    ],
    views: [
        'specialobjectcr.ProgressExecutionWorkGrid',
        'specialobjectcr.ProgressExecutionWorkEditWindow'
    ],

    mainView: 'specialobjectcr.ProgressExecutionWorkGrid',
    mainViewSelector: 'specialobjectcrprogressexecutionworkgrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.specialobjectcr.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'progressExecutionWorkObjectCrPerm',
            editFormAspectName: 'progressExecutionWorkGridAspect',
            permissions: [
                {
                    name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.CalcPercentOfCompletion',
                    applyTo: 'button[name=CalcPercentOfCompletion]',
                    selector: 'specialobjectcrprogressexecutionworkgrid'
                },
                { name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Edit', applyTo: 'b4savebutton', selector: 'specialobjectcrprogressexecutionworkeditwin' },
                { name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_Edit', applyTo: '[name=StageWorkCr]', selector: 'specialobjectcrprogressexecutionworkeditwin' },
                { name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_View',
                    applyTo: '[name=StageWorkCr]',
                    selector: 'specialobjectcrprogressexecutionworkeditwin',
                    applyBy: function (component, allowed) {
                        if (component)
                            component.setVisible(allowed);
                    }
                },
                { name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_Edit', applyTo: '[name=ManufacturerName]', selector: 'specialobjectcrprogressexecutionworkeditwin' },
                { name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_View',
                    applyTo: '[name=ManufacturerName]',
                    selector: 'specialobjectcrprogressexecutionworkeditwin',
                    applyBy: function (component, allowed) {
                        if (component)
                            component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Column.StageWorkCr',
                    applyTo: '[dataIndex=StageWorkCrName]',
                    selector: 'specialobjectcrprogressexecutionworkgrid',
                    applyBy: function (component, allowed) {
                        if (component)
                            component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Column.Manufacturer',
                    applyTo: '[dataIndex=ManufacturerName]',
                    selector: 'specialobjectcrprogressexecutionworkgrid',
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
            xtype: 'grideditctxwindowaspect',
            name: 'progressExecutionWorkGridAspect',
            modelName: 'specialobjectcr.TypeWorkCr',
            gridSelector: 'specialobjectcrprogressexecutionworkgrid',
            editFormSelector: 'specialobjectcrprogressexecutionworkeditwin',
            editWindowView: 'specialobjectcr.ProgressExecutionWorkEditWindow',
        
            otherActions: function (actions) {
                var me = this;
                
                actions[me.editFormSelector + ' [name=StageWorkCr]'] = { 'beforeload': { fn: me.controller.onBeforeLoadStageSfl, scope: me } };
                actions[me.gridSelector + ' button[name=CalcPercentOfCompletion]'] = { 'click': { fn: me.onCalcPercentOfCompletion, scope: me } };
            },
            
            onCalcPercentOfCompletion: function() {
                var me = this,
                    grid = me.getGrid();

                me.controller.mask('Расчет...', me.controller.getMainComponent());

                B4.Ajax.request({
                    url: B4.Url.action('CalcPercentOfCompletion', 'SpecialTypeWorkCr'),
                    params: {
                        objectId: me.controller.getContextValue(me.controller.getMainComponent(), 'objectCrId')
                    },
                    timeout: 9999999
                }).next(function () {
                    me.controller.unmask();
                    
                    grid.getStore().load();
                    B4.QuickMsg.msg('Успешно', 'Расчет процента выполнения произведен успешно!', 'success');
                }).error(function (e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка!', (e.message || 'Во время выполнения расчета процента выполнения произошла ошибка'));
                });
            },
            
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('ObjectCr', me.controller.getContextValue(me.controller.getMainComponent(), 'objectCrId'));
                    }
                },
                aftersetformdata: function (asp, record) {
                    var me = this,
                    grid = me.getGrid();
                    me.controller.setContextValue(grid, 'workId', record.get('Work').Id);
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView(),
            store;

        if (!view) {
            view = Ext.widget('specialobjectcrprogressexecutionworkgrid');
            store = view.getStore();

            store.on('beforeload', function (store, operation) {
                operation.params.objectCrId = id;
            },
            me);
        }

        me.bindContext(view);
        me.setContextValue(view, 'objectCrId', id);
        me.application.deployView(view, 'specialobjectcr_info');
        me.setStatePermissions();

        store.load();
    },

    setStatePermissions: function () {
        var me = this,
            aspect;

            aspect = me.getAspect('progressExecutionWorkObjectCrPerm');

            me.mask('Загрузка', me.getMainView());
        B4.Ajax.request(B4.Url.action('GetByObjectCrId', 'SpecialMonitoringSmr', {
            objectCrId: me.getContextValue(me.getMainView(), 'objectCrId')
        })).next(function(response) {
            var obj = Ext.JSON.decode(response.responseText),
                model = me.getModel('specialobjectcr.MonitoringSmr');

            model.load(obj.MonitoringSmrId, {
                success: function(rec) {
                    aspect.setPermissionsByRecord(rec);
                },
                scope: me
            });
            me.unmask();
            return true;
        }, me).error(function() {
            me.unmask();
            Ext.Msg.alert('Сообщение', 'Произошла ошибка');
        }, me);

    },
    
    onBeforeLoadStageSfl: function (field, options) {
        options.params = {};
        options.params.workId = this.controller.getContextValue(this.controller.getMainComponent(), 'workId');
    }
});