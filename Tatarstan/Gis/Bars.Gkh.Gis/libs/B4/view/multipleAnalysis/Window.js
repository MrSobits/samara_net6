Ext.define('B4.view.multipleAnalysis.Window', {
    extend: 'Ext.window.Window',
    alias: 'widget.multipleAnalysisWindow',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.multipleAnalysis.Indicator',
        'B4.form.MonthPicker',
        'B4.enums.GisTypeCondition',
        'B4.QuickMsg',
        'B4.form.SelectField',
        'B4.store.regressionanalysis.HouseType',
        'B4.store.multipleAnalysis.MunicipalityFias',
        'B4.form.GisTreeSelectField'
    ],

    title: 'Шаблон',
    closable: true,
    constrain: true,
    modal: true,
    width: 800,
    height: 500,
    layout: 'anchor',
    maximizable: true,
    
    editedRecord: undefined,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.multipleAnalysis.Indicator');

        store.on('beforeload', function (st, operation) {
            operation.params.id = me.editedRecord ? me.editedRecord.get('Id') : 0;
        });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    height: 90,
                    frame: true,
                    anchor: '100%',
                    layout: 'anchor',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'gistreeselectfield',
                                    name: 'HouseType',                                            
                                    fieldLabel: 'Тип дома',
                                    width: 393,
                                    labelWidth: 60,
                                    titleWindow: 'Выбор типа дома',
                                    store: 'B4.store.regressionanalysis.HouseType',
                                    allowBlank: false,
                                    editable: false,
                                    idProperty: 'EntityId'
                                },
                                //{
                                //    xtype: 'combobox',
                                //    name: 'TypeCondition',
                                //    fieldLabel: 'Условие',
                                //    labelAlign: 'right',
                                //    editable: false,
                                //    store: B4.enums.GisTypeCondition.getStore(),
                                //    displayField: 'Display',
                                //    valueField: 'Value',
                                //    width: 140,
                                //    labelWidth: 70,
                                //    allowBlank: false,
                                //    queryMode: 'local'
                                //},
                                {
                                    xtype: 'textfield',
                                    name: 'EMail',
                                    //allowBlank: false,
                                    regex: /.+@.+\..+/i,
                                    regexText: 'Электронный адрес набран с ошибкой.',
                                    fieldLabel: 'Email',
                                    labelWidth: 70,
                                    labelAlign: 'right',
                                    flex: 1,
                                    maxWidth: 520
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '5 0 0 0',
                            layout: {
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'МР',
                                    store: Ext.create('B4.store.multipleAnalysis.MunicipalityFias'),
                                    name: 'MunicipalArea',
                                    width: 393,
                                    labelWidth: 60,
                                    labelAlign: 'left',
                                    editable: false,
                                    selectionMode: 'SINGLE',
                                    windowCfg: {
                                        modal: true
                                    },
                                    columns: [
                                        {
                                            text: 'Наименование',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        }
                                    ]
                                },
                                //{
                                //    xtype: 'combobox',
                                //    name: 'FormDay',
                                //    fieldLabel: 'День',
                                //    labelAlign: 'right',
                                //    editable: false,
                                //    displayField: 'Day',
                                //    valueField: 'Day',
                                //    width: 140,
                                //    labelWidth: 70,
                                //    //allowBlank: false,
                                //    queryMode: 'local'
                                //}
                                //{
                                //    xtype: 'b4selectfield',
                                //    fieldLabel: 'Нас. пункт',
                                //    store: Ext.create('B4.store.volumediscrepancy.Settlement'),
                                //    disabled: true,
                                //    name: 'Settlement',
                                //    width: 260,
                                //    labelWidth: 110,
                                //    labelAlign: 'right'
                                //},
                                //{
                                //    xtype: 'b4selectfield',
                                //    fieldLabel: 'Улица',
                                //    store: Ext.create('B4.store.volumediscrepancy.Street'),
                                //    disabled: true,
                                //    name: 'Street',
                                //    width: 250,
                                //    labelAlign: 'right'
                                //}
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '5 0 0 0',
                            layout: {
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'b4monthpicker',
                                    fieldLabel: 'Месяц',
                                    name: 'MonthYear',
                                    editable: false,
                                    width: 180,
                                    labelWidth: 60
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'TypeCondition',
                                    fieldLabel: 'Условие',
                                    labelAlign: 'right',
                                    editable: false,
                                    store: B4.enums.GisTypeCondition.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    width: 120,
                                    labelWidth: 65,
                                    allowBlank: false,
                                    queryMode: 'local'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сформировать',
                                    name: 'Form',
                                    margin: '0 0 0 5',
                                    width: 88
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    anchor: '100% -90',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'treepanel',
                            title: 'Индикаторы',
                            width: 400,
                            store: store,
                            rootVisible: false,
                            columnLines: true,
                            plugins: [
                                Ext.create('Ext.grid.plugin.CellEditing', {
                                    clicksToEdit: 1,
                                    pluginId: 'cellEditing',
                                    listeners: {
                                        edit: function (editor, e) {
                                            var min = e.record.get('MinValue'),
                                                max = e.record.get('MaxValue'),
                                                isMin = e.column.dataIndex === 'MinValue';

                                            if (Ext.isEmpty(min) || Ext.isEmpty(max) || min <= max) {
                                                e.record.commit();
                                                return;
                                            }
                                            e.record.set(e.column.dataIndex, isMin ? max : min);
                                            e.record.commit();
                                            B4.QuickMsg.msg('Внимание', 'Минимальное значение не должно превышать максимальное!', 'warning');
                                        }
                                    }
                                })
                            ],
                            columns: [
                                {
                                    xtype: 'treecolumn',
                                    dataIndex: 'Name',
                                    text: 'Наименование',
                                    menuDisable: true,
                                    flex: 1
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'MinValue',
                                    text: 'Min значение',
                                    menuDisable: true,
                                    getEditor: function (record) {
                                        return me.getEditor(record, this);
                                    },
                                    width: 50,
                                    renderer: me.renderValue
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'MaxValue',
                                    text: 'Max значение',
                                    menuDisable: true,
                                    getEditor: function (record) {
                                        return me.getEditor(record, this);
                                    },
                                    width: 50,
                                    renderer: me.renderValue
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'DeviationPercent',
                                    text: '% отклонения',
                                    menuDisable: true,
                                    getEditor: function (record) {
                                        return me.getEditor(record, this);
                                    },
                                    width: 50,
                                    renderer: me.renderValue
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ExactValue',
                                    text: 'Точное значение',
                                    menuDisable: true,
                                    getEditor: function (record) {
                                        return me.getEditor(record, this);
                                    },
                                    width: 50,
                                    renderer: me.renderValue
                                }
                            ]
                        },
                        {
                            xtype: 'grid',
                            title: 'Результат',
                            name: 'AnalysisResult',
                            columnLines: true,
                            flex: 1,
                            enableLocking: true,
                            columns: [],
                            disabled: true
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            defaults: {
                                margin: '2 0 2 0'
                            },
                            columns: 6,
                            items: [
                                { xtype: 'b4savebutton', text: 'Сохранить и закрыть' }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            defaults: {
                                margin: '2 0 2 0'
                            },
                            columns: 1,
                            items: [
                                { xtype: 'b4closebutton', handler: function () { this.up('window').close(); } }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    getEditor: function (record, column) {
        var me = this;

        if (!record.get('leaf')) {
            return null;
        }

        switch (column.dataIndex) {
            case 'MinValue':
            case 'MaxValue':
                if (record.get('DeviationPercent') || record.get('ExactValue')) {
                    me.confirmChange(record, column);
                    return null;
                }
                break;
            case 'DeviationPercent':
                if (record.get('MinValue') || record.get('MaxValue') || record.get('ExactValue')) {
                    me.confirmChange(record, column);
                    return null;
                }
                break;
            case 'ExactValue':
                if (record.get('MinValue') || record.get('MaxValue') || record.get('DeviationPercent')) {
                    me.confirmChange(record, column);
                    return null;
                }
                break;
        }

        return Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('Ext.form.field.Number', {
                hideTrigger: true,
                decimalPrecision: 4,
                listeners: {
                    change: function (value) {
                        return Ext.util.Format.number(value, '0,000.0000');
                    }
                }
            })
        });
    },

    renderValue: function (value, metaData, record) {
        if (!record.get('leaf')) {
            metaData.style = 'background: #CCCCCC';
            return null;
        }
        return value;
    },

    confirmChange: function (record, column) {
        var newField, oldField;

        if (record.get('DeviationPercent')) {
            oldField = '% отклонения';
        } else if (record.get('ExactValue')) {
            oldField = 'точное значение';
        } else {
            oldField = 'диапазон дат';
        }

        if (column.dataIndex == 'DeviationPercent') {
            newField = '% отклонения';
        } else if (column.dataIndex == 'ExactValue') {
            newField = 'точного значния';
        } else {
            newField = 'диапазона значений';
        }

        Ext.Msg.confirm('Внимание', Ext.String.format('Для заполнения {0} необходимо очистить {1}!<br />Очистить?', newField, oldField), function (confirmationResult) {
            if (confirmationResult != 'yes') {
                return;
            }
            column.up('treepanel').fireEvent('changeConfirmed', record, column);
        });
    }
});