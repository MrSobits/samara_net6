Ext.define('B4.controller.RealEstateType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.InlineGrid',
        'B4.view.realestatetype.Grid',
        'B4.view.realestatetype.CommonParamGrid',
        'B4.view.realestatetype.Edit',
        'B4.view.realestatetype.TreeMultiSelect',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'RealEstateType',
        'RealEstateTypeCommonParam',
        'RealEstateTypeStructElement',
        'RealEstateTypePriorityParam',
        'realestatetype.Municipality'
    ],

    stores: [
        'RealEstateType',
        'RealEstateTypeCommonParam',
        'RealEstateTypeStructElement',
        'RealEstateTypePriorityParam',
        'realestatetype.Municipality',
        'dict.MunicipalityForSelected',
        'realestatetype.MunicipalityForSelect'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'realestatetype.Grid',
        'realestatetype.Edit',
        'realestatetype.CommonParamGrid',
        'realestatetype.StructElemGrid',
        'realestatetype.PriorityParamGrid',
        'realestatetype.MunicipalityGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'realestatetypegrid'
        },
        {
            ref: 'editView',
            selector: 'realestatetypeedit'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Ovrhl.RealEstateType.Municipality.View', applyTo: 'retmunicipalitygrid', selector: 'realestatetypeedit',
                    applyBy: function (component, allowed) {
                        if (component && component.tab) {
                            component.tab.setVisible(allowed);
                        }
                    }
                },
                { name: 'Ovrhl.RealEstateType.Municipality.Create', applyTo: 'b4addbutton', selector: 'retmunicipalitygrid' },
                { name: 'Ovrhl.RealEstateType.Municipality.Delete', applyTo: 'b4deletecolumn', selector: 'retmunicipalitygrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'inlinegridaspect',
            name: 'commonParamGridAspect',
            modelName: 'RealEstateTypeCommonParam',
            gridSelector: 'commonparameditgrid',
            updateGrid: function() {
                this.getGrid().getStore().load();
            },
            listeners: {
                beforesave: function(asp, store) {
                    var result = true;

                    Ext.each(store.data.items, function(item) {
                        return result = asp.validateCommonParams(item);
                    });

                    if (!result) {
                        B4.QuickMsg.msg('Ошибка', 'Все записи должны быть заполнены', 'error');
                        return false;
                    }

                    return result;
                }
            },

            validateCommonParams: function(item) {
                return !Ext.isEmpty(item.get('CommonParamCode')) && !Ext.isEmpty(item.get('Min')) && !Ext.isEmpty('Max');
            },

            addRecord: function() {
                var plugin,
                    store = this.getGrid().getStore(),
                    rec = this.controller.getModel(this.modelName).create(),
                    grid = this.getGrid();

                store.insert(0, rec);

                if (this.cellEditPluginId && grid) {
                    plugin = grid.getPlugin(this.cellEditPluginId);
                    plugin.startEditByPosition({ row: 0, column: this.firstEditColumnIndex });
                }
            },

            deleteRecord: function(record) {
                var me = this,
                    store = me.getGrid().getStore();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                    if (result == 'yes') {
                        store.remove(record);
                    }
                }, me);
            },

            save: function() {
                var me = this,
                    store = me.getGrid().getStore(),
                    modifiedRecs = store.getModifiedRecords(),
                    removedRecs = store.getRemovedRecords();

                if (modifiedRecs.length > 0 || removedRecs.length > 0) {
                    if (me.fireEvent('beforesave', me, store) !== false) {
                        me.mask('Сохранение', me.getGrid());
                        store.sync({
                            callback: function() {
                                me.unmask();
                                store.load();
                            },
                            failure: me.handleDataSyncError,
                            scope: me
                        });
                    }
                }
            }
        },
        {
            xtype: 'inlinegridaspect',
            name: 'realEstateTypePriorityAspect',
            storeName: 'RealEstateTypePriorityParam',
            modelName: 'RealEstateTypePriorityParam',
            gridSelector: 'realestatetypepriorityparamgrid',
            addRecord: function() {
                var plugin,
                    store = this.controller.getStore(this.storeName),
                    rec = this.controller.getModel(this.modelName).create(),
                    grid = this.getGrid();

                rec.set('RealEstateType', this.controller.getEditView().objId);

                store.insert(0, rec);

                if (this.cellEditPluginId && grid) {
                    plugin = grid.getPlugin(this.cellEditPluginId);
                    plugin.startEditByPosition({ row: 0, column: this.firstEditColumnIndex });
                }
            },

            gridAction: function(grid, action) {
                if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
                    switch (action.toLowerCase()) {
                    case 'add':
                        this.addRecord();
                        break;
                    case 'update':
                        this.updateGrid();
                        break;
                    case 'save':
                        if (this.beforeSave(grid) !== false) {
                            this.save();
                        }
                        break;
                    }
                }
            },

            beforeSave: function(grid) {
                var me = this,
                    store = grid.getStore(),
                    valid = true;

                store.each(function(rec) {
                    valid = me.validatePriorityParam(rec);
                    return valid;
                });

                if (!valid) {
                    B4.QuickMsg.msg("Ошибка", "Все записи должны быть заполнены", "error");
                    return false;
                }

                return true;
            },

            validatePriorityParam: function(rec) {
                return !Ext.isEmpty(rec.get('Code')) && !Ext.isEmpty(rec.get('Weight'));
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'realestatetypemuAspect',
            gridSelector: 'retmunicipalitygrid',
            storeName: 'realestatetype.Municipality',
            modelName: 'realestatetype.Municipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#realestatetypeMuMultiSelectWindow',
            storeSelect: 'realestatetype.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            titleSelectWindow: 'Выбор муниципальных образований',
            titleGridSelect: 'Муниципальные образования для отбора',
            titleGridSelected: 'Выбранные муниципальные образования',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [],
                        editView = asp.controller.getEditView();

                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', editView);
                        B4.Ajax.request({
                            url: B4.Url.action('AddMunicipality', 'RealEstateTypeMunicipality'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                retId: editView.objId
                            }
                        }).next(function() {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            Ext.Msg.alert('Сохранение!', 'Муниципальные образования сохранены успешно');
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать муниципальные образования');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function() {
        var me = this,
            actions = {
                'realestatetypegrid': {
                    rowaction: me.onRowAction,
                    scope: me
                },
                'realestatetypegrid b4addbutton': {
                    click: me.onRealEstateTypeAdd,
                    scope: me
                },
                'realestatetypegrid b4updatebutton': {
                    click: me.onRealEstateTypeUpdate,
                    scope: me
                },
                'realestatetypeedit': {
                    render: me.onEditViewRender,
                    scope: me
                },
                'realestatetypeedit form b4savebutton': {
                    click: me.onRealEstateSave,
                    scope: me
                },
                'realestatetypeedit commonparameditgrid b4savebutton': {
                    click: me.onRealEstateCommonTypeSave,
                    scope: me
                },
                'realestatetypeedit structelemeditgrid b4addbutton': {
                    click: me.onRealEstateStructElemAdd,
                    scope: me
                },
                'realestatetypeedit structelemeditgrid b4updatebutton': {
                    click: me.onStructElGridUpdate,
                    scope: me
                },
                'realestatetypeedit structelemeditgrid b4savebutton': {
                    click: me.onStructElGridSave,
                    scope: me
                },
                'structelemeditgrid': {
                    rowaction: me.onStructElemGridRowAction,
                    render: me.onStructElemGridRender
                },
                'realesttypestructelselect b4savebutton': {
                    click: me.saveStructElems
                }
            };
        me.control(actions);
        me.callParent(arguments);
    },

    index: function() {
        var view = this.getMainView() || Ext.widget('realestatetypegrid');
        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    },

    edit: function(id) {
        var view = this.getEditView() || Ext.widget('realestatetypeedit', {
            objId: id
        });
        this.bindContext(view);
        this.application.deployView(view);

        //view.getStore().load();
    },

    onRealEstateTypeAdd: function(btn) {
        Ext.History.add('edit_realestatetype/0');
    },

    onRealEstateTypeUpdate: function(btn) {
        btn.up('realestatetypegrid').getStore().load();
    },

    // RowAction event handling
    onRowAction: function(grid, action, record) {
        switch (action.toLowerCase()) {
        case 'edit':
            Ext.History.add('edit_realestatetype/' + record.getId());
            break;
        case 'delete':
            this.deleteRealEstateType(record.getId(), grid);
            break;
        }
    },

    deleteRealEstateType: function (id, grid) {
        var me = this;
        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
            if (result == 'yes') {

                me.mask('Удаление', B4.getBody());
                B4.Ajax.request({
                    url: B4.Url.action('Delete', 'RealEstateType'),
                    method: 'POST',
                    params: { records: Ext.encode([id]) }
                }).next(function () {
                    me.unmask();
                    grid.getStore().load();
                }).error(function (result) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                });
            }
        });
    },

    onEditViewRender: function(editView) {
        var me = this,
            id = +editView.objId,
            form = editView.down('form'),
            tabpanel = editView.down('tabpanel'),
            muStore = editView.down('retmunicipalitygrid').getStore(),
            commonParamStore = editView.down('commonparameditgrid').getStore(),
            structElemStore = editView.down('structelemeditgrid').getStore(),
            priorityParamStore = editView.down('realestatetypepriorityparamgrid').getStore(),
            estateObj,
            realEstateType;

        tabpanel.disable();
        muStore.on('beforeload', me.onBeforeLoadChild, me);
        commonParamStore.on('beforeload', me.onBeforeLoadChild, me);
        structElemStore.on('beforeload', me.onBeforeLoadChild, me);
        priorityParamStore.on('beforeload', me.onBeforeLoadChild, me);

        muStore.load();
        commonParamStore.load();
        priorityParamStore.load();
        structElemStore.load();
        if (!id) {
            estateObj = Ext.create('B4.model.RealEstateType');
            form.loadRecord(estateObj);
        } else {
            realEstateType = Ext.ModelMgr.getModel('B4.model.RealEstateType');
            realEstateType.load(id, {
                success: function(obj) {
                    form.loadRecord(obj);
                    tabpanel.setDisabled(false);
                }
            });
        }
    },

    onRealEstateSave: function(btn) {
        var editView = btn.up('realestatetypeedit'),
            form = editView.down('form'),
            tabpanel = editView.down('tabpanel'),
            record = form.getRecord(),
            values = form.getValues();

        if (!values || !values.Name) {
            B4.QuickMsg.msg('Ошибка', 'Значение поля "Наименование" необходимо заполнить.', 'failure');
        } else {
            record.set(form.getValues());
            record.save({
                success: function(rec) {
                    editView.objId = rec.getId();
                    form.loadRecord(rec);
                    tabpanel.setDisabled(false);
                    B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
                }
            });
        }
    },

    onRealEstateCommonTypeSave: function(btn) {
        var editView = btn.up('realestatetypeedit'),
            commonParamGrid = editView.down('commonparameditgrid'),
            store = commonParamGrid.getStore();

        store.each(function(rec) {
            rec.set('RealEstateType', editView.objId);
        });
    },

    onRealEstateStructElemAdd: function(btn) {
        btn.multiSelectTreeWin = Ext.create('B4.view.realestatetype.TreeMultiSelect', {
            realEstateTypeId: btn.up('realestatetypeedit').objId
        });
        btn.multiSelectTreeWin.show();
    },

    saveStructElems: function(btn) {
        var me = this,
            win = btn.up('realesttypestructelselect'),
            grid = win.down('treepanel'),
            root = grid.getRootNode(),
            records = [],
            editView = me.getEditView();

        root.cascadeBy(function(record) {
            if (record.get('checked')) {
                records.push(record);
            }
        });

        records = Ext.Array.map(records, function(rec) {
            return {
                RealEstateType: editView.objId,
                StructuralElement: rec.get('ElemId'),
                Exists: rec.get('Exists')
            };
        });

        B4.Ajax.request({
            url: B4.Url.action('Create', 'RealEstateTypeStructElement'),
            method: 'POST',
            params: {
                records: Ext.encode(records)
            }
        }).next(function(response) {
            editView.down('structelemeditgrid').getStore().load();
            win.close();
        }).error(function() {
            Ext.Msg.alert('Ошибка!', 'При выполнении запроса произошла ошибка!');
        });
    },

    onStructElemGridRowAction: function(grid, action, record) {
        switch (action.toLowerCase()) {
        case 'delete':
            Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                if (result == 'yes') {
                    B4.Ajax.request({
                        url: B4.Url.action('Delete', 'RealEstateTypeStructElement'),
                        method: 'POST',
                        params: { records: Ext.encode([record.getId()]) }
                    }).next(function(resp) {
                        grid.getStore().load();
                    }).error(function() {
                    });
                }
            }, this);
            break;
        }
    },

    onStructElemGridRender: function(grid) {
        grid.getStore().load();
    },

    onBeforeLoadChild: function (store, operation) {
        operation.params.RealEstateTypeId = this.getEditView().objId;
    },

    onStructElGridUpdate: function(btn) {
        btn.up('structelemeditgrid').getStore().load();
    },

    onStructElGridSave: function(btn) {
        var store = btn.up('structelemeditgrid').getStore();
        var records = store.getModifiedRecords();
        records = Ext.Array.map(records, function(rec) {
            return {
                Id: rec.get('Id'),
                Exists: rec.get('Exists')
            };
        });
        B4.Ajax.request({
            url: B4.Url.action('Update', 'RealEstateTypeStructElement'),
            method: 'POST',
            params: {
                records: Ext.encode(records)
            }
        }).next(function(response) {
            store.load();
        }).error(function() {
            Ext.Msg.alert('Ошибка!', 'При выполнении запроса произошла ошибка!');
        });
    }
});