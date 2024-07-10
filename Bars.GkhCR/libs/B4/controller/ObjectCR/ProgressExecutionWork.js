Ext.define('B4.controller.objectcr.ProgressExecutionWork', {
/*
* Контроллер раздела ход выполнения работы
*/
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: ['objectcr.TypeWorkCr', 'objectcr.MonitoringSmr', 'objectcr.ArchiveMultiplyContragentSmr','objectcr.BuildControlTypeWorkSmr', 'objectcr.BuildControlTypeWorkSmrFile'],
    stores: ['objectcr.ProgressExecutionWork','objectcr.ArchiveMultiplyContragentSmr','objectcr.BuildControlTypeWorkSmr', 'objectcr.BuildControlTypeWorkSmrFile'],
    views: [
        'objectcr.ProgressExecutionWorkGrid',
        'objectcr.ProgressExecutionWorkEditWindow',
        'objectcr.ArchiveMultiplyContragentSmrGrid',
        'objectcr.ArchiveMultiplyContragentSmrEditWindow',
        'objectcr.ArchiveMultiplyContragentSmrGrid',
        'objectcr.ArchiveMultiplyContragentSmrEditWindow',
        'objectcr.BuildControlTypeWorkSmrEditWindow',
        'objectcr.BuildControlTypeWorkSmrGrid',
        'objectcr.FileInfoGrid',
        'objectcr.FileInfoEditWindow'
    ],
    typeWorkId: null,
    skId: null,
    mainView: 'objectcr.ProgressExecutionWorkGrid',
    mainViewSelector: 'progressexecutionworkgrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'progressExecutionWorkObjectCrPerm',
            editFormAspectName: 'progressExecutionWorkGridAspect',
            permissions: [
                {
                    name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.CalcPercentOfCompletion',
                    applyTo: 'button[name=CalcPercentOfCompletion]',
                    selector: 'progressexecutionworkgrid'
                },
                { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Edit', applyTo: 'b4savebutton', selector: 'progressexecutionworkeditwin' },
                { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_Edit', applyTo: '[name=StageWorkCr]', selector: 'progressexecutionworkeditwin' },
                { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_View',
                    applyTo: '[name=StageWorkCr]',
                    selector: 'progressexecutionworkeditwin',
                    applyBy: function (component, allowed) {
                        if (component)
                            component.setVisible(allowed);
                    }
                },
                { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_Edit', applyTo: '[name=ManufacturerName]', selector: 'progressexecutionworkeditwin' },
                { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_View',
                    applyTo: '[name=ManufacturerName]',
                    selector: 'progressexecutionworkeditwin',
                    applyBy: function (component, allowed) {
                        if (component)
                            component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Column.StageWorkCr',
                    applyTo: '[dataIndex=StageWorkCrName]',
                    selector: 'progressexecutionworkgrid',
                    applyBy: function (component, allowed) {
                        if (component)
                            component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Column.Manufacturer',
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
            xtype: 'grideditctxwindowaspect',
            name: 'progressExecutionWorkGridAspect',
            modelName: 'objectcr.TypeWorkCr',
            gridSelector: 'progressexecutionworkgrid',
            editFormSelector: 'progressexecutionworkeditwin',
            editWindowView: 'objectcr.ProgressExecutionWorkEditWindow',
        
            otherActions: function (actions) {
                var me = this;
                actions['progressexecutionworkeditwin #sflStageWorkCr'] = { 'beforeload': { fn: me.controller.onBeforeLoadStageSfl, scope: me } };
                actions[me.gridSelector + ' button[name=CalcPercentOfCompletion]'] = { 'click': { fn: me.onCalcPercentOfCompletion, scope: me } };
            },
            
            onCalcPercentOfCompletion: function() {
                var me = this,
                    grid = me.getGrid();

                me.controller.mask('Расчет...', me.controller.getMainComponent());

                B4.Ajax.request({
                    url: B4.Url.action('CalcPercentOfCompletion', 'TypeWorkCr'),
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
                    typeWorkId = record.getId();

                    var form = asp.getForm(),
                        archGrid = form.down('archivemultiplycontragentsmrgrid'),
                        archStore = archGrid.getStore();

                    archStore.on('beforeload', function (store, operation) {
                        operation.params.typeWorkId = typeWorkId;
                    },
                        me);
                    archStore.load();

                    skGrid = form.down('buildcontroltypeworksmrgrid'),
                        skStore = skGrid.getStore();

                    skStore.on('beforeload', function (store, operation) {
                        operation.params.typeWorkId = typeWorkId;
                    },
                        me);
                    skStore.load();
                    
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела, грид в окне
            */
            xtype: 'grideditctxwindowaspect',
            name: 'archiveMultiplyContragentSmrGridAspect',
            modelName: 'objectcr.ArchiveMultiplyContragentSmr',
            gridSelector: 'archivemultiplycontragentsmrgrid',
            editFormSelector: '#archiveMultiplyContragentSmrEditWindow',
            editWindowView: 'objectcr.ArchiveMultiplyContragentSmrEditWindow',

            listeners: {
                getdata: function (asp, record) {
                    var me = this, store;
                    if (!record.get('Id')) {
                        record.set('TypeWorkCr', typeWorkId);
                    }
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела, грид в окне
            */
            xtype: 'grideditwindowaspect',
            name: 'buildControlTypeWorkSmrGridAspect',
            modelName: 'objectcr.BuildControlTypeWorkSmr',
            gridSelector: 'buildcontroltypeworksmrgrid',
            editFormSelector: '#buildControlTypeWorkSmrEditWindow',
            editWindowView: 'objectcr.BuildControlTypeWorkSmrEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
       otherActions: function (actions) {
                var me = this;
           actions['#buildControlTypeWorkSmrEditWindow #sfTypeWorkCrAddWork'] = { 'beforeload': { fn: this.onBeforeLoadTypeWorkCrAddWork, scope: this } };
           actions[this.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
           actions['#buildControlTypeWorkSmrEditWindow' + ' #btnMap'] = { 'click': { fn: me.onClickbtnMap2, scope: me } };
            },
            onBeforeLoadContragent: function (field, options, store) {
                options.params.typeExecutant = "18";
                return true;
            },
            onClickbtnMap2: function () {
                var me = this;
                me.controller.application.redirectTo(Ext.String.format('dotmap/{0}', skId));
            },
        onBeforeLoadTypeWorkCrAddWork: function (store, operation) {
                 operation = operation || {};
                 operation.params = operation.params || {};
                 operation.params.required = true;
                 operation.params.typeWorkId = typeWorkId;
            },
            listeners: {
                getdata: function (asp, record) {
                    var me = this, store;
                    if (!record.get('Id')) {
                        record.set('TypeWorkCr', typeWorkId);
                    }
                },
                aftersetformdata: function (asp, record) {
                    var me = this,
                        grid = me.getGrid();

                    skId = record.getId();

                    var form = asp.getForm(),
                        archGrid = form.down('objectcrfileinfogrid'),
                        archStore = archGrid.getStore();

                    archStore.on('beforeload', function (store, operation) {
                        operation.params.SKId = skId;
                    },
                        me);
                    archStore.load();

                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела, грид в окне
            */
            xtype: 'grideditwindowaspect',
            name: 'buildControlTypeWorkSmrFileGridAspect',
            modelName: 'objectcr.BuildControlTypeWorkSmrFile',
            gridSelector: 'objectcrfileinfogrid',
            editFormSelector: '#objectcrFileInfoEditWindow',
            editWindowView: 'objectcr.FileInfoEditWindow',
            otherActions: function (actions) {
                actions[this.editFormSelector + ' button[action=ViewVideo]'] = { 'click': { fn: this.ViewVideo, scope: this } };
            },
            ViewVideo: function (btn) {
                var me = this,
                    panel = btn.up('#objectcrFileInfoEditWindow'),
                    record = panel.getForm().getRecord();
                var recId = record.get('VideoLink');
                new Ext.Window({
                    title: 'Проcмотр',
                    layout: { align: 'stretch' },
                    renderTo: B4.getBody().getActiveTab().getEl(), //pnlView, //view.getEl(),
                    constrain: true,
                    autoScroll: true,
                    html: recId,
                    maximizable: true
                }).show();

            },
            listeners: {
                getdata: function (asp, record) {
                    var me = this, store;
                    if (!record.get('Id')) {
                        record.set('BuildControlTypeWorkSmr', skId);
                    }
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView(),
            store;
        
        if (!view) {
            view = Ext.widget('progressexecutionworkgrid');
            store = view.getStore();

            store.on('beforeload', function (store, operation) {
                operation.params.objectCrId = id;

                operation.params.withoutSk = true;
            },
            me);
        }

        me.bindContext(view);
        me.setContextValue(view, 'objectCrId', id);
        me.application.deployView(view, 'objectcr_info');
        me.setStatePermissions();

        store.load();
      
    },
    setStatePermissions: function () {
        var me = this,
            aspect;

            aspect = me.getAspect('progressExecutionWorkObjectCrPerm');

            me.mask('Загрузка', me.getMainView());
        B4.Ajax.request(B4.Url.action('GetByObjectCrId', 'MonitoringSmr', {
            objectCrId: me.getContextValue(me.getMainView(), 'objectCrId')
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