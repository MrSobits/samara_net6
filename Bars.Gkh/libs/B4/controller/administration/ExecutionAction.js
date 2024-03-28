Ext.define('B4.controller.administration.ExecutionAction', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.view.administration.executionaction.AddWindow',
        'B4.view.administration.executionaction.Panel',
        'B4.store.administration.executionaction.ExecutionActionTask',
        'B4.view.administration.executionaction.actionwithparams.*'
    ],

    models: [
        'administration.executionaction.ExecutionActionTask',
        'administration.executionaction.ExecutionAction'
    ],
    stores: [
        'administration.executionaction.ExecutionActionTask',
        'administration.executionaction.ExecutionAction'
    ],

    views: ['administration.executionaction.Panel'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'administration.executionaction.Panel',
    mainViewSelector: 'executionactionpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'executionactionpanel'
        },
        {
            ref: 'addWindow',
            selector: 'executionactionaddwindow'
        },
        {
            ref: 'taskGrid',
            selector: 'executionactiontaskgrid'
        },
        {
            ref: 'resultGrid',
            selector: 'executionactionresultgrid'
        },
        {
            ref: 'executionActionParams',
            selector: '[name=ExecutionActionParams]'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'taskGridWindowAspect',
            gridSelector: 'executionactiontaskgrid',
            editFormSelector: 'executionactionaddwindow',
            editWindowView: 'administration.executionaction.AddWindow',
            storeName: 'administration.executionaction.ExecutionActionTask',
            modelName: 'administration.executionaction.ExecutionActionTask',
            listeners: {
                beforesetformdata: function(asp, record) {
                    var me = asp.controller,
                        selectedActionCode = asp.getForm().down('b4selectfield[name=ExecutionAction]'),
                        code = record.get('ActionCode'),
                        name = record.get('Name'),
                        description = record.get('Description'),
                        action = { Code: code, Name: name, Description: description };

                    record.set('ExecutionAction', action);

                    me.actionChange(selectedActionCode, action);
                },
                aftersetformdata: function (asp, record) {
                    var me = asp.controller,
                        form = asp.getForm(),
                        selectedActionCode = form.down('b4selectfield[name=ExecutionAction]'),
                        schedulerPanel = form.down('schedulerpanel'),
                        saveButton = form.down('b4savebutton'),
                        id = record.get('Id') || 0,
                        paramPanel = me.getExecutionActionParams(),
                        baseParams = record.get('BaseParams').Params,
                        paramsKey = {},
                        field = {};

                    if (id === 0) {
                        schedulerPanel.setValues({ StartNow: true });
                        saveButton.setText('Создать задачу');
                    } else {

                        schedulerPanel.setRecord(record, true);

                        for (paramsKey in baseParams) {
                            field = paramPanel.down('[name=' + paramsKey + ']');
                            if (field) {
                                field.setValue(baseParams[paramsKey]);
                            }
                        }

                        saveButton.setIconCls('icon-arrow-refresh');
                        saveButton.setText('Перезапустить задачу');

                        form.down('[name=ExecutionAction]').setDisabled(true);
                        form.down('[name=ExecutionActionParams]').setDisabled(true);
                        record.setDirty();
                    }

                    me.moveWindow();
                },
                getdata: function (asp, record) {
                    var me = asp.controller,
                        form = asp.getForm(),
                        selectedActionCode = form.down('b4selectfield[name=ExecutionAction]').getValue(),
                        paramsContainer = form.down('[name=ExecutionActionParams] > form'),
                        schedulerPanel = form.down('schedulerpanel'),
                        id = record.get('Id') || 0,
                        emptyBaseParams = { Files: null, Params: null };

                    if (id === 0) {
                        emptyBaseParams.Params = paramsContainer.getValues(false, false, false, true);
                        record.set('BaseParams', emptyBaseParams);
                        record.set('ActionCode', selectedActionCode);
                    }
                    Ext.apply(record, schedulerPanel.getRecord(record));
                },
                savesuccess: function(asp) {
                    var me = asp.controller;
                    me.updateTaskGrid();
                }
            },
            otherActions: function (actions) {
                var asp = this,
                    me = asp.controller;

                if (asp.editFormSelector) {
                    actions[asp.editFormSelector + ' b4savebutton'] = {
                        'click': {
                            fn: asp.customSaveRequestHandler,
                            scope: asp
                        }
                    };
                }
            },
            customSaveRequestHandler: function () {
                var asp = this;

                asp.saveRequestHandler();
            },
            customSetValues: function (record, paramName) {
                var asp = this,
                    form = asp.getForm(),
                    value = {};

                value[paramName] = record[paramName];
                form.down('[name=' + paramName + ']').setValue(value);
            },
            deleteRecord: function (record) {
                var asp = this,
                    me = asp.controller;

                Ext.Msg.confirm('Удаление задачи', 'Выполняемая задача будет прервана. Вы действительно хотите удалить задачу?', function (result) {
                    if (result == 'yes') {
                        var model = asp.getModel(record);

                        var rec = new model({ Id: record.getId() });
                        asp.mask('Удаление', B4.getBody());
                        rec.destroy()
                            .next(function () {
                                asp.unmask();
                                asp.fireEvent('deletesuccess', asp);
                                me.updateTaskGrid();
                            }, asp)
                            .error(function (result) {
                                asp.unmask();
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            }, asp);
                    }
                }, asp);
            },
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'executionactionaddwindow b4selectfield': {
                    change: me.actionChange,
                    scope: me
                },
                'executionactionresultgrid': {
                    'rowaction': {
                        fn: me.onRowAction,
                        scope: me
                    }
                },
                'executionactiontaskgrid button[name=QueueInfo]': {
                    click: me.getQueueInfo,
                    scope: me
                },
            };
        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('executionactionpanel');

        me.bindContext(view);
        me.application.deployView(view);

        me.updateTaskGrid();
        me.updateResultGrid();
    },

    updateTaskGrid: function() {
        this.getTaskGrid().getStore().load();
    },

    updateResultGrid: function () {
        this.getResultGrid().getStore().load();
    },

    moveWindow: function () {
        var me = this,
            window = me.getAddWindow() || Ext.widget('executionactionaddwindow'),
            xPos = window.getPosition()[0];

        window.setPosition(xPos, 15);
    },

    getQueueInfo: function () {
        var me = this;

        B4.Ajax.request({
            url: B4.Url.action('GetQueueInfo', 'ExecutionAction'),
            method: 'POST',
        }).next(function (response) {
            var responseText = response.responseText || '',
                result = {},
                newWin = {};
            me.unmask();

            result = JSON.stringify(Ext.decode(responseText).data, null, '\t');
            newWin = window.open('about:blank', 'DataResult');

            newWin.document.write('<pre>' + result + '</pre>');
        }).error(function (response) {
            me.unmask();
        });
    },

    actionChange: function (selectField, action) {
        var me = this,
            textArea = selectField.up().down('textarea');

        me.hideParamFields();
        if (action) {
            textArea.setValue(action.Description);
            me.showParamFields(action.Code);
        } else {
            textArea.setValue('');
        }
    },

    hideParamFields: function (panel, code) {
        var me = this,
            paramPanel = me.getExecutionActionParams(),
            form = paramPanel.down('form');

        form.removeAll();
        paramPanel.hide();
    },

    showParamFields: function (code) {
        var me = this,
            paramPanel = me.getExecutionActionParams(),
            form = paramPanel.down('form');
        if (code) {
            try {
                paramPanel.show();
                var params = Ext.create('B4.view.administration.executionaction.actionwithparams.' + code);
                form.add(params);
            } catch (e) {
                me.hideParamFields();
            }
        }
    },

    onRowAction: function (grid, action, rec) {
        switch(action) {
            case 'gotoresult':
                this.gotoResult(rec);
                break;
        }
    },

    gotoResult: function(rec) {
        var dataResult = rec.get('Result'),
            result,
            newWin,
            data = { success: null, data: null, message: null, Error: null, StackTrace: null};

        if (dataResult && dataResult.hasOwnProperty('data')) {
            if ('string' === typeof dataResult.data) {
                data.data = JSON.parse(dataResult.data);
            } else {
                data.data = dataResult.data;
            }
            data.success = dataResult.success;
            data.message = dataResult.message;
            data.StackTrace = dataResult.StackTrace;
            data.Error = dataResult.Error;
        }

        result = JSON.stringify
        (
            data, 
            function(key, value) {if (value !== null) return value},
            '\t'
        );
        newWin = window.open('about:blank', 'DataResult');

        newWin.document.write('<pre>' + result.split('\\r\\n').join('\r\n\t\t') + '</pre>');
    },
});