Ext.define('B4.controller.al.StoredReport', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.mixins.Context',
        'B4.form.ComboBox',
        'B4.enums.al.ParamType',
        'B4.enums.al.OwnerType',
        'B4.enums.al.ReportPrintFormat',
        'B4.aspects.Permission',
        'B4.view.al.ExternalLinkWindow',
        'B4.store.dict.RoleForSelect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody',
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    views: [
        'al.StoredReportGrid',
        'al.StoredReportEdit',
        'al.DataSourceAddWindow',
        'al.ParameterWindow'
    ],

    models: [
        'al.StoredReport',
        'al.DataSource',
        'al.ReportParam',
        'al.Role'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'storedreportgrid'
        },
        {
            ref: 'storedReportWindow',
            selector: 'storedreportedit'
        },
        {
            ref: 'dataWindow',
            selector: 'datasourceaddwindow'
        },
        {
            ref: 'paramWindow',
            selector: 'parameterwindow'
        },
        {
            ref: 'dataSourceGrid',
            selector: 'grid[name=dataStore]'
        },
        {
            ref: 'rolesGrid',
            selector: 'grid[name=rolesGrid]'
        },
        {
            ref: 'paramsGrid',
            selector: 'grid[name=paramGrid]'
        },
        {
            ref: 'dataSourcesIdsField',
            selector: 'storedreportedit hidden[name=DataSourcesIds]'
        },
        {
            ref: 'deletedParamsIdsField',
            selector: 'storedreportedit hidden[name=DeletedParamsIds]'
        },
        {
            ref: 'reportParamsField',
            selector: 'storedreportedit hidden[name=ReportParams]'
        },
        {
            ref: 'reportId',
            selector: 'storedreportedit hidden[name=Id]'
        },
        {
            ref: 'addConnCheckbox',
            selector: 'storedreportedit checkbox[name=addConn]'
        }
    ],

    aspects: [
        {
            xtype: 'permissionaspect',
            applyBy: function (cmp, allowed) {
                cmp.setVisible(allowed);
            },
            applyOn: {
                event: 'show',
                selector: 'storedreportedit'
            },
            permissions: [
                {
                    name: 'B4.Analytics.Reports.StoredReports.Connection',
                    applyTo: 'storedreportedit checkbox[name=addConn]'
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            gridSelector: 'storedreportgrid',
            editFormSelector: 'storedreportedit',
            modelName: 'al.StoredReport',
            editWindowView: 'al.StoredReportEdit',
            listeners: {
                beforesave: function (asp, record) {
                    var dataSourcesIds = Ext.Array.map(asp.controller.getDataSourceGrid().getStore().getRange(),
                            function (r) {
                                return r.get('Id');
                            }),
                        paramsGrid = asp.controller.getParamsGrid(),
                        paramsStore = paramsGrid.getStore(),
                        reportParams = Ext.JSON.encode(Ext.Array.map(Ext.Array.filter(paramsStore.getRange(), function (r) {
                            return r.raw.OwnerType !== B4.enums.al.OwnerType.System;
                        }), function (r) {
                            return r.raw;
                        })),
                        form = asp.getForm(),
                        rolesGrid = form.down('[name=rolesGrid]'),
                        selectedRoles = rolesGrid.selModel.getSelection(),
                        checkAll = form.down('[name=ForAll]');

                    if (!checkAll.getValue() && selectedRoles.length === 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо указать роли, для которых будет доступен отчет');
                        return false;
                    }

                    paramsGrid.deletedRecordsIds = paramsGrid.deletedRecordsIds || [];
                    record.set('DataSourcesIds', dataSourcesIds);
                    asp.controller.getDataSourcesIdsField().setValue(dataSourcesIds.join());
                    asp.controller.getDeletedParamsIdsField().setValue(paramsGrid.deletedRecordsIds.join());
                    asp.controller.getReportParamsField().setValue(reportParams);
                    return record;
                },
                aftersetformdata: function (asp, rec) {
                    asp.controller.loadDataSourcesStore(asp, rec);
                    if (!asp.getForm().alreadySaved) {
                        asp.getForm().alreadySaved = true;
                        asp.controller.loadRolePermissions(asp, rec);
                    }
                    var paramStore = asp.controller.getParamsGrid().getStore();
                    if (!rec.get('Id')) {
                        asp.controller.getStoredReportWindow().down('b4enumcombo[name=ReportType]').setValue(2);
                    } else {
                        asp.controller.getStoredReportWindow().down('b4filefield[name=Template]').setValue({
                            id: rec.get('Id'),
                            name: 'Файл шаблона'
                        });
                        paramStore.on('beforeload', function (store, operation) {
                            operation.params = operation.params || {};
                            operation.params.reportId = rec.get('Id');
                        });
                        paramStore.load();
                    }
                },
                savesuccess: function (asp, rec) {
                    var form = asp.getForm(),
                        paramsGrid = asp.controller.getParamsGrid(),
                        storedReportGrid = asp.controller.getMainView(),
                        rolesGrid = form.down('[name=rolesGrid]'),
                        records = rolesGrid.selModel.getSelection(),
                        result = [];

                    if (paramsGrid) {
                        paramsGrid.deletedRecordsIds = [];
                    }
                    
                    if (records.length > 0) {
                        Ext.each(records, function (record) {
                            result.push(record.data.Id);
                        });
                    }

                    asp.controller.mask('Сохранение', form);
                    B4.Ajax.request({
                        url: B4.Url.action('SavePermissions', 'StoredReport'),
                        params: {
                            reportId: rec.getId(),
                            roleIds: Ext.encode(result)
                        },
                        timeout: 200000
                    }).next(function () {
                        asp.controller.unmask(form);
                        storedReportGrid.getStore().load();
                    }).error(function (response) {
                        asp.controller.unmask(form);
                        Ext.Msg.alert('Ошибка!', !Ext.isString(response.message) ? 'Не удалось сохранить права доступа' : response.message);
                    });
                }
            },
            onSaveSuccess: function (asp) {
                var w = asp.controller.getStoredReportWindow(),
                    notif = w.down('#needSaveNotification'),
                    panel = w.down('#magicPanel');

                notif.hide();
                panel.enable();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            gridSelector: 'storedreportedit grid[name=paramGrid]',
            modelName: 'al.ReportParam',
            editFormSelector: 'parameterwindow',
            editWindowView: 'al.ParameterWindow',
            saveRequestHandler: function () { },
            deleteRecord: function () { },
            editRecord: function (record) {
                var me = this,
                    model;

                model = me.getModel(record);
                me.setFormData(record || new model({ Id: 0 }));

                me.getForm().getForm().isValid();
            },
            listeners: {
                'beforerowaction': function (asp, grid, action, record) {
                    if (action === 'delete' || action === 'edit') {
                        if (record.get('OwnerType') === B4.enums.al.OwnerType.System) {
                            Ext.MessageBox.alert('Ошибка', 'Системные параметры нельзя изменять.');
                            return false;
                        }
                    }

                    if (action === 'delete') {
                        if (record.get('Id')) {
                            grid.deletedRecordsIds = grid.deletedRecordsIds || [];
                            grid.deletedRecordsIds.push(record.get('Id'));
                            grid.getStore().remove(record);
                        }
                        return false;
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'button[action=DownloadEmptyTemplate]': {
                'click': {
                    fn: me.onMagicBtnClick,
                    scope: me
                }
            },
            'button[action=OpenStimulDesigner]': {
                'click': {
                    fn: me.onMagicBtnClick,
                    scope: me
                }
            },
            '[name=templateCard]': {
                'show': {
                    fn: me.onTemplateCardShow,
                    scope: me
                }
            },
            'button[action=GetExternalLink]': {
                'click': {
                    fn: me.onGetExternalLinkBtnClick,
                    scope: me
                }
            },
            'b4selectfield[name=DataSourcesIds]': {
                'change': {
                    fn: me.onDataSourcesSelected,
                    scope: me
                }
            },
            'storedreportedit b4addbutton[name="datasource"]': {
                'click': {
                    fn: me.showDataWindow,
                    scope: me
                }
            },
            'storedreportedit grid[name="dataStore"] b4deletecolumn': {
                'click': {
                    fn: me.deleteRecFromDataSource,
                    scope: me
                }
            },
            'storedreportedit checkbox[name=ForAll]': {
                'change': {
                    fn: me.checkAllClick,
                    scope: me
                }
            },
            'datasourceaddwindow b4closebutton': {
                'click': {
                    fn: me.closeDataWindow,
                    scope: me
                }
            },
            'datasourceaddwindow': {
                'beforesetlist': {
                    fn: me.onBeforeSetDataSourceList,
                    scope: me
                }
            },
            'parameterwindow b4closebutton': {
                'click': {
                    fn: me.closeParamWindow,
                    scope: me
                }
            },
            'parameterwindow b4savebutton': {
                'click': {
                    fn: me.pickParamRecord,
                    scope: me
                }
            },
            'datasourceaddwindow button[name="pickdata"]': {
                'click': {
                    fn: me.pickRecord,
                    scope: me
                }
            },
            'parameterwindow b4enumcombo[name=ParamType]': {
                'change': {
                    fn: me.onParamTypeChange,
                    scope: me
                }
            },
            'storedreportgrid': {
                'selectionchange': {
                    fn: me.onReportSelectionChange,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('storedreportgrid');
        me.bindContext(view);
        me.application.deployView(view);
    },

    checkAllClick: function (chckbx, val) {
        var rolesGrid = this.getRolesGrid();


        rolesGrid.setDisabled(val);
        if (val) {
            rolesGrid.getSelectionModel().deselectAll();
        }
    },

    onTemplateCardShow: function (p) {
        var notif = p.down('#needSaveNotification'),
            panel = p.down('#magicPanel');

        notif.show();
        panel.disable();
    },

    onMagicBtnClick: function (btn) {
        var me = this,
            dataSourcesIds = Ext.Array.map(me.getSelectedDataSources(), function (ds) { return ds.get('Id'); }),
            addConn = me.getAddConnCheckbox().getValue(),
            reportId = me.getReportId().getValue();

        if (dataSourcesIds.length === 0) {
            Ext.Msg.confirm('Внимание', 'Не выбраны источники данных. Продолжить?', function (b) {
                if (b === 'yes') {
                    me.doMagic(btn.action, reportId, addConn);
                }
            });
        } else {
            me.doMagic(btn.action, reportId, addConn);
        }
    },

    doMagic: function (action, reportId, addConn) {
        switch (action) {
            case 'DownloadEmptyTemplate':
                this.downloadEmptyTemplate(reportId, addConn);
                break;
            case 'OpenStimulDesigner':
                this.openStimulDesigner(reportId, addConn);
                break;
        }
    },

    downloadEmptyTemplate: function (reportId, addConn) {
        window.open(B4.Url.action('GetEmptyTemplate', 'EmptyTemplate', {
            addConn: addConn,
            reportId: reportId
        }), '_blank');
    },

    openStimulDesigner: function (reportId, addConn) {
        window.open(B4.Url.action('', 'StimulDesigner', {
            id: reportId,
            addConn: addConn
        }), '_blank');
    },

    onDataSourcesSelected: function (combo, newVal) {
        var downloadBtn = combo.up('storedreportedit').down('button[action=DownloadEmptyTemplate]');
        downloadBtn.setDisabled(!newVal);
    },

    showDataWindow: function () {
        var me = this,
            win = me.getDataWindow() || Ext.widget('datasourceaddwindow');

        win.show();
    },

    closeDataWindow: function (btn) {
        btn.up('datasourceaddwindow').close();
    },

    closeParamWindow: function (btn) {
        btn.up('parameterwindow').close();
    },

    pickRecord: function (btn) {
        var me = this,
            root = btn.up('datasourceaddwindow').down('treepanel').getStore().getRootNode(),
            localStore = me.getStoredReportWindow().down('gridpanel').getStore(),
            selected = Ext.Array.filter(root.childNodes, function (c) { return c.get('checked'); }),
            records = Ext.Array.map(selected, function (s) { return { Name: s.get('text'), Id: s.get('id') } }),
            ids = new String();

        Ext.each(records, function (val, i) {
            ids += val.Id;
            if (records.length - 1 > i) {
                ids += ',';
            }
        });
        localStore.removeAll();
        localStore.add(records);
        btn.up('datasourceaddwindow').close();
    },

    pickParamRecord: function (btn) {
        var me = this,
            win = btn.up('parameterwindow'),
            values = win.getForm().getValues(),
            localStore = me.getStoredReportWindow().down('gridpanel[name=paramGrid]').getStore(),
            paramType = win.down('b4enumcombo[name=ParamType]').value,
            sqlQuery;

        values.Required = values.Required === "on" ? true : false;
        values.Multiselect = values.Multiselect === "on" ? true : false;

        if (paramType === B4.enums.al.ParamType.SqlQuery) {
            sqlQuery = win.down('textarea[name=SqlQuery]').value;
            me.mask('Проверка выполнения sql-запроса', win);
            B4.Ajax.request({
                url: B4.Url.action('CheckSqlQueryParameter', 'StoredReport'),
                params: {
                    sqlQuery: sqlQuery
                },
                timeout: 200000
            }).next(function () {
                me.unmask(win);
                // обвноляем только если редактируем сохранённые параметры
                if (values.Id > 0) {
                    var record = localStore.getById(values.Id);
                    if (record) {
                        localStore.remove(record);
                    }
                }
                localStore.add(values);
                win.close();
            }).error(function (response) {
                me.unmask(win);
                Ext.Msg.alert('Ошибка!', !Ext.isString(response.message) ? 'Ошибка!' : response.message);
            });
        } else {
            // надо просто обновить если уже существует запись
            if (values.Id > 0) {
                var record = localStore.getById(values.Id);
                if (record) {
                    localStore.remove(record);
                }
            }

            localStore.add(values);
            win.close();
        }
    },

    loadDataSourcesStore: function (asp, report) {
        var json,
            store;

        if (!report.get('Id')) {
            return;
        }
        B4.Ajax.request({
            url: B4.Url.action('GetTree', 'StoredReport'),
            params: {
                reportId: report.get('Id')
            }
        }).next(function (response) {
            if (asp.controller.getDataSourceGrid()) {
                json = Ext.JSON.decode(response.responseText);
                store = asp.controller.getDataSourceGrid().getStore();
                store.add(Ext.Array.map(json.data, function (s) { return { Name: s.text, Id: s.id } }));
            }
        });
    },

    loadRolePermissions: function(asp, rec) {
        var rolesGrid = asp.controller.getRolesGrid(),
            forAll = rec.get('ForAll'),
            checkAll = asp.getForm().down('checkbox[name=ForAll]');

        if (!rec.getId()) {
            return;
        }

        asp.controller.mask('Загрузка...', rolesGrid);

        checkAll.setValue(forAll);
        rolesGrid.setDisabled(forAll);

        rolesGrid.getStore().load({
            scope: this,
            callback: function(records) {
                if (!forAll) {
                    B4.Ajax.request({
                        url: B4.Url.action('GetPermissions', 'StoredReport'),
                        params: {
                            reportId: rec.getId()
                        }
                    }).next(function(response) {
                        var resp = Ext.JSON.decode(response.responseText).data,
                            permissions = resp.Permissions,
                            sm = rolesGrid.getSelectionModel();

                        var recsToCheck = [];
                        Ext.each(records, function(record) {
                            for (var i = 0; i < permissions.length; i++) {
                                if (record.data.Id === permissions[i]) {
                                    recsToCheck.push(record);
                                }
                            }
                        });

                        sm.select(recsToCheck);

                        asp.controller.unmask();
                    });
                } else {
                    asp.controller.unmask();
                }
            }
        });
    },

    getSelectedDataSources: function () {
        return this.getDataSourceGrid().getStore().getRange();
    },

    onParamTypeChange: function (combo, newVal, oldVal) {
        var comboCfg =
            {
                xtype: 'b4combobox',
                valueField: 'Id',
                fieldLabel: 'Справочник',
                displayField: 'Display',
                name: 'Additional'
            },
            win = combo.up('parameterwindow'),
            multiChckbx = win.down('checkbox[name=Multiselect]'),
            comboField = win.down('b4combobox[name=Additional]'),
            sqlTextAreaCfg =
            {
                xtype: 'textarea',
                fieldLabel: 'Текст запроса',
                name: 'SqlQuery',
                allowBlank: false,
                maxLength: 2000
            },
            underSqlTextAreaInfoCfg =
            {
                xtype: 'container',
                style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; ' +
                    'background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;">' +
                    '</span></span><span style="display: table-cell; padding-left: 5px;">' +
                    'В результате выполнения запроса должно быть два столбца: Первый столбец - уникальный идентификатор параметра. ' +
                    'По нему необходимо идентифицировать параметр в коде отчета. Значения должны быть уникальны. ' +
                    'Второй столбец - наименование входного параметра. Это значение которое будет отображаться ' +
                    'пользователю при выборе входных параметров.' +
                    '</span>',
                name: 'SqlQueryInfo'
            },
            sqlTextArea = win.down('textarea[name=SqlQuery]'),
            underSqlTextAreaInfo = win.down('container[name=SqlQueryInfo]');;

        win.suspendLayout = true;
        multiChckbx.setValue(false);
        multiChckbx.setDisabled(true);

        if (comboField) {
            win.remove(comboField);
        }

        if (sqlTextArea) {
            win.remove(sqlTextArea);
        }

        if (underSqlTextAreaInfo) {
            win.remove(underSqlTextAreaInfo);
        }

        if (newVal === B4.enums.al.ParamType.Catalog) {
            comboCfg.url = '/CatalogRegistry/List';
            comboCfg.fieldLabel = 'Справочник';
            win.insert(4, comboCfg);
            multiChckbx.setDisabled(false);
        } else if (newVal === B4.enums.al.ParamType.Enum) {
            comboCfg.url = '/EnumRegistry/List';
            comboCfg.fieldLabel = 'Перечисление';
            win.insert(4, comboCfg);
            multiChckbx.setDisabled(false);
        } else if (newVal === B4.enums.al.ParamType.SqlQuery) {
            win.insert(4, sqlTextAreaCfg);
            win.insert(5, underSqlTextAreaInfoCfg);
            multiChckbx.setDisabled(false);
        }

        win.suspendLayout = false;
        win.doLayout();
    },

    deleteRecFromDataSource: function () {
        var me = this,
            record = Array.prototype.slice.call(arguments, 5, 6)[0],
            paramStore = me.getParamsGrid().getStore(),
            localStore = me.getStoredReportWindow().down('grid[name = "dataStore"]').getStore();


        Ext.each(paramStore.data.items, function (val, i) {
            if (val.DataSourceId === record.Id) {
                paramStore.remove(val);
            }
        });
        localStore.remove(record);
        B4.QuickMsg.msg('Инфо', 'Запись успешно удалена', 'info');
    },

    onBeforeSetDataSourceList: function (win, data) {
        var me = this,
            records = me.getDataSourceGrid().getStore().data.items;

        Ext.each(data, function (val, i) {
            Ext.each(records, function (rec, i) {
                if (rec.get('Name') === val.text) {
                    val.checked = true;
                }
            });
        });

        return true;
    },

    onGetExternalLinkBtnClick: function (btn) {
        var selection = btn.up('grid').getSelectionModel().getSelection(),
            report;
        if (selection.length > 0) {
            report = selection[0];
            this.showExternalLinkWindow(report.get('Id'), encodeURIComponent(report.get('Token')));
        }
    },

    showExternalLinkWindow: function (reportId, token) {
        var win = Ext.create('B4.view.al.ExternalLinkWindow', {
            reportId: reportId,
            token: token
        });
        win.show();
    },

    onReportSelectionChange: function (selModel, selected) {
        this.getMainView().down('button[action=GetExternalLink]').setDisabled(selected.length == 0);
    }
});