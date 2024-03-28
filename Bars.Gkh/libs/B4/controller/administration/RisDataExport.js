Ext.define('B4.controller.administration.RisDataExport', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.SchedulerEditWindow',
        'B4.enums.FormatDataExportStatus',
        'B4.enums.FormatDataExportObjectType',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [
        'administration.risdataexport.FormatDataExportTask',
    ],
    stores: [
        'administration.risdataexport.FormatDataExportTask',
    ],

    views: [
        'administration.risdataexport.Panel',
        'administration.risdataexport.AddWindow',
        'administration.risdataexport.Wizard',
        'administration.risdataexport.DpkrPanel',
        'administration.risdataexport.DpkrWorksEditWindow',
        'administration.risdataexport.DpkrEditWindow',
        'administration.risdataexport.DpkrGrid',
        'administration.risdataexport.DpkrDocumentGrid',
        'administration.risdataexport.DpkrWorksGrid',
        'administration.risdataexport.DpkrWorksPanel',
    ],

    aspects: [
        {
            xtype: 'schedulereditwindowaspect',
            name: 'exportTaskGridWindowAspect',
            gridSelector: 'risdataexporttaskgrid',
            editFormSelector: 'risdataexportwizard',
            editWindowView: 'administration.risdataexport.Wizard',
            modelName: 'administration.risdataexport.FormatDataExportTask',
            listeners: {
                aftersetformdata: function(asp, record) {
                    var form = asp.getForm(),
                        propertyGrid = form.down('propertygrid'),
                        id = record.get('Id') || 0;

                    Ext.each(form.query('[type=create]'), function(i) {
                        i.setDisabled((id !== 0));
                    });
                    Ext.each(form.query('[type=edit]'), function(i) {
                        i.setDisabled((id === 0));
                    });

                    if (id === 0) {
                        form.getLayout().next();
                    } else {
                        asp.controller.changeStateNavigateButtons();
                        propertyGrid.setSource(record.get('DisplayParams'))
                    }
                },
                getdata: function(asp, record) {
                    var form = asp.getForm(),
                        id = record.getId() || 0,
                        inspectionGrid = form.down('grid[name=InspectionList]'),
                        persAccGrid = form.down('grid[name=PersAccList]'),
                        longTermGrid = form.down('grid[name=LongTermList]'),
                        shortTermGrid = form.down('grid[name=ShortTermList]'),
                        duUstavGrid = form.down('grid[name=DuUstavList]'),
                        realityObjectGrid = form.down('grid[name=RealityObjectList]'),

                        baseParams = {};

                    if (id > 0) {
                        record.set('StartNow', null);
                        record.set('StartNow', record.get('PeriodType') === 0);
                        return;
                    }

                    asp.setBaseParams(baseParams, record, 'MainContragent');
                    asp.setBaseParams(baseParams, record, 'MunicipalityList');
                    asp.setBaseParams(baseParams, record, 'ContragentList');
                    asp.setBaseParams(baseParams, record, 'UseIncremental');
                    asp.setBaseParams(baseParams, record, 'StartEditDate');
                    asp.setBaseParams(baseParams, record, 'EndEditDate');
                    asp.setBaseParams(baseParams, record, 'MaxFileSize');
                    asp.setBaseParams(baseParams, record, 'IsSeparateArch');
                    asp.setBaseParams(baseParams, record, 'NoEmptyMandatoryFields');
                    asp.setBaseParams(baseParams, record, 'OnlyExistsFiles');
                    asp.setBaseParams(baseParams, record, 'AuditType');
                    asp.setBaseParams(baseParams, record, 'DisposalStartDate');
                    asp.setBaseParams(baseParams, record, 'DisposalEndDate');
                    asp.setBaseParams(baseParams, record, 'ChargePeriod');
                    asp.setBaseParams(baseParams, record, 'ProgramVersionMunicipalityList');
                    asp.setBaseParams(baseParams, record, 'ProgramCrList');
                    asp.setBaseParams(baseParams, record, 'ObjectCrMunicipalityList');
                    asp.setBaseParams(baseParams, record, 'WithoutAttachment');

                    Ext.apply(baseParams, asp.getGridFilter(inspectionGrid, 'Inspection'));
                    Ext.apply(baseParams, asp.getGridFilter(persAccGrid, 'PersAcc'));
                    Ext.apply(baseParams, asp.getGridFilter(longTermGrid, 'ProgramVersion'));
                    Ext.apply(baseParams, asp.getGridFilter(shortTermGrid, 'ObjectCr'));
                    Ext.apply(baseParams, asp.getGridFilter(duUstavGrid, 'DuUstav'));
                    Ext.apply(baseParams, asp.getGridFilter(realityObjectGrid, 'RealityObject'));

                    record.set('BaseParams', { Files: null, Params: baseParams });
                }
            },
            updateStartButton: function (button) {
                var me = this.controller;
                button.setText(me.isStartInExecutor() ? 'Создать задачу' : 'Запустить задачу');
            },
            otherActions: function(actions) {
                var asp = this;

                if (asp.editFormSelector) {
                    actions[asp.editFormSelector + ' grid[name=EntityGroup]'] = {
                        'selectionchange': {
                            fn: asp.onSectionSelect,
                            scope: asp
                        }
                    };
                }
            },
            onSectionSelect: function(grid, selected) {
                var asp = this,
                    rec = asp.getForm().getRecord(),
                    wizard = asp.getForm(),
                    pages = wizard.query('panel[isWizardPage]') || [],
                    selectedSection = Ext.Array.map(selected, function(s) {
                        return s.getId()
                    });

                Ext.each(pages, function(p) {
                    p.initWizardPage(selectedSection);
                });

                rec.set('EntityGroupCodeList', selectedSection);
            },
            setBaseParams: function(baseParams, record, paramName) {
                var value = record.get(paramName);
                if (Ext.isDefined(value) && value !== null) {
                    baseParams[paramName] = value;
                }
            },
            getComplexFilter: function(store) {
                var filter = {};
                if (store && store.lastOptions && store.lastOptions.params) {
                    filter = store.lastOptions.params.complexFilter;
                    return Ext.isString(filter) ? Ext.decode(filter) : filter;
                }

                return filter;
            },
            getDataFilter: function (store) {
                var filter = {};
                if (store && store.lastOptions && store.lastOptions.params) {
                    filter = store.lastOptions.params.dataFilter;
                    return Ext.isString(filter) ? Ext.decode(filter) : filter;
                }

                return filter;
            },
            getGridFilter: function(grid, filterName, idName) {
                var asp = this,
                    id = Ext.isEmpty(idName) ? 'Id' : idName,
                    store = grid.getStore(),
                    selection = (grid && grid.getSelectionModel().getSelection()) || [],
                    ids = Ext.Array.map(selection, function(s) {
                        return s.get(id);
                    }),
                    complexFilter = {},
                    dataFilter = {},
                    filter = {},
                    result = {};
                if (grid) {
                    complexFilter = asp.getComplexFilter(store);
                    if (!Ext.isEmpty(complexFilter)) {
                        filter['complexFilter'] = complexFilter;
                    }

                    dataFilter = asp.getDataFilter(store);
                    if (!Ext.isEmpty(dataFilter)) {
                        filter['dataFilter'] = dataFilter;
                    }
                    result[filterName + 'Filter'] = filter;
                    result[filterName + 'List'] = ids;
                }

                return result;
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'risdataexportdpkreditwindow',
            modelName: 'administration.risdataexport.FormatDataExportInfo',
            gridSelector: 'risdataexportinfogrid',
            listeners: {
                beforerowaction: function(asp, grid, action, record) {
                    if (record.data.ObjectType == B4.enums.FormatDataExportObjectType.CrProgram) {
                        asp.editFormSelector = 'risdataexportdpkreditwindow',
                            asp.editWindowView = 'administration.risdataexport.DpkrEditWindow'
                            
                    } 
                    if (record.data.ObjectType == B4.enums.FormatDataExportObjectType.CrProgramWorks) {
                        asp.editFormSelector = 'risdataexportdpkrworkseditwindow',
                            asp.editWindowView = 'administration.risdataexport.DpkrWorksEditWindow'
                    }
                    asp.controller.setContextValue(grid, 'Id', record.get('Id'));
                    return true;
                },
                aftersetformdata: function (asp, rec, form) {
                    if (asp.editFormSelector == 'risdataexportdpkreditwindow') {
                        form.down('grid[name=DpkrGrid]').getStore().load();
                        form.down('grid[name=DpkrDocumentGrid]').getStore().load();
                    }
                    if (asp.editFormSelector == 'risdataexportdpkrworkseditwindow') {
                        form.down('grid[name=DpkrWorksGrid]').getStore().load();
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.tab.show();
                } else {
                    component.tab.hide();
                }
            },
            permissions: [
                {
                    name: 'Administration.ImportExport.RisDataExportInfo',
                    applyTo: 'risdataexportinfogrid',
                    selector: 'risdataexportpanel',
                }
            ]
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'administration.risdataexport.Panel',
    mainViewSelector: 'risdataexportpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'risdataexportpanel'
        },
        {
            ref: 'exportResultGrid',
            selector: 'risdataexportresultgrid'
        },
        {
            ref: 'exportTaskGrid',
            selector: 'risdataexporttaskgrid'
        },
        {
            ref: 'exportInfoGrid',
            selector: 'risdataexportinfogrid'
        },
        {
            ref: 'exportTaskWindow',
            selector: 'risdataexportaddwindow'
        },
        {
            ref: 'wizard',
            selector: 'risdataexportwizard'
        }
    ],

    init: function() {
        var me = this,
            actions = {
                'risdataexportresultgrid': { rowaction: { fn: me.onResultGridRowAction, scope: me } },
                'risdataexportwizard b4updatebutton': { click: { fn: me.onUpdateButtonClick, scope: me } },
                'risdataexportwizard button[type=navigation]': { click: { fn: me.onNavigateClick, scope: me } },
                'risdataexportwizard checkbox[name=UseIncremental]': {
                    change: { fn: me.onUseIncrementalChange, scope: me }
                },
                'risdataexportdpkrworksgrid b4updatebutton': { click: { fn: me.onUpdateButtonClick, scope: me } },
                'risdataexportdpkrgrid b4updatebutton': { click: { fn: me.onUpdateButtonClick, scope: me } },
                'risdataexportdpkrdocumentgrid b4updatebutton': { click: { fn: me.onUpdateButtonClick, scope: me } },
                'risdataexportdpkrworksgrid': { 'store.beforeload': { fn: me.onBeforeLoad, scope: me } },
                'risdataexportdpkrgrid': { 'store.beforeload': { fn: me.onBeforeLoad, scope: me } },
                'risdataexportdpkrdocumentgrid': { 'store.beforeload': { fn: me.onBeforeLoad, scope: me } }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('risdataexportpanel');

        me.bindContext(view);
        me.application.deployView(view);

        me.getExportTaskGrid().getStore().load();
        me.getExportResultGrid().getStore().load();
        me.getExportInfoGrid().getStore().load();
    },

    onNavigateClick: function(button) {
        var me = this,
            layout = me.getWizard().getLayout(),
            validateResult = layout.getActiveItem().isValid();

        if (validateResult === true || button.action === 'prev') {
            layout[button.action]();

            me.changeStateNavigateButtons();
        } else {
            Ext.Msg.alert('Ошибка валидации', validateResult);
        }
    },

    changeStateNavigateButtons: function() {
        var me = this,
            panel = me.getWizard(),
            layout = panel.getLayout(),
            wizardPage = layout.getActiveItem(),
            prevButton = panel.down('[action=prev]'),
            nextButton = panel.down('[action=next]'),
            saveButton = panel.down('[action=save]'),
            canNext = layout.getNext() !== false;

        if (wizardPage && wizardPage.isWizardPage) {
            wizardPage.loadWizardPage();
        }

        prevButton.setDisabled(!layout.getPrev());
        saveButton.setVisible(!canNext);
        nextButton.setVisible(canNext);
    },

    onResultGridRowAction: function(grid, action, rec) {
        var me = this,
            logFile = rec.get('LogFile') || {},
            taskId = rec.get('TaskId') || 0,
            resultId = rec.get('Id'),
            status = rec.get('Status'),
            msgTitle = 'Получение файла лога';

        switch (action) {
            case 'getlog':
                switch (status) {
                    case B4.enums.FormatDataExportStatus.Pending:
                    case B4.enums.FormatDataExportStatus.Running:
                        B4.QuickMsg.msg(msgTitle,
                            'Задача не завершена. Лог недоступен', 'warning');
                        return;
                }

                if (logFile.Id > 0) {
                    window.open(B4.Url.action('Download', 'FileUpload', { id: logFile.Id }));
                } else {
                    B4.QuickMsg.msg(msgTitle, 'При сохранении лога произошла ошибка. Файл недоступен', 'error');
                }
                return;

            case 'updatestatus':
                me.mask('Обновление статуса задачи...', grid.ownerCt);
                B4.Ajax.request({
                    url: B4.Url.action('UpdateRemoteStatus', 'FormatDataExport'),
                    params: {
                        id: taskId,
                        resultId: resultId
                    },
                    timeout: 1000 * 60 * 60
                }).next(function (response) {
                    me.unmask();
                    grid.getStore().load();
                    B4.QuickMsg.msg('Обновление статуса', 'Статус успешно обновлен', 'success');
                }).error(function (response) {
                    me.unmask();
                    B4.QuickMsg.msg('Обновление статуса', 'Не удалось обновить статус', 'error');
                });
                break;
            case 'getuploadresult':
                if (taskId == 0) {
                    B4.QuickMsg.msg('Получение статуса задачи', 'Отсутствует идентификатор задачи', 'error');
                    break;
                }
                me.mask('Получение статуса задачи...', grid.ownerCt);
                B4.Ajax.request({
                    url: B4.Url.action('GetRemoteStatus', 'FormatDataExport'),
                    params: {
                        id: taskId
                    },
                    timeout: 1000 * 60 * 60
                }).next(function (response) {
                    me.unmask();
                    var dataResult = response.responseText,
                        result = '',
                        newWin = null,
                        data = {};

                    if (dataResult) {
                        if ('string' === typeof dataResult) {
                            data = JSON.parse(dataResult);
                        } else {
                            data = dataResult;
                        }
                    }

                    if (data && data.LogId > 0) {
                        window.open(B4.Url.action('GetRemoteFile', 'FormatDataExport', { id: data.LogId }));
                    } else {
                        B4.QuickMsg.msg('Получение файла лога', 'При сохранении лога произошла ошибка. Файл недоступен', 'error');
                    }

                }).error(function (response) {
                    me.unmask();
                    var error = response.message || response.responseText || response.statusText;

                    Ext.Msg.alert('Ошибка', error);
                });
                break;
        }
    },

    onUpdateButtonClick: function(button) {
        button.up('b4grid').getStore().load();
    },

    onAddButtonClick: function(button) {
        var window = Ext.widget('risdataexportaddwindow');

        window.show();
    },

    onUseIncrementalChange: function(component, newValue) {
        var parentContainer = component.up('container'),
            timeIntervalField = parentContainer.down('container[name=EditDateInterval]');

        Ext.each(timeIntervalField.query(), function(component) {
            component.setDisabled(!newValue)
        });
    },

    isStartInExecutor: function() {
        try {
            return Gkh.config.Administration.FormatDataExport.FormatDataExportGeneral.StartInExecutor;
        } catch (e) {
            return false;
        }
    },
    onBeforeLoad: function (store, operation) {
        var me = this,
            view = me.getMainView();

        operation.params.Id = me.getContextValue(view, 'Id');
    }
});