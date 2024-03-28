Ext.define('B4.controller.dict.ViolationGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.dict.Violation',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhBlobText'
    ],

    models: [
        'dict.ViolationGji',
        'dict.ViolationFeatureGji',
        'dict.ViolationActionsRemovGji',
        'dict.ViolationNormativeDocItemGji'
    ],
    stores: [
        'dict.ViolationGji',
        'dict.ViolationFeatureGji',
        'dict.ViolationActionsRemovGji',
        'dict.FeatureViolGjiForSelect',
        'dict.FeatureViolGjiForSelected',
        'dict.ActionsRemovViolForSelect',
        'dict.ActionsRemovViolForSelected',
        'dict.NormativeDoc',
        'dict.NormativeDocItem',
        'dict.NormativeDocItemGrouping',
        'dict.FeatureViolGji',
        'dict.ViolationFeatureGroupsGji',
        'dict.ViolationNormativeDocItemGji',
        'dict.NormativeDocItemTreeStore'
    ],
    views: [
        'dict.violationgji.Grid',
        'dict.violationgji.EditWindow',
        'dict.violationgji.ViolationGroupsGjiGrid',
        'dict.violationgji.ViolationActionsRemovGrid',
        'SelectWindow.MultiSelectWindow',
        'dict.violationgji.ViolationNormativeDocItemGrid'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.violationgji.Grid',
    mainViewSelector: 'violationGjiGrid',

    violationGjiEditWindowSelector: '#violationGjiEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'violationGjiGrid'
        }
    ],

    aspects: [
        {
            // аспект кнопки экспорта реестра
            xtype: 'b4buttondataexportaspect',
            name: 'violationGjiButtonExportAspect',
            gridSelector: 'violationGjiGrid',
            buttonSelector: 'violationGjiGrid #btnExport',
            controllerName: 'ViolationGji',
            actionName: 'Export'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'normativeDocItemGridAspect',
            storeName: 'dict.ViolationNormativeDocItemGji',
            modelName: 'dict.ViolationNormativeDocItemGji',
            gridSelector: 'violationNormativeDocItemGrid'
        },
        {
            // Аспект взаимодействия таблицы справочника Нарушений и формы редактирования
            xtype: 'grideditwindowaspect',
            name: 'violationGjiGridWindowAspect',
            gridSelector: 'violationGjiGrid',
            editFormSelector: '#violationGjiEditWindow',
            storeName: 'dict.ViolationGji',
            modelName: 'dict.ViolationGji',
            editWindowView: 'dict.violationgji.EditWindow',
            onSaveSuccess: function (asp, record) {
                var me = this;
                var form = me.getForm();
                var grid = form.down('#violationGroupsGjiGrid');
                var store = grid.getStore();
                var featureIds = new Array();
                for (var index = 0; index < store.data.items.length; index++) {
                    if (store.data.items[index].data.IsChecked) {
                        featureIds.push(store.data.items[index].data.Id);
                    }
                }

                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('SaveViolationGroups', 'ViolationFeatureGji', {
                    groupIds: featureIds,
                    violationId: asp.controller.violationId
                })).next(function () {
                    asp.controller.unmask();
                    asp.controller.getStore(asp.storeName).load();
                    return true;
                }).error(function () {
                    asp.controller.unmask();
                });

                var docItemsGrid = form.down('#violationNormativeDocItemGrid');
                var docItemsStore = docItemsGrid.getStore();
                var dataItems = new Array();
                for (var index = 0; index < docItemsStore.data.items.length; index++) {
                    dataItems.push({
                        NormDocItemId: docItemsStore.data.items[index].data.NormativeDocItem.Id,
                        ViolStruct: docItemsStore.data.items[index].data.ViolationStructure
                    });
                }

                B4.Ajax.request(B4.Url.action('SaveNormativeDocItems', 'ViolationNormativeDocItemGji',
                {
                    items: Ext.encode(dataItems),
                    violationId: asp.controller.violationId
                })).next(function () {
                    var ndiStore = asp.controller.getStore('dict.ViolationNormativeDocItemGji');
                    ndiStore.removeAll();
                    ndiStore.load();
                });

                asp.controller.setViolationId(record.getId());
            },
            saveRecordHasUpload: function (rec) {
                var me = this;
                var frm = me.getForm();
                me.mask('Сохранение', frm);
                frm.submit({
                    url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                    params: {
                        records: Ext.encode([rec.getData()])
                    },
                    success: function (form, action) {
                        me.unmask();
                        me.updateGrid();

                        var model = me.getModel(rec);
                        var data = action.result.data;

                        if (data.length > 0) {
                            var id = data[0] instanceof Object ? data[0].Id : data[0];
                            model.load(id, {
                                success: function (newRec) {
                                    me.fireEvent('savesuccess', me, newRec);
                                    me.setFormData(newRec);
                                }
                            });
                        }
                    },
                    failure: function (form, action) {
                        me.unmask();
                        me.fireEvent('savefailure', rec, action.result.message);
                        Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                    }
                });
            },
            saveRecordHasNotUpload: function (rec) {
                var me = this;
                var frm = me.getForm();
                me.mask('Сохранение', frm);
                rec.save({ id: rec.getId() })
                    .next(function (result) {
                        me.unmask();
                        me.updateGrid();
                        var model = me.getModel();
                        if (result.responseData && result.responseData.data) {
                            var data = result.responseData.data;
                            if (data.length > 0) {
                                var id = data[0] instanceof Object ? data[0].Id : data[0];
                                model.load(id, {
                                    success: function (newRec) {
                                        me.fireEvent('savesuccess', me, newRec);
                                        me.setFormData(newRec);
                                    }
                                });
                            } else {
                                me.fireEvent('savesuccess', me);
                            }
                        } else {
                            me.fireEvent('savesuccess', me);
                        }
                    }, this)
                    .error(function (result) {
                        me.unmask();
                        me.fireEvent('savefailure', result.record, result.responseData);

                        Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    }, this);
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.setViolationId(record.getId());

                    var me = this;
                    var form = me.getForm();
                    var id = record.data.NormativeDocItem;
                    asp.controller.getStore('B4.store.dict.NormativeDocItem').load({
                        id: id,
                        scope: this,
                        callback: function (records, operation, success) {
                            if (success) {
                                var item;
                                for (var index = 0; index < records.length; index++) {
                                    if (records[index].data.Id == id) {
                                        item = records[index];
                                        break;
                                    }
                                }

                                if (item) {
                                    form.down('#normativeDoc').setValue(item.data.NormativeDocName);
                                }
                            }
                        }
                    });

                    asp.controller.getStore('dict.ViolationFeatureGroupsGji').load();
                }
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            fieldSelector: '[name="RuleOfLaw"]',
            editPanelAspectName: 'violationGjiGridWindowAspect',
            controllerName: 'ViolationGji',
            valueFieldName: 'RuleOfLaw',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: 'RuleOfLaw'
        },
        {
            // Аспект взаимодействия таблицы справочника Нарушений и справочника Характеристики нарушений
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'violationFeatureGjiAspect',
            gridSelector: '#violationFeatureGjiGrid',
            storeName: 'dict.ViolationFeatureGji',
            modelName: 'dict.ViolationFeatureGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#violationFeatureMultiSelectWindow',
            storeSelect: 'dict.FeatureViolGjiForSelect',
            storeSelected: 'dict.FeatureViolGjiForSelected',
            titleSelectWindow: 'Выбор характеристик нарушения',
            titleGridSelect: 'Характеристики нарушения для отбора',
            titleGridSelected: 'Выбранные характеристики нарушений',
            columnsGridSelect: [
                    { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                    { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                    { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddFeaturesViolation', 'ViolationFeatureGji', {
                            featuresIds: recordIds,
                            violationId: asp.controller.violationId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать характеристики');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            // Аспект взаимодействия таблицы справочника Нарушений и справочника Мероприятия по устранению нарушений
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'violationActionsRemovGjiAspect',
            gridSelector: '#violationActionsRemovGrid',
            storeName: 'dict.ViolationActionsRemovGji',
            modelName: 'dict.ViolationActionsRemovGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#violationActionsRemovMultiSelectWindow',
            storeSelect: 'dict.ActionsRemovViolForSelect',
            storeSelected: 'dict.ActionsRemovViolForSelected',
            titleSelectWindow: 'Выбор мероприятий по устранению нарушений',
            titleGridSelect: 'Мероприятия по устранению нарушений для отбора',
            titleGridSelected: 'Выбранные мероприятия',
            columnsGridSelect: [
                    { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                    { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                    { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddViolationActionsRemov', 'ViolationActionsRemovGji', {
                            actRemoveViolIds: recordIds,
                            violationId: asp.controller.violationId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать мероприятия');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('dict.ViolationFeatureGji').on('beforeload', me.onBeforeLoad, me);
        me.getStore('dict.ViolationActionsRemovGji').on('beforeload', me.onBeforeLoad, me);
        me.getStore('dict.ViolationFeatureGroupsGji').on('beforeload', me.onBeforeLoad, me);
        me.getStore('dict.ViolationNormativeDocItemGji').on('beforeload', me.onBeforeLoad, me);
        me.getStore('dict.NormativeDocItemTreeStore').on('beforeload', me.onBeforeLoad, me);
        me.getStore('dict.NormativeDocItemGrouping').load();

        me.control({
            "#normativeDocItemSelectField": {
                change: this.onItemChange
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('violationGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ViolationGji').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.violationId = this.violationId;
    },

    setViolationId: function (id) {
        this.violationId = id;

        var storeViolFeatures = this.getStore('dict.ViolationFeatureGji');
        var storeViolActRemove = this.getStore('dict.ViolationActionsRemovGji');
        var storeNormativeDocItems = this.getStore('dict.ViolationNormativeDocItemGji');
        storeViolActRemove.removeAll();
        storeViolFeatures.removeAll();
        storeNormativeDocItems.removeAll();

        var editWindow = Ext.ComponentQuery.query(this.violationGjiEditWindowSelector)[0];

        if (id > 0) {
            editWindow.down('violationGroupsGjiGrid').setDisabled(false);
            editWindow.down('violationActionsRemovGrid').setDisabled(false);
            storeViolActRemove.load();
            storeViolFeatures.load();
            storeNormativeDocItems.load();
        } else {
            editWindow.down('violationGroupsGjiGrid').setDisabled(true);
            editWindow.down('violationActionsRemovGrid').setDisabled(true);
        }
    },
    onItemChange: function (field, newValue) {
        var me = this;

        if (!newValue) {
            field.dataSelected = null;
        }

        if (!field.dataSelected) {
            return;
        }

        var data = field.dataSelected;

        var grid = Ext.ComponentQuery.query('#violationNormativeDocItemGrid')[0];
        var row = grid.getSelectionModel().getSelection()[0];
        me.getStore('dict.NormativeDocItem').load(
        {
            callback: function (records, operation, success) {
                if (!success) {
                    return;
                }

                for (var index2 = 0; index2 < data.length; index2++) {
                    for (var index = 0; index < records.length; index++) {
                        var item = records[index];
                        if (item.data.Id != data[index2].data.id) {
                            continue;
                        }

                        else {

                            row.set('NormativeDoc', item.data.NormativeDoc);
                            row.set('NormativeDocItemName', item.data.Number);
                            row.set('NormativeDocName', item.data.NormativeDocName);
                            row.set('Id', item.data.Id);
                            row.set('NormativeDocItem', { Id: item.data.Id });
                            row.setId(item.data.Id);
                            break;
                        }
                    }
                }
            }
        });
    }
});