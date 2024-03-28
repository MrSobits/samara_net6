Ext.define('B4.controller.ObjectCr', {
    /*
    * Контроллер раздела Объектов КР
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.GkhButtonImportAspect',
        'B4.form.ComboBox',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.ObjectCr',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'ObjectCr'
    ],
    stores: [
        'ObjectCr',
        'DeletedObjectCr',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.ProgramCrObj',
        'dict.ProgramCrForSelected',
        'objectcr.TypeWorkCrHistory'
    ],
    views: [
        'objectcr.Panel',
        'objectcr.AddWindow',
        'objectcr.Grid',
        'objectcr.ProgramSelectWindow',
        'objectcr.DeletedObjectCrGrid',
        'objectcr.DeletedObjectCrEditWindow',
        'objectcr.FilterPanel',
        'SelectWindow.MultiSelectWindow',
        'objectcr.ImportWindow',
        'objectcr.TypeWorkCrGrid',
        'objectcr.TypeWorkCrHistoryGridForDeleted'
    ],

    mainView: 'objectcr.Panel',
    mainViewSelector: 'objectCrPanel',

    refs: [
        {
            ref: 'ObjectCrFilterPanel',
            selector: 'objectcrfilterpnl'
        },
        {
            ref: 'mainView',
            selector: 'objectCrPanel'
        }
    ],

    aspects: [
        /* 
         *  пермишшен Объекта КР по роли
         */
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhCr.ObjectCrViewCreate.Create', applyTo: 'b4addbutton', selector: '#objectCrGrid' },
                {
                    name: 'GkhCr.ObjectCrViewCreate.GjiNumberFill', applyTo: '#btnGjiNumberFill', selector: '#objectCrGrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.DeletedObject.View', applyTo: 'deletedobjectcrgrid', selector: '#objectCrTabPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.tab.show();
                        } else {
                            component.tab.hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.DeletedObject.Field.Recover', applyTo: '#btnRecover', selector: 'deletedobjectcrgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.ObjectCrViewCreate.Columns.DateAcceptCrGji', applyTo: '#DateAcceptCrGji', selector: 'objectcrgrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                        component.ownerCt.menu.down('[headerId=' + component.id + ']').setVisible(allowed);
                    }
                }
            ]
        },
        /*
         * пермишшен по удалению записи Объекта КР (по его статусы), вынесен в отдельный аспект для  удобства 
         */
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhCr.ObjectCr.Delete' }],
            name: 'deleteObjectCrStatePerm'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'objectCrStateTransferAspect',
            gridSelector: 'objectCrPanel #objectCrGrid',
            menuSelector: 'objectCrGridStateMenu',
            stateType: 'cr_object'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'smrStateTransferAspect',
            gridSelector: '#objectCrGrid',
            menuSelector: 'objectCrGridMonitoringSmrMenu',
            stateType: 'cr_obj_monitoring_cmp',
            stateProperty: 'MonitoringSmrState',
            entityIdProperty: 'MonitoringSmrId',
            cellClickAction: function (grid, e, action, record) {
                switch (action.toLowerCase()) {
                    case 'statechangesmr':
                        e.stopEvent();
                        var menu = this.getContextMenu();
                        this.currentRecord = record;
                        menu.updateData(record, e.xy, this.stateType, this.stateProperty);
                        break;
                }
            }
        },
        {
            /*
            *аспект для импорта
            */
            xtype: 'gkhbuttonimportaspect',
            name: 'objectCrImportAspect',
            buttonSelector: '#objectCrGrid #btnImport',
            codeImport: 'ObjectCr',
            windowImportView: 'objectcr.ImportWindow',
            windowImportSelector: '#importObjectCrWindow',
            listeners: {
                aftercreatewindow: function(window, importId) {
                    var label = window.down('#containerInfo'),
                        chk1 = window.down('[name=HasLinkOverhaul]'),
                        chk2 = window.down('[name=ReplaceObjectCr]'),
                        chk3 = window.down('[name=ReplaceTypeWork]'),
                        programCr = window.down('[name=ProgramCr]'),
                        fileField = window.down('b4filefield[name=FileImport]'),
                        info = window.down('container[name=ImportInfo]');

                    label.setVisible(importId === 'Bars.GkhCr.Import.FinanceSourceImport');

                    if (importId === 'Bars.GkhCr.Import.ContractsImport') {
                        info.show();
                        programCr.hide();
                        programCr.setDisabled(true);
                        fileField.possibleFileExtensions = 'xls,xlsx';
                    }

                    if (importId === 'Bars.GkhCr.Import.ProgramCrImport') {
                        chk1.show();
                        chk2.show();
                        chk3.show();
                    } else {
                        chk1.hide();
                        chk2.hide();
                        chk3.hide();
                    }
                }
            }
        },
        {
            /*
             * аспект взаимодействия таблицы объектов КР и формы добавления
             */
            xtype: 'gkhgrideditformaspect',
            name: 'objectCrGridWindowAspect',
            gridSelector: '#objectCrGrid',
            editFormSelector: '#objectCrAddWindow',
            storeName: 'ObjectCr',
            modelName: 'ObjectCr',
            editWindowView: 'objectcr.AddWindow',
            controllerEditName: 'B4.controller.objectcr.Navi',
            otherActions: function (actions) {
                actions['#objectCrAddWindow b4selectfield[name=ProgramCr]'] = { change: { fn: this.onProgramChange, scope: this } };
            },
            onProgramChange: function (field, newValue) {
                var roField = field.up().down('b4selectfield[name = RealityObject]');

                if (newValue) {
                    roField.setDisabled(false);
                } else {
                    roField.setDisabled(true);
                }
            },
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteObjectCrStatePerm').loadPermissions(record)
                        .next(function (response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] === 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result === 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        rec.destroy()
                                            .next(function () {
                                                this.fireEvent('deletesuccess', this);
                                                me.updateGrid();
                                                me.unmask();
                                            }, this)
                                            .error(function (result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, this);
                                    }
                                }, me);
                            }

                        }, this);
                }
            },
            rowAction: function (grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                    }
                }
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.get('Id') : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('objectcredit/{0}', id));
                    } else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            }
        },
        {
            /*
             * аспект взаимодействия таблицы удаленных объектов КР и формы добавления
             */
            xtype: 'gkhgrideditformaspect',
            name: 'deletedObjectCrGridWindowAspect',
            gridSelector: 'deletedobjectcrgrid',
            editFormSelector: '#objectCrAddWindow',
            storeName: 'DeletedObjectCr',
            modelName: 'ObjectCr',
            editWindowView: 'objectcr.AddWindow'
        },
        {
            /**
             * Аспект взаимодействия таблицы и формы редактирования договора кап. ремонта
             */
            xtype: 'grideditwindowaspect',
            name: 'deletedObjectCrGridWindowAspect',
            gridSelector: 'deletedobjectcrgrid',
            editFormSelector: 'deletedobjectwin',
            storeName: 'DeleteObjectCr',
            modelName: 'ObjectCr',
            editWindowView: 'objectcr.DeletedObjectCrEditWindow',
            listeners: {
                aftersetformdata: function (asp, rec) {
                    var me = this,
                        view = me.getForm(),
                        historyGrid = view.down('typeworkhistorygridfordeleted'),
                        store = historyGrid.getStore();

                    store.clearFilter(true);
                    store.filter('objectCrId', rec.get('Id'));
                }
            },
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' b4updatebutton'] = { 'click': { fn: me.updateGrid, scope: me } };
            },
            updateGrid: function () {
                this.controller.getStore('DeletedObjectCr').load();
            }
        },
        {
            /*
             * аспект взаимодействия триггер-поля фильтрации мун. районов и таблицы объектов КР
             */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'objectCrfieldmultiselectwindowaspect',
            fieldSelector: 'objectcrfilterpnl #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#municipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'Okato', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранные записи',
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        var filterPanel = asp.controller.getObjectCrFilterPanel(),
                            municipality = filterPanel.down('#tfMunicipality');

                        municipality.setValue(asp.parseRecord(records, asp.valueProperty));
                        municipality.updateDisplayedText(asp.parseRecord(records, asp.textProperty));
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать одно или несколько муниципальных образования');
                        return false;
                    }
                    return true;
                }
            }
        },
{
            /*
             * фильтр по видам работ
             */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'objectcrcrworkmultiselect',
            fieldSelector: 'objectcrfilterpnl #tfWorks',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#workCrSelectWindow',
            storeSelect: 'dict.WorkSelect',
            storeSelected: 'dict.WorkSelected',
            columnsGridSelect: [
                { header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранные записи',
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        var filterPanel = asp.controller.getObjectCrFilterPanel(),
                            workfield = filterPanel.down('#tfWorks');

                        workfield.setValue(asp.parseRecord(records, asp.valueProperty));
                        workfield.updateDisplayedText(asp.parseRecord(records, asp.textProperty));
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать один или несколько видом работ');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
             * аспект взаимодействия триггер-поля фильтрации програм КР и таблицы объектов КР
             */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'programCrfieldmultiselectwindowaspect',
            fieldSelector: 'objectcrfilterpnl #tfProgramCr',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#programCrSelectWindow',
            storeSelect: 'dict.ProgramCrObj',
            storeSelected: 'dict.ProgramCrForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } },
                {
                    header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Period/List'
                    }
                }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
                { header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранные записи',
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        var filterPanel = asp.controller.getObjectCrFilterPanel(),
                            municipality = filterPanel.down('#tfProgramCr');

                        municipality.setValue(asp.parseRecord(records, asp.valueProperty));
                        municipality.updateDisplayedText(asp.parseRecord(records, asp.textProperty));
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать одну или несколько програм КР');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'objectCrButtonExportAspect',
            gridSelector: '#objectCrGrid',
            buttonSelector: '#objectCrGrid #btnObjectCrExport',
            controllerName: 'ObjectCr',
            actionName: 'Export'
        }
    ],

    init: function () {
        var me = this;

        me.getStore('ObjectCr').on('beforeload', me.onBeforeLoad, me);
        me.getStore('DeletedObjectCr').on('beforeload', me.onBeforeLoadDeleted, me);

        me.control({
            '#objectCrTabPanel': { 'tabchange': { fn: me.changeTab, scope: me } },
            'objectcrgrid #btnGjiNumberFill': { 'click': { fn: me.onGjiNumberFillButtonClick, scope: me } },
            'objectcrprogramselectwindow b4savebutton': { 'click': { fn: me.onGjiNumberFillApply, scope: me } },
            'deletedobjectcrgrid #btnRecover': { 'click': { fn: me.recover, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('objectCrPanel');
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('ObjectCr').load();
        me.getAspect('objectCrImportAspect').loadImportStore();

        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'GkhParams')
        }).next(function (response) {
            var settlementCol = view.down('objectcrgrid [dataIndex=Settlement]'),
                json = Ext.JSON.decode(response.responseText);

            if (settlementCol) {
                if (json.ShowStlObjectCrGrid) {
                    settlementCol.show();
                } else {
                    settlementCol.hide();
                }
            }

        }).error(function () {
            Ext.Msg.alert('Ошибка!', 'Ошибка получения параметров приложения');
        });
    },

    changeTab: function (tabPanel, newTab, oldTab) {
        newTab.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        var filterPanel = this.getObjectCrFilterPanel();
        if (filterPanel) {
            operation.params.programId = filterPanel.down('#tfProgramCr').getValue();
            operation.params.municipalityId = filterPanel.down('#tfMunicipality').getValue();
            operation.params.isbuildContr = filterPanel.down('#cbbuildContr').getValue();
            operation.params.workId = filterPanel.down('#tfWorks').getValue();
        }
    },

    onBeforeLoadDeleted: function (store, operation) {
        var filterPanel = this.getObjectCrFilterPanel();
        if (filterPanel) {
            operation.params.programId = filterPanel.down('#tfProgramCr').getValue();
            operation.params.municipalityId = filterPanel.down('#tfMunicipality').getValue();
            operation.params.isbuildContr = filterPanel.down('#cbbuildContr').getValue();
            operation.params.workId = filterPanel.down('#tfWorks').getValue();
            operation.params.deleted = true;
        }
    },

    recover: function (btn) {
        var me = this,
            grid = btn.up('deletedobjectcrgrid'),
            selected = Ext.Array.map(grid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); }),
            message;

        if (selected.length === 0) {
            Ext.Msg.alert('Сообщение', 'Выберите объекты для восстановления!');
        } else if (selected.length === 1) {
            message = 'Восстановить выбранный объект?';
        } else {
            message = 'Восстановить ' + selected.length + ' выбранных объекта?';
        }

        if (!Ext.isEmpty(message)) {
            Ext.Msg.confirm('Восстановление объекта', message, function (result) {
                if (result === 'yes') {
                    me.mask('Восстановление', B4.getBody().getActiveTab());
                    B4.Ajax.request({
                        url: B4.Url.action('Recover', 'ObjectCr'),
                        timeout: 9999999,
                        params: {
                            selected: selected
                        }
                    }).next(function () {
                        B4.QuickMsg.msg('Восстановление объектов КР', 'Объекты успешно восстановлены', 'success');
                        grid.getStore().load();
                        me.unmask();
                    }).error(function (error) {
                        B4.QuickMsg.msg('Восстановление объектов КР', error.message || error, 'error');
                        me.unmask();
                    });
                }
            }, me);
        }
    },

    onGjiNumberFillButtonClick: function (btn) {
        var me = this,
            win = Ext.create('B4.view.objectcr.ProgramSelectWindow', {
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy',
                windowText: 'Выберите программу КР, для объектов которой необходимо заполнить поле "Номер ГЖИ".'
            }),
            programCrSf = win.down('b4selectfield[name=ProgramCr]');

        programCrSf.getStore().on('beforeload', function (store, operation) {
            operation.params = operation.params || {};
            // Программы, где видимость = Полная
            operation.params.onlyFull = true;
        }, me);

        win.show();
    },

    onGjiNumberFillApply: function (btn) {
        var me = this,
            win = btn.up('objectcrprogramselectwindow'),
            programCrSf = win.down('b4selectfield[name=ProgramCr]'),
            selectedProgramCrId = programCrSf.getValue();

        if (!selectedProgramCrId || selectedProgramCrId === 0) {
            Ext.Msg.alert('Внимание!', 'Необходимо выбрать запись!');
            return;
        }

        me.mask('Заполнение...', win);

        B4.Ajax.request({
            url: B4.Url.action('GjiNumberFill', 'ProgramCr'),
            params: {
                programCrId: selectedProgramCrId
            },
            timeout: 9999999
        }).next(function () {
            me.unmask();
            win.close();
            Ext.Msg.alert('Заполнение номера ГЖИ', 'Выполнено успешно');
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При заполнении номера ГЖИ возникла ошибка: ' + e.message);
        });
    }
});