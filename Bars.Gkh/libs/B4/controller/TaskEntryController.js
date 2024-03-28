Ext.define('B4.controller.TaskEntryController', {
    extend: 'B4.base.Controller',

    requires: ['B4.QuickMsg', 'B4.aspects.permission.GkhPermissionAspect'],

    stores: ['TaskEntryStore'],
    views: ['TaskEntryGrid'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'taskentrygrid'
        },
        {
            ref: 'abortButton',
            selector: 'taskentrygrid button[action="AbortTask"]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            name: 'detailsPerm',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            },
            permissions: [
                {
                    name: 'Tasks.Delete_View',
                    applyTo: '[name=DeleteTask]',
                    selector: 'taskentrygrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Tasks.ClearRabbitMQ_View',
                    applyTo: '[name=ClearQueueRabbitMQ]',
                    selector: 'taskentrygrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'taskentrygrid button[action="AbortTask"]': {
                click: me.onAbortTask
            },
            'taskentrygrid button[action="Restart"]': {
                click: me.onRestart
            },
            'taskentrygrid button[action="ClearQueueRabbitMQ"]': {
                click: me.onClearQueueRabbitMQ
            },
            'taskentrygrid button[action="DeleteTask"]': {
                click: me.onDeleteTask
            },
            'taskentrygrid': {
                selectionchange: me.selectionChanged,
                rowaction: me.onRowAction
            }
        });

        me.callParent(arguments);
    },

    index: function (params) {
        var me = this,
            view = me.getMainView() || Ext.widget('taskentrygrid'),
            polling,
            task,
            taskId;

        me.bindContext(view);
        me.application.deployView(view);

        if (params && params.ParentId && (taskId = parseInt(params.ParentId))) {
            var headerFilterPlugin = view.getPlugin('headerFilter');

            if (headerFilterPlugin) {
                var parentField = headerFilterPlugin.fields.get('ParentId');
                parentField.setValue(taskId);
            }
        }

        view.getStore().load();

        view.on('close', function () {
            if (task && task.destroy) {
                task.destroy();
            }
        });
    },

    onRestart: function () {
        B4.Ajax.request({
            url: B4.Url.action('Restart', 'Tasks')
        });
    },

    onAbortTask: function () {
        var me = this,
            grid = me.getMainView(),
            selected = grid.getSelectionModel().getSelection(),
            rec;

        if (!selected || selected.length === 0) {
            return false;
        }

        rec = selected[0];

        me.mask('Отмена задачи...');
        B4.Ajax.request({
            url: B4.Url.action('AbortTask', 'TaskEntry'),
            params: {
                taskId: rec.get('Id')
            },
            timeout: 999999
        }).next(function (resp) {
            var decoded = Ext.JSON.decode(resp.responseText) || resp;

            Ext.Msg.alert('Внимание', decoded && decoded.message ? decoded.message : 'Задача в процессе завершения');
            me.unmask();
            grid.getStore().load();
        }).error(function (resp) {
            var decoded = Ext.JSON.decode(resp.responseText) || resp;

            Ext.Msg.alert('Ошибка', decoded && decoded.message ? decoded.message : 'Ошибка при отмене задачи');
            me.unmask();
        });
    },

    onClearQueueRabbitMQ: function () {
        var me = this;
        me.mask('Очистка очереди RabbitMQ...');
        B4.Ajax.request({
            url: B4.Url.action('ClearQueueRabbitMQ', 'GkhTasks')

        }).next(function (response) {
            var obj = Ext.JSON.decode(response.responseText) || response;

                me.unmask();
                B4.QuickMsg.msg('Успешно', 'Очистка очереди RabbitMQ', 'success');

        }).error(function (e) {
            B4.QuickMsg.msg('Ошибка', 'При очистке очереди RabbitMQ возникла ошибка', 'error');
            me.unmask();
        });
    },

    onDeleteTask: function() {
        var me = this,
            grid = me.getMainView(),
            selected = grid.getSelectionModel().getSelection(),
            rec;

        if (!selected || selected.length === 0) {
            return false;
        }

        rec = selected[0];

        me.mask('Удаление задачи...');
        B4.Ajax.request({
            url: B4.Url.action('DeleteTask', 'GkhTasks'),
            params: {
                ParentId: rec.get('ParentId')
            },
            timeout: 999999
        }).next(function (response) {
            var obj = Ext.JSON.decode(response.responseText) || response;

            me.unmask();
            B4.QuickMsg.msg('Успешно', 'Удаление задачи', 'success');
            grid.getStore().load();

        }).error(function (e) {
            B4.QuickMsg.msg('Ошибка', 'При удалении задачи возникла ошибка', 'error');
            me.unmask();
        });
    },

    onRowAction: function (grid, action, rec) {
        var me = this;
        if (action.toLowerCase() === 'gotoresult') {
            me.mask();
            B4.Ajax.request({
                url: B4.Url.action('GetDataForUI', 'Tasks'),
                params: { id: rec.get('Id') }
            }).next(function (resp) {
                me.unmask();
                var result = Ext.JSON.decode(resp.responseText),
                    data = result.data;
                if (data && data.url) {
                    window.open(data.url, '_blank');
                    window.focus();
                }
            }).error(function (resp) {
                me.unmask();
            });
        }
    },

    selectionChanged: function () {
        var me = this,
            button = me.getAbortButton();

        if (button) {
            button.enable();
        }
    }
});