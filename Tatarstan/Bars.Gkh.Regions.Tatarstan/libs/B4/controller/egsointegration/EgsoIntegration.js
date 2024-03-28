Ext.define('B4.controller.egsointegration.EgsoIntegration', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.InlineGrid',
        'B4.enums.EgsoTaskType',
        'B4.enums.EgsoTaskStateType'
    ],

    views: [
        'egsointegration.EgsoIntegrationGrid',
        'egsointegration.EgsoIntegrationWindow'
    ],

    models: ['egso.EgsoIntegration', 'egso.Municipality'],

    stores: ['egso.EgsoIntegration', 'egso.Municipality'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'egsointegration.EgsoIntegrationGrid',
    mainViewSelector: 'egsointegrationgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'egsointegrationgrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'egsoIntegrationAspect',
            gridSelector: 'egsointegrationgrid',
            editFormSelector: 'egsointegrationwindow',
            storeName: 'egso.EgsoIntegration',
            modelName: 'egso.EgsoIntegration',
            editWindowView: 'egsointegration.EgsoIntegrationWindow',
            listeners: {
                beforesetformdata: function(asp, rec, form) {
                    var taskType = form.down('b4enumcombo[name=TaskType]'),
                        year = form.down('b4combobox[name=Year]'),
                        municipalitiesGrid = form.down('grid[name=MunicipalitiesGrid]'),
                        cellEditPlugin = municipalitiesGrid.getPlugin('cellEditing'),
                        btnExecute = form.down('button[name=Execute]'),
                        egsoTaskId = rec.getId(),
                        egsoState = rec.getData().StateType;

                    asp.controller.setContextValue(asp.getForm(), 'isLoaded', false);

                    cellEditPlugin.on('beforeedit', function() {
                        if (egsoTaskId)
                            return false;
                    }, asp);

                    if (egsoTaskId && egsoState != B4.enums.EgsoTaskStateType.Undefined) {
                        taskType.setDisabled(true);
                        year.setDisabled(true);
                        btnExecute.setDisabled(true);
                    }

                },
                aftersetformdata: function(asp, rec, form) {
                    var taskType = form.down('b4enumcombo[name=TaskType]'),
                        year = form.down('b4combobox[name=Year]'),
                        municipalitiesGrid = form.down('grid[name=MunicipalitiesGrid]'),
                        store = municipalitiesGrid.getStore();

                    if (!taskType.getValue()) {
                        taskType.setValue(B4.enums.EgsoTaskType.ManyApartmentsCount);
                    }

                    if (!year.getValue()) {
                        year.setValue(new Date().getFullYear());
                    }
                    store.load();

                    asp.controller.setContextValue(asp.getForm(), 'isLoaded', true);
                }
            },
            otherActions: function(actions) {
                actions[this.editFormSelector + ' button[name=Execute]'] =
                    { 'click': { fn: this.onClickExecute, scope: this } };
                actions[this.editFormSelector + ' b4combobox[name=Year]'] =
                    { 'change': { fn: this.reloadGrid, scope: this } };
                actions[this.editFormSelector + ' b4enumcombo[name=TaskType]'] =
                    { 'change': { fn: this.reloadGrid, scope: this } };
                actions[this.editFormSelector + '[title=Задача]'] =
                    { 'store.beforeload': { fn: this.onStoreBeforeLoad, scope: this } };
            },
            reloadGrid: function(cmp) {
                var asp = this,
                    store = asp.getForm().down('grid[name=MunicipalitiesGrid]').getStore(),
                    isLoaded = asp.controller.getContextValue(asp.getForm(), 'isLoaded');

                if (isLoaded) {
                    store.load();
                }
            },
            onStoreBeforeLoad: function(store, operation) {
                var form = this.getForm(),
                    taskType = form.down('b4enumcombo[name=TaskType]').getValue(),
                    year = form.down('b4combobox[name=Year]').getValue(),
                    egsoTaskId = form.getRecord().getId();

                Ext.apply(operation.params, {
                    egsoTaskId: egsoTaskId,
                    taskType: taskType,
                    year: year
                });
            },
            onClickExecute: function(btn) {
                var me = this,
                    view = me.controller.getMainView(),
                    win = btn.up('egsointegrationwindow'),
                    taskType = win.down('b4enumcombo[name=TaskType]'),
                    year = win.down('b4combobox[name=Year]'),
                    municipalitiesGrid = win.down('grid[name=MunicipalitiesGrid]'),
                    egsoTaskId = win.getRecord().getId(),
                    records = [];

                municipalitiesGrid.getStore()
                    .each(function(rec) {
                        records.push(rec.data);
                    });

                me.mask('Сохранение', win);
                B4.Ajax.request({
                        url: B4.Url.action('CreateTask', 'EgsoIntegration'),
                        method: 'POST',
                        params: {
                            year: year.getValue(),
                            taskType: taskType.getValue(),
                            municipalities: Ext.encode(records),
                            egsoTaskId: egsoTaskId
                        },
                        timeout: 60 * 60 * 1000 // 1 час
                    })
                    .next(function() {
                        B4.QuickMsg.msg('Успех', 'Задача успешно обработана', 'success');
                        me.unmask();
                        win.close();
                        view.getStore().load();
                    })
                    .error(function(response) {
                        var msg = response.message || 'При обработке задачи произошла ошибка';
                        B4.QuickMsg.msg('Ошибка', msg, 'failure');
                        me.unmask();
                        win.close();
                        view.getStore().load();
                    });
            },
            updateGrid: function() {
                this.getGrid().getStore().load();
            }
        },
        {
            xtype: 'inlinegridaspect',
            name: 'egsoIntegrationValuesAspect',
            storeName: 'egso.Municipality',
            modelName: 'egso.Municipality',
            gridSelector: 'egsointegrationwindow'
        }
    ],

    index: function() {
        var view = this.getMainView() || Ext.widget('egsointegrationgrid');

        view.getStore().load();

        this.bindContext(view);
        this.application.deployView(view);
    }
});