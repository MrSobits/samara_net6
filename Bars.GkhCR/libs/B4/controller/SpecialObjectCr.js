Ext.define('B4.controller.SpecialObjectCr', {
    /*
    * Контроллер раздела Объектов КР для владельцев специальных счетов
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
        'B4.aspects.permission.SpecialObjectCr', 
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'specialobjectcr.SpecialObjectCr'
    ],
    stores: [
        'specialobjectcr.SpecialObjectCr',
        'specialobjectcr.DeletedSpecialObjectCr',
        'specialobjectcr.ProgramCrObj',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.ProgramCrForSelected',
        'specialobjectcr.TypeWorkCrHistory'
    ],
    views: [
        'specialobjectcr.Panel',
        'specialobjectcr.AddWindow',
        'specialobjectcr.Grid',
        'specialobjectcr.DeletedObjectCrGrid',
        'specialobjectcr.DeletedObjectCrEditWindow',
        'specialobjectcr.FilterPanel',
        'SelectWindow.MultiSelectWindow',
        'specialobjectcr.ImportWindow',
        'specialobjectcr.TypeWorkCrGrid',
        'specialobjectcr.TypeWorkCrHistoryGridForDeleted'
    ],

    mainView: 'specialobjectcr.Panel',
    mainViewSelector: 'specialobjectcrpanel',

    refs: [
        {
            ref: 'ObjectCrFilterPanel',
            selector: 'specialobjectcrfilterpnl'
        },
        {
            ref: 'mainView',
            selector: 'specialobjectcrpanel'
        }
    ],

    aspects: [
        /* 
         *  пермишшен Объекта КР по роли
         */
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhCr.SpecialObjectCrViewCreate.Create', applyTo: 'b4addbutton', selector: 'specialobjectcrgrid' },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.DeletedObject.View', applyTo: 'deletedspecialobjectcrgrid', selector: 'specialobjectcrpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.tab.show();
                        } else {
                            component.tab.hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.DeletedObject.Field.Recover', applyTo: 'button[name=btnRecover]', selector: 'deletedspecialobjectcrgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCrViewCreate.Columns.DateAcceptCrGji', applyTo: 'datecolumn[dataIndex=DateAcceptCrGji]', selector: 'specialobjectcrgrid',
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
            permissions: [
                { name: 'GkhCr.SpecialObjectCr.Delete' }
            ],
            name: 'deleteObjectCrStatePerm'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'objectCrStateTransferAspect',
            gridSelector: 'specialobjectcrpanel specialobjectcrgrid',
            menuSelector: 'objectCrGridStateMenu',
            stateType: 'special_cr_object'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'smrStateTransferAspect',
            gridSelector: 'specialobjectcrgrid',
            menuSelector: 'objectCrGridMonitoringSmrMenu',
            stateType: 'special_cr_obj_monitoring_cmp',
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
             * аспект взаимодействия таблицы объектов КР и формы добавления
             */
            xtype: 'gkhgrideditformaspect',
            name: 'objectCrGridWindowAspect',
            gridSelector: 'specialobjectcrgrid',
            editFormSelector: 'specialobjectcraddwindow',
            storeName: 'specialobjectcr.SpecialObjectCr',
            modelName: 'specialobjectcr.SpecialObjectCr',
            editWindowView: 'specialobjectcr.AddWindow',
            controllerEditName: 'B4.controller.specialobjectcr.Navi',
            otherActions: function (actions) {
                actions[this.editFormSelector + ' b4selectfield[name=ProgramCr]'] = { change: { fn: this.onProgramChange, scope: this } };
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
                        me.controller.application.redirectTo(Ext.String.format('specialobjectcredit/{0}', id));
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
            gridSelector: 'deletedspecialobjectcrgrid',
            editFormSelector: 'specialobjectcraddwindow',
            storeName: 'specialobjectcr.DeletedSpecialObjectCr',
            modelName: 'specialobjectcr.SpecialObjectCr',
            editWindowView: 'specialobjectcr.AddWindow'
        },
        {
            /**
            * Аспект взаимодействия таблицы и формы редактирования договора кап. ремонта
            */
            xtype: 'grideditwindowaspect',
            name: 'deletedObjectCrGridWindowAspect',
            gridSelector: 'deletedspecialobjectcrgrid',
            editFormSelector: 'deletedspecialobjectcrwin',
            storeName: 'specialobjectcr.DeletedSpecialObjectCr',
            modelName: 'specialobjectcr.SpecialObjectCr',
            editWindowView: 'specialobjectcr.DeletedObjectCrEditWindow',
            listeners: {
                aftersetformdata: function (asp, rec) {
                    var me = this,
                        view = me.getForm(),
                        historyGrid = view.down('typeworkspecialcrhistorygridfordeleted'),
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
                this.controller.getStore('specialobjectcr.DeletedSpecialObjectCr').load();
            }
        },
        {
            /*
             * аспект взаимодействия триггер-поля фильтрации мун. районов и таблицы объектов КР
             */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'objectCrfieldmultiselectwindowaspect',
            fieldSelector: 'specialobjectcrfilterpnl [name=tfMunicipality]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#municipalitySpecialSelectWindow',
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
                            municipality = filterPanel.down('[name=tfMunicipality]');

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
             * аспект взаимодействия триггер-поля фильтрации програм КР и таблицы объектов КР
             */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'programCrfieldmultiselectwindowaspect',
            fieldSelector: 'specialobjectcrfilterpnl [name=tfProgramCr]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#programSpecialCrSelectWindow',
            storeSelect: 'specialobjectcr.ProgramCrObj',
            storeSelected: 'dict.ProgramCrForSelected',
            otherActions: function (actions) {
                actions[this.fieldSelector] = { 'beforeload': { fn: this.btnUpdate, scope: this } };
            },
            btnUpdate: function (btn) {
                var form = btn.up(this.fieldSelector);
            },
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
                            programCr = filterPanel.down('[name=tfProgramCr]');

                        programCr.setValue(asp.parseRecord(records, asp.valueProperty));
                        programCr.updateDisplayedText(asp.parseRecord(records, asp.textProperty));
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
            gridSelector: 'specialobjectcrgrid',
            buttonSelector: 'specialobjectcrgrid #btnObjectCrExport',
            controllerName: 'SpecialObjectCr', //!!
            actionName: 'Export'
        }
    ],

    init: function () {
        var me = this;

        me.getStore('specialobjectcr.SpecialObjectCr').on('beforeload', me.onBeforeLoad, me);
        me.getStore('specialobjectcr.DeletedSpecialObjectCr').on('beforeload', me.onBeforeLoadDeleted, me);

        me.control({
            '#specialObjectCrTabPanel': { 'tabchange': { fn: me.changeTab, scope: me } },
            'deletedspecialobjectcrgrid [name=btnRecover]': { 'click': { fn: me.recover, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('specialobjectcrpanel');
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('specialobjectcr.SpecialObjectCr').load();

        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'GkhParams')
        }).next(function (response) {
            var settlementCol = view.down('specialobjectcrgrid [dataIndex=Settlement]'),
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
        this.setFilterParams(operation);
    },

    onBeforeLoadDeleted: function (store, operation) {
        this.setFilterParams(operation);
        operation.params.deleted = true;
    },

    setFilterParams: function (operation) {
        var filterPanel = this.getObjectCrFilterPanel();
        if (filterPanel) {
            operation.params.programId = filterPanel.down('[name=tfProgramCr]').getValue();
            operation.params.municipalityId = filterPanel.down('[name=tfMunicipality]').getValue();
        }
    },

    recover: function (btn) {
        var me = this,
            grid = btn.up('deletedspecialobjectcrgrid'),
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
                        url: B4.Url.action('Recover', 'SpecialObjectCr'),
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
    }
});