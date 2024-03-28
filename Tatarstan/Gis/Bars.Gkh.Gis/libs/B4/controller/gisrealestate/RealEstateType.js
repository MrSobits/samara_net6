Ext.define('B4.controller.gisrealestate.RealEstateType', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.aspects.InlineGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['gisrealestate.realestatetype.RealEstateType',
        'gisrealestate.realestatetype.RealEstateTypeCommonParam'],
    stores: ['gisrealestate.realestatetype.RealEstateType'],

    views: [
        'gisrealestatetype.Grid',
        'gisrealestatetype.EditPanel'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'gisrealestatetypegrid'
        },
        {
            ref: 'editPanel',
            selector: 'gisrealestatetypeeditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'commonParamGridAspect',
            modelName: 'gisrealestate.realestatetype.RealEstateTypeCommonParam',
            gridSelector: 'giscommonparameditgrid',
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                beforesave: function (asp, store) {
                    var result = null;

                    var commandParamStore = asp.getGrid().commonParamStore;
                    Ext.each(store.data.items, function (item) {
                        result = asp.validateCommonParams(item, commandParamStore);
                        if (!result.success) {
                            return false;
                        }

                        return true;
                    });

                    if (!result.success) {
                        B4.QuickMsg.msg('Ошибка', result.message, 'error');
                        return false;
                    }

                    return true;
                }
            },

            validateCommonParams: function (item, commandParamStore) {
                var paramCode = item.get('CommonParamCode'),
                    message = '',
                    isPrecision,
                    result = {},
                    min = item.get('Min'),
                    max = item.get('Max');
                
                result.success = true;
                result.message = message;

                if (Ext.isEmpty(paramCode)) {
                    return result;
                }

                isPrecision = commandParamStore.findRecord('Code', paramCode).get('IsPrecision');
                if (isPrecision) {
                    result.success = !Ext.isEmpty(item.get('PrecisionValue'));
                    if (!result.success) {
                        result.message = 'Необходимо заполнить поле "Точное значение"';
                    }
                } else {
                    if (!Ext.isEmpty(min) && !Ext.isEmpty(max)) {
                        var dateRegEx = /(^\d{1,4}[\.|\\/|-]\d{1,2}[\.|\\/|-]\d{1,4})(\s*(?:0?[1-9]:[0-5]|1(?=[012])\d:[0-5])\d\s*[ap]m)?$/;
                        if (dateRegEx.test(min)) {
                            result.success = new Date(min) < new Date(max);
                        } else if (typeof min != 'string') {
                            result.success = min <= max;
                        }

                        if (!result.success) {
                            result.message = 'Максимальное значение должно быть не меньше минимального';
                        }

                        return result;
                    }

                    result.success = (!Ext.isEmpty(min) && !Ext.isEmpty(max))
                        || !Ext.isEmpty(item.get('PrecisionValue'));
                    if (!result.success) {
                        result.message = 'Необходимо заполнить либо оба поля диапазона значений, либо поле "Точное значение"';
                    }
                }

                return result;
            },

            //addRecord: function() {
            //    var plugin,
            //        store = this.getGrid().getStore(),
            //        rec = this.controller.getModel(this.modelName).create(),
            //        grid = this.getGrid();

            //    store.insert(0, rec);

            //    if (this.cellEditPluginId && grid) {
            //        plugin = grid.getPlugin(this.cellEditPluginId);
            //        plugin.startEditByPosition({ row: 0, column: this.firstEditColumnIndex });
            //    }
            //},

            deleteRecord: function (record) {
                var me = this,
                    store = me.getGrid().getStore();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                    if (result == 'yes') {
                        store.remove(record);
                    }
                }, me);
            },

            save: function () {
                var me = this,
                    controller = me.controller,
                    view = controller.getEditPanel(),
                    realEstateTypeId = view.realEstateTypeId,
                    store = me.getGrid().getStore(),
                    modifiedRecs = store.getModifiedRecords(),
                    removedRecs = store.getRemovedRecords();

                if (modifiedRecs.length > 0 || removedRecs.length > 0) {
                    if (modifiedRecs.length > 0) {
                        Ext.each(store.data.items, function (item) {
                            // так как данные универсальны и хранятся в строке то необходимо преобразовывать в правильный строковый формат все даты
                            var min = item.get('Min'),
                                max = item.get('Max'),
                                precisionValue = item.get('PrecisionValue');
                            if (Ext.isDate(min)) {
                                item.set('Min', Ext.util.Format.date(min, 'm/d/Y'));
                            }
                            if (Ext.isDate(max)) {
                                item.set('Max', Ext.util.Format.date(max, 'm/d/Y'));
                            }
                            if (Ext.isDate(precisionValue)) {
                                item.set('PrecisionValue', Ext.util.Format.date(precisionValue, 'm/d/Y'));
                            }
                            item.set('RealEstateType', realEstateTypeId);

                            return true;
                        });
                    }

                    if (me.fireEvent('beforesave', me, store) !== false) {
                        me.mask('Сохранение', me.getGrid());
                        store.sync({
                            callback: function () {
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
            name: 'indicatorGridAspect',
            modelName: 'gisrealestate.realestatetype.RealEstateTypeIndicator',
            gridSelector: 'gisrealestateindicatorgrid',
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                beforesave: function (asp, store) {
                    var result = true;

                    Ext.each(store.data.items, function (item) {
                        return result = asp.validateCommonParams(item);
                    });

                    if (!result) {
                        B4.QuickMsg.msg('Ошибка', 'Должно быть заполнено либо поле "Точное значение", либо диапозон значений', 'error');
                        return false;
                    }

                    return result;
                }
            },

            validateCommonParams: function (item) {
                return (!Ext.isEmpty(item.get('Min')) && !Ext.isEmpty(item.get('Max'))) || !Ext.isEmpty(item.get('PrecisionValue'));
            },

            addRecord: function () {
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

            deleteRecord: function (record) {
                var me = this,
                    store = me.getGrid().getStore();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                    if (result == 'yes') {
                        store.remove(record);
                    }
                }, me);
            },

            save: function () {
                var me = this,
                    store = me.getGrid().getStore(),
                    modifiedRecs = store.getModifiedRecords(),
                    removedRecs = store.getRemovedRecords();

                if (modifiedRecs.length > 0 || removedRecs.length > 0) {
                    if (me.fireEvent('beforesave', me, store) !== false) {
                        me.mask('Сохранение', me.getGrid());
                        store.sync({
                            callback: function () {
                                me.unmask();
                                store.load();
                            },
                            failure: me.handleDataSyncError,
                            scope: me
                        });
                    }
                }
            }
        }
    ],

    mainView: 'gisrealestatetype.Grid',
    mainViewSelector: 'gisrealestatetypegrid',

    init: function () {
        var me = this;

        me.control(
            {
                'gisrealestatetypegrid': {
                    rowaction: { fn: me.onRowAction, scope: me },
                    actionClick: { fn: me.actionClick, scope: me }
                },
                'gisrealestatetypegrid button[name=AddGroup]': {
                    click: me.addGroupToGrid,
                    scope: me
                },
                'gisrealestatetypegrid button[name=AddType]': {
                    click: me.addType,
                    scope: me
                },
                'gisrealestatetypegrid b4updatebutton': {
                    click: me.onMainPanelUpdate,
                    scope: me
                },
                'gisrealestatetypeeditpanel': {
                    render: { fn: me.onRenderEditPanel, scope: me },
                    onAddGroup: { fn: me.addGroupToBase, scope: me }
                },
                'gisrealestatetypeeditpanel form b4savebutton': {
                    click: me.onEditPanelSave,
                    scope: me
                },
                'gisrealestatetypeeditpanel giscommonparameditgrid': {
                    render: me.onRenderCommonParamGrid,
                    scope: me
                },
                'gisrealestatetypeeditpanel gisrealestateindicatorgrid b4savebutton': {
                    click: me.onEditPanelIndicatorSave,
                    scope: me
                },
                'gisrealestatetypeeditpanel button[name=AddGroup]': {
                    click: function (button) {
                        Ext.create('B4.view.gisrealestatetype.TypeGroupEditPanel', { editPanel: button.up('gisrealestatetypeeditpanel') }).show();
                    }
                }
            }
        );

        this.callParent(arguments);
    },

    onRenderCommonParamGrid: function (grid) {
        var me = this;

        grid.getStore().on('load', function (store, records) {
            var model = me.getModel('gisrealestate.realestatetype.RealEstateTypeCommonParam'),
                fields = model.prototype.fields.getRange();

            Ext.each(records, function (record) {
                //fields.push({ name: record.get('name'), type: 'bool' });
            });
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('gisrealestatetypegrid'),
            store = view.getStore();

        this.bindContext(view);
        this.application.deployView(view);

        store.on({
            beforeload: function () {
                view.getEl().mask('Загрузка...');
            },
            load: function () {
                view.getEl().unmask();
            }
        });
    },

    actionClick: function (panel, record) {
        var me = this,
            id = record.get('EntityId'),
            name = record.get('Name'),
            entity = record.get('Entity');

        //Если действие с новой группой
        if (record.get('Id') == 'new') {
            if (!name) {
                B4.QuickMsg.msg('Внимание', 'Заполните наименование группы', 'warning');
                return;
            }

            var newRecords = [{
                Name: name
            }];

            panel.getEl().mask('Сохранение...');
            B4.Ajax.request({
                url: 'GisRealEstateTypeGroup/Create',
                params: {
                    records: Ext.encode(newRecords)
                }
            }).next(function () {
                panel.getEl().unmask();
                panel.getStore().load();
            });
        } else {
            //Если действие с типом дома
            if (entity == 'RealEstateType') {
                me.editRealEstateType(record);
            } else { //Действие с существующей группой
                //Если у группы доступен редактор, то при нажатии на действие, обновляем группу
                if (record.get('allowEditor')) {
                    var records = [{
                        Id: id,
                        Name: name
                    }];

                    panel.getEl().mask('Сохранение...');
                    B4.Ajax.request({
                        url: 'GisRealEstateTypeGroup/Update',
                        params: {
                            records: Ext.encode(records)
                        }
                    }).next(function () {
                        panel.getEl().unmask();
                        panel.getStore().load();
                    });
                } else {
                    //Иначе включаем редактор

                    //Проверка на наличие уже включенных редакторов
                    if (panel.getStore().getRootNode().findChildBy(function (node) {
                        return node.get('allowEditor');
                    })) {
                        B4.QuickMsg.msg('Внимание', 'Сохраните предыдущую группу', 'warning');
                        return;
                    }

                    record.set('allowEditor', true);
                    panel.getView().refresh();
                    panel.editingPlugin.startEdit(record, 2);
                }
            }
        }
    },

    onRowAction: function (grid, action, record) {
        var me = this,
            entity = 'Gis' + record.get('Entity'),
            id = record.get('EntityId');

        if (record.get('Id') === 'new') {
            B4.QuickMsg.msg('Внимание', 'Группа еще не добавлена', 'warning');
            return;
        }

        if (entity === 'GisRealEstateTypeGroup') {
            if (record.get('children') && record.get('children').length > 0) {
                B4.QuickMsg.msg('Внимание', 'Удаление группы невозможно, так как в ней содержатся типы домов', 'warning');
                return;
            }
        }

        if (action.toLowerCase() === 'delete') {
            Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                if (result === 'yes') {

                    me.mask('Удаление', B4.getBody());
                    B4.Ajax.request({
                        url: B4.Url.action('Delete', entity),
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
        }
    },

    //Добавить группу
    addGroupToGrid: function (button) {
        var panel = button.up('treepanel'),
            store = panel.getStore(),
            root = store.getRootNode();

        //Проверка на наличие уже добаленных групп
        if (store.getRootNode().findChildBy(function (node) {
            return node.get('Id') == 'new';
        })) {
            B4.QuickMsg.msg('Внимание', 'Сохраните предыдущую группу', 'warning');
            return;
        }

        var newRecord = root.insertChild(0, {
            Id: 'new',
            leaf: true,
            allowEditor: true
        });

        panel.editingPlugin.startEdit(newRecord, 2);
    },

    addType: function () {
        Ext.History.add('gisrealestatetype/0');
    },

    addGroupToBase: function (editPanel, editWin, name) {
        var selectField = editPanel.down('b4selectfield[name=Group]');
        editWin.close();

        var newRecords = [{
            Name: name
        }];

        editPanel.getEl().mask('Сохранение...');
        B4.Ajax.request({
            url: 'GisRealEstateTypeGroup/Create',
            params: {
                records: Ext.encode(newRecords)
            }
        }).next(function (result) {
            var response = Ext.decode(result.responseText);
            editPanel.getEl().unmask();
            selectField.getStore().on({
                load: function () {
                    selectField.setValue(response.data[0]);
                }
            });
            selectField.getStore().load();
        });
    },

    onMainPanelUpdate: function (btn) {
        btn.up('gisrealestatetypegrid').getStore().load();
    },

    onRenderEditPanel: function (panel) {
        var me = this,
            id = +panel.realEstateTypeId,
            form = panel.down('form'),
            tabpanel = panel.down('tabpanel'),
            commonParamStore = panel.down('giscommonparameditgrid').getStore(),
            indicatorStore = panel.down('gisrealestateindicatorgrid').getStore(),
            estateObj,
            realEstateType;

        tabpanel.disable();

        commonParamStore.on('beforeload', me.onBeforeLoadStore, me);
        indicatorStore.on('beforeload', me.onBeforeLoadStore, me);

        commonParamStore.load();
        indicatorStore.load();

        if (!id) {
            estateObj = Ext.create('B4.model.gisrealestate.realestatetype.RealEstateType');
            form.loadRecord(estateObj);
        } else {
            realEstateType = Ext.ModelMgr.getModel('B4.model.gisrealestate.realestatetype.RealEstateType');
            realEstateType.load(id, {
                success: function (obj) {
                    form.loadRecord(obj);
                    tabpanel.setDisabled(false);
                }
            });
        }
    },

    onEditPanelSave: function (btn) {
        var me = this,
            editPanel = btn.up('gisrealestatetypeeditpanel'),
            form = editPanel.down('form'),
            record = form.getRecord(),
            values = form.getValues(),
            isCreating = !record.getId();

        if (!values || !values.Name) {
            B4.QuickMsg.msg('Ошибка', 'Значение поля "Наименование" необходимо заполнить.', 'failure');
        } else {
            editPanel.getEl().mask('Сохранение...');
            record.set(form.getValues());
            record.save({
                success: function (rec) {
                    editPanel.getEl().unmask();
                    if (isCreating) {
                        editPanel.close();
                        me.application.redirectTo(Ext.String.format('gisrealestatetype/{0}', rec.getId()));
                    }
                }
            });
        }
    },

    onEditPanelIndicatorSave: function (btn) {
        var editPanel = btn.up('gisrealestatetypeeditpanel'),
            indicatorGrid = editPanel.down('gisrealestateindicatorgrid'),
            store = indicatorGrid.getStore();

        store.each(function (rec) {
            rec.set('RealEstateType', editPanel.realEstateTypeId);
        });
    },

    edit: function (id) {
        var me = this,
            view = this.getEditPanel() || Ext.widget('gisrealestatetypeeditpanel', {
                realEstateTypeId: id
            });

        me.setContextValue(view, 'realEstateTypeId', id);

        me.bindContext(view);
        me.application.deployView(view);
    },

    onBeforeLoadStore: function (store, operation) {
        operation.params['RealEstateTypeId'] = this.getEditPanel().realEstateTypeId;
    },

    editRealEstateType: function (record) {
        Ext.History.add('gisrealestatetype/' + record.get('EntityId'));
    }
});