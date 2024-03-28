Ext.define('B4.ux.config.LegalNumberEditor', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.legalnumbereditor',
    layout: 'anchor',
    requires: [
        'B4.enums.SymbolsLocation',
        'B4.form.ComboBox'
    ],
    editorGrid: null,
    editorStore: null,

    mixins: {
        field: 'Ext.form.field.Field'
    },

    initComponent: function() {
        var me = this;

        me.editorStore = Ext.create('Ext.data.Store', {
            autoDestroy: true,
            fields: ['Name', 'SymbolsCount', 'SymbolsLocation', 'Order']
        });

        me.rulesStore = Ext.create('Ext.data.Store', {
            autoDestroy: true,
            fields: ['Name', 'Description', 'SymbolsCountConfig', 'SymbolsLocationConfig', 'DefaultConfig', 'IsRequired'],
            proxy: {
                type: 'ajax',
                url: B4.Url.action('ListRulesName', 'PaymentDocumentNumber'),
                reader: {
                    type: 'json',
                    root: 'data',
                    idProperty: 'Name',
                    totalProperty: 'totalCount',
                    successProperty: 'success',
                    messageProperty: 'message'
                }
            }
        });

        me.numberExampleField = Ext.create('Ext.form.field.Text', {
            readOnly: true
        });

        me.editorGrid = Ext.create('Ext.grid.Panel', {
            firstEditColumnIndex: 0,
            border: true,
            columns: [
                {
                    header: 'Наименование',
                    flex: 1,
                    dataIndex: 'Name',
                    xtype: 'gridcolumn',
                    renderer: function(value, metaData, record, rowIndex, colIndex, store, view) {
                        var ruleIndex = me.rulesStore.find('Name', value),
                            rule = ruleIndex > -1 ? me.rulesStore.getAt(ruleIndex) : null;
                        return rule ? rule.get('Description') : value;
                    },
                    editor: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: true,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        displayField: 'Description',
                        emptyItem: { Name: '-' },
                        fields: ['Name', 'Description'],
                        url: '/PaymentDocumentNumber/ListRulesName'
                    }
                },
                {
                    header: 'Количество символов ',
                    flex: 1,
                    dataIndex: 'SymbolsCount',
                    xtype: 'gridcolumn',
                    renderer: function (value, metaData, record) {
                        var configRuleName = record.get('Name'),
                            ruleIndex = me.rulesStore.find('Name', configRuleName),
                            rule = ruleIndex > -1 ? me.rulesStore.getAt(ruleIndex) : null,
                            isAllSymbolLocation = record.get('SymbolsLocation') !== 10 || configRuleName === me.defaultRuleName;

                        return (rule && rule.get('SymbolsCountConfig').Visible && isAllSymbolLocation)
                            ? value
                            : '';
                    },
                    editor: {
                        xtype: 'numberfield',
                        allowDecimals: false,
                        operand: CondExpr.operands.eq,
                        minValue: 0
                    }
                },
                {
                    header: 'Расположения символов в значении',
                    flex: 1,
                    dataIndex: 'SymbolsLocation',
                    xtype: 'gridcolumn',
                    name: 'SymbolsLocation',
                    renderer: function (value, metaData, record) {
                        var configRuleName = record.get('Name'),
                            ruleIndex = me.rulesStore.find('Name', configRuleName),
                            rule = ruleIndex > -1 ? me.rulesStore.getAt(ruleIndex) : null;

                        return rule && rule.get('SymbolsLocationConfig').Visible
                            ? B4.enums.SymbolsLocation.displayRenderer(value)
                            : '';
                    },
                    editor: {
                        xtype: 'b4combobox',
                        items: B4.enums.SymbolsLocation.getItems(),
                        displayField: 'Display',
                        valueField: 'Value'
                    }
                },
                {
                    header: 'Порядок в номере',
                    flex: 1,
                    dataIndex: 'Order',
                    xtype: 'gridcolumn',
                    editor: {
                        xtype: 'numberfield',
                        allowDecimals: false,
                        operand: CondExpr.operands.eq,
                        minValue: 0
                    }
                }
            ],
            selModel: {
                selType: 'cellmodel'
            },
            store: me.editorStore,
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        beforeedit: function (editor, e) {
                            var configRuleName = e.record.get('Name'),
                                ruleIndex = me.rulesStore.find('Name', configRuleName),
                                rule = ruleIndex > -1 ? me.rulesStore.getAt(ruleIndex) : null,
                                isAllSymbolLocation = e.record.get('SymbolsLocation') !== 10 || configRuleName === me.defaultRuleName;
                            
                            if (e.field === 'SymbolsCount') {
                                return rule && rule.get('SymbolsCountConfig').Editable && isAllSymbolLocation;
                            }
                            else if (e.field === 'SymbolsLocation') {
                                return rule && rule.get('SymbolsLocationConfig').Editable;
                            }
                            else if (e.field === 'Name' && configRuleName === me.defaultRuleName) {
                                return me.countOfRequiredRules() > 1;
                            }
                            return true;
                        }
                    }
                })
            ],
            listeners: {
                'edit': {
                    fn: me.__setNewValue,
                    scope: me
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    handler: me.addRecord.bind(me)
                                }, {
                                    xtype: 'b4deletebutton',
                                    handler: me.doRemoveValue.bind(me)
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Пример номера (обновляется после сохранения)',
                    items: [
                        me.numberExampleField,
                        {
                            xtype: 'label',
                            text: 'Обновление параметров настройки на сервере расчётов произойдёт через пять минут.'
                        }
                    ],
                    defaults: {
                        labelWidth: 150,
                        anchor: '100%'
                    }
                },
                me.editorGrid
            ]
        });

        me.initField();
        me.callParent(arguments);
    },

    addRecord: function() {
        var me = this,
            grid = me.editorGrid,
            plugin,
            store = grid.getStore(),
            // по умолчанию заполняем значением Все значения
            rec = store.create({
                SymbolsLocation: 10,
                SymbolsCount: 0,
                Order: Ext.max(Ext.Array.pluck(Ext.Array.pluck(store.getRange(), 'data'), 'Order')) + 1
            });

        
        store.add(rec);

        me.__setNewValue();
    },

    doRemoveValue: function() {
        var me = this,
            sm = me.editorGrid.getSelectionModel(),
            record = null;
        if (sm.hasSelection()) {
            record = sm.getSelection()[0];
            if (me.countOfRequiredRules() > 1 || record.get('Name') !== me.defaultRuleName) {
                me.editorStore.remove(record);
                if (me.editorStore.getCount() > 0) {
                    sm.select(0);
                }
            }
        }

        me.__setNewValue();
    },

    __setNewValue: function() {
        var me = this,
            res = [];
        me.editorStore.each(function (rec) {
            res.push(rec.data);
        });

        me.changed = true;
        me.mixins.field.setValue.call(me, res);
    },

    setValue: function () {
        var me = this,
            args = arguments;
        me.rulesStore.load(function () {
            var ruleIndex = me.rulesStore.find('IsRequired', true),
                defaultRule = ruleIndex > -1 ? me.rulesStore.getAt(ruleIndex) : null;
            me.defaultRuleName = defaultRule.get('Name');
            me.mixins.field.setValue.apply(me, args);
            me.editorStore.removeAll();
            if (Ext.isEmpty(me.value)) {
                if (defaultRule) {
                    me.editorStore.add({
                        Name: me.defaultRuleName,
                        SymbolsCount: 0,
                        SymbolsLocation: 10,
                        Order: 1
                    });
                }
            } else {
                Ext.each(me.value, function (v) {
                    me.editorStore.add({
                        Name: v.Name,
                        SymbolsCount: v.SymbolsCount,
                        SymbolsLocation: v.SymbolsLocation,
                        Order: v.Order
                    });
                });
            }
        });

        Ext.Ajax.request({
            method: 'GET',
            url: B4.Url.action('/PaymentDocumentNumber/GetExample'),
            success: function (response) {
                //десериализуем полученную строку
                var obj = Ext.JSON.decode(response.responseText);

                if (obj.success) {
                    me.numberExampleField.setValue(obj.data);
                } 
            },
            failure: function (result) {
                Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            }
        });
    },

    isEqual: function () {
        var me = this;
        return !me.changed;
    },

    reset: function () {
        var me = this;

        if (me.editorStore) {
            me.editorStore.removeAll();
        }

        me.mixins.field.reset.apply(me, arguments);
        me.changed = false;
    },

    isValid: function () {
        var me = this,
            values = me.editorStore.getRange(),
            result = Ext.Array.each(values, function(value) { return !Ext.isEmpty(value.get('Name')); }) === true;

        return result;
    },

    countOfRequiredRules: function() {
        var me = this,
            requiredCount = 0;
        Ext.Array.each(me.editorStore.getRange(), function (value) {
            if (value.get('Name') === me.defaultRuleName) {
                requiredCount++;
            }
        });
        return requiredCount;
    },

    destroy: function () {
        var me = this;

        me.callParent();
        Ext.destroy(me.win);
        
        delete me.Id;
        delete me.Name;

        if (me.editorGrid) {
            delete me.editorGrid;
        }
        if (me.editorStore) {
            delete me.editorStore;
        }
        if (me.rulesStore) {
            delete me.rulesStore;
        }
        if (me.numberExampleField) {
            delete me.numberExampleField;
        }
    }
});