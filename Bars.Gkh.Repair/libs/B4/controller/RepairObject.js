Ext.define('B4.controller.RepairObject', {
    /*
    * Контроллер раздела Объектов текущего ремонта
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.form.ComboBox',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.repairobject.RepairObject',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['RepairObject'],
    stores: [
        'RepairObject',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.RepairProgramObj',
        'dict.RepairProgramForSelected',
        'dict.RepairProgramRo'
    ],
    views: [
        'repairobject.Panel',
        'repairobject.AddWindow',
        'repairobject.Grid',
        'repairobject.FilterPanel'
    ],

    mainView: 'repairobject.Panel',
    mainViewSelector: 'repairObjectPanel',

    refs: [
        {
            ref: 'RepairObjectFilterPanel',
            selector: '#repairObjectFilterPanel'
        },
        {
            ref: 'mainView',
            selector: 'repairObjectPanel'
        }
    ],

    aspects: [
        /* пермишшен Объекта текущего ремонта по роли */
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRepair.RepairObjectViewCreate.Create', applyTo: 'b4addbutton', selector: '#repairObjectGrid' }
            ]
        },
        {
            xtype: 'repairobjectstateperm'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhRepair.RepairObject.Delete' }],
            name: 'deleteRepairObjectStatePerm'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'repairObjectStateTransferAspect',
            gridSelector: 'repairObjectPanel #repairObjectGrid',
            menuSelector: 'repairObjectGridStateMenu',
            stateType: 'repair_object'
        },
        {
            /*
            аспект взаимодействия таблицы объектов текущего ремонта и формы добавления
            */
            xtype: 'gkhgrideditformaspect',
            name: 'repairObjectGridWindowAspect',
            gridSelector: '#repairObjectGrid',
            editFormSelector: '#repairObjectAddWindow',
            storeName: 'RepairObject',
            modelName: 'RepairObject',
            editWindowView: 'repairobject.AddWindow',
            controllerEditName: 'B4.controller.repairobject.Navigation',
            otherActions: function (actions) {
                actions['#repairObjectAddWindow #sfRepairProgram'] = {
                    'change': { fn: this.onChangeRepairProgram, scope: this },
                    'beforeload': { fn: this.onBeforeLoadRepairProgram, scope: this }
                };
                actions['#repairObjectAddWindow #sfRealityObject'] = { 'beforeload': { fn: this.onBeforeLoadRealtyObject, scope: this } };
            },
            onBeforeLoadRepairProgram: function (store, operation) {
                operation.params.forRepairObject = true;
            },
            onChangeRepairProgram: function (field, newValue) {
                var sfRealityObject = field.up().down('#sfRealityObject');
                if (sfRealityObject) {
                    sfRealityObject.repairProgramId = newValue.Id;
                }
            },
            onBeforeLoadRealtyObject: function (store, operation) {
                var asp = this,
                    sfRealityObject = asp.getForm().down('#sfRealityObject');
                
                if (sfRealityObject) {
                    operation = operation || {};
                    operation.params = operation.params || {};
                    operation.params.repairProgramId = sfRealityObject.repairProgramId;
                }
            },
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteRepairObjectStatePerm').loadPermissions(record)
                        .next(function(response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                                    if (result == 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        rec.destroy()
                                            .next(function() {
                                                this.fireEvent('deletesuccess', this);
                                                me.updateGrid();
                                                me.unmask();
                                            }, this)
                                            .error(function(result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, this);
                                    }
                                }, me);
                            }

                        }, this);
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля фильтрации мун. образований и таблицы объектов текущего ремонта
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'repairObjectfieldmultiselectwindowaspect',
            fieldSelector: '#repairObjectFilterPanel #tfMunicipality',
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
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        var filterPanel = asp.controller.getRepairObjectFilterPanel();

                        var municipality = filterPanel.down('#tfMunicipality');
                        municipality.setValue(this.parseRecord(records, this.valueProperty));
                        municipality.updateDisplayedText(this.parseRecord(records, this.textProperty));
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
            аспект взаимодействия триггер-поля фильтрации программ текущего ремонта и таблицы объектов текущего ремонта
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'programRepairProgramfieldmultiselectwindowaspect',
            fieldSelector: '#repairObjectFilterPanel #tfRepairProgram',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#repairProgramSelectWindow',
            storeSelect: 'dict.RepairProgramForSelect',
            storeSelected: 'dict.RepairProgramForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
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
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                operation.params.forRepairObject = true;
            },
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        var filterPanel = asp.controller.getRepairObjectFilterPanel();

                        var repairProgram = filterPanel.down('#tfRepairProgram');
                        repairProgram.setValue(this.parseRecord(records, this.valueProperty));
                        repairProgram.updateDisplayedText(this.parseRecord(records, this.textProperty));
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать одну или несколько программ текущего ремонта');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'repairObjectButtonExportAspect',
            gridSelector: '#repairObjectGrid',
            buttonSelector: '#repairObjectGrid #btnRepairObjectExport',
            controllerName: 'RepairObject',
            actionName: 'Export'
        }
    ],

    init: function () {
        this.getStore('RepairObject').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('repairObjectPanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('RepairObject').load();
    },

    onBeforeLoad: function (store, operation) {
        var filterPanel = this.getRepairObjectFilterPanel();
        if (filterPanel){
            operation.params.programId = filterPanel.down('#tfRepairProgram').getValue();
            operation.params.municipalityId = filterPanel.down('#tfMunicipality').getValue();
        }
    }
});