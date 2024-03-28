Ext.define('B4.view.volumediscrepancy.MainView', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.volumediscrepancymainview',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.SelectField',
        'B4.ux.grid.column.Enum',
        'B4.form.MonthPicker',
        'B4.form.ComboBox'
    ],

    title: 'Расхождение объемов',
    closable: true,
    layout: 'anchor',

    initComponent: function () {
        var me = this,
            curDate = new Date(),
            discrepancyStore = Ext.create('B4.store.volumediscrepancy.DiscrepancyList');
        
        Ext.apply(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Фильтры',
                    padding: 2,
                    layout: 'anchor',
                    margin: 5,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0 5 5 5',
                                labelWidth: 70,
                                width: 300,
                                labelAlign: 'right',
                                emptyItem: { Name: 'Все' },
                                queryMode: 'local',
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
                            items: [
                                {
                                    xtype: 'b4monthpicker',
                                    name: 'Period',
                                    fieldLabel: 'Период',
                                    value: Ext.Date.add(curDate, Ext.Date.MONTH, -1)
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Услуга',
                                    //store: Ext.create('B4.store.volumediscrepancy.Service'),
                                    store: Ext.create('B4.store.volumediscrepancy.ServiceGroup'),
                                    name: 'Service'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Поставщик',
                                    store: Ext.create('B4.store.volumediscrepancy.Supplier'),
                                    name: 'Supplier'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'ShowNullValues',
                                    margin: '2 0 0 10',
                                    boxLabel: 'Отображать нулевые значения',
                                    labelWidth: 200
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0 5 5 5',
                                labelWidth: 70,
                                width: 300,
                                labelAlign: 'right',
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
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'МР',
                                    store: Ext.create('B4.store.volumediscrepancy.MunicipalArea'),
                                    name: 'MunicipalArea'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Нас. пункт',
                                    store: Ext.create('B4.store.volumediscrepancy.Settlement'),
                                    disabled: true,
                                    name: 'Settlement'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Улица',
                                    store: Ext.create('B4.store.volumediscrepancy.Street'),
                                    disabled: true,
                                    selectionMode: 'MULTI',
                                    name: 'Street'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'gridpanel',
                    name: 'Discrepancy',
                    margin: 5,
                    anchor: '0 -80',
                    columnLines: true,
                    selModel: Ext.create('Ext.selection.CheckboxModel'),
                    store: discrepancyStore,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'HouseAddress',
                            flex: 2,
                            text: 'Адрес',
                            filter: {
                                xtype: 'textfield',
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ServiceName',
                            flex: 1,
                            text: 'Услуга',
                            filter: {
                                xtype: 'textfield'
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'UOData',
                            flex: 1,
                            text: 'Данные УО',
                            renderer: me.numberRenderer
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'RSOData',
                            flex: 1,
                            text: 'Данные РСО',
                            renderer: me.numberRenderer
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Discrepancy',
                            flex: 1,
                            text: 'Расхождение',
                            tdCls: 'x-change-cell',
                            renderer: me.numberRenderer
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'IsPublished',
                            text: 'Статус',
                            flex: 1,
                            renderer: function (value) {
                                if (value) {
                                    return 'Опубликовано';
                                }
                                return 'Не опубликовано';
                            },
                            filter: {
                                xtype: 'combobox',
                                operand: CondExpr.operands.eq,
                                displayField: 'Name',
                                valueField: 'Value',
                                editable: false,
                                store: Ext.create('Ext.data.Store', {
                                    fields: [
                                        'Name',
                                        'Value'
                                    ],
                                    data: [
                                        {
                                            Name: 'Все'
                                        },
                                        {
                                            Name: 'Не опубликовано',
                                            Value: false
                                        },
                                        {
                                            Name: 'Опубликовано',
                                            Value: true
                                        }
                                    ]
                                })
                            }
                        }
                    ],
                    plugins: [
                        Ext.create('B4.ux.grid.plugin.HeaderFilters')
                    ],
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: discrepancyStore,
                            dock: 'bottom'
                        }
                    ],
                    viewConfig: {
                        getRowClass: function (record) {
                            if (record.get('Discrepancy') >= 0) {
                                return 'font-green-change-cell';
                            } else {
                                return 'font-tomato-change-cell';
                            }
                        },
                        enableTextSelection: true
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'button',
                                    name: 'Refresh',
                                    iconCls: 'icon-arrow-refresh',
                                    text: 'Обновить'
                                },
                                {
                                    xtype: 'button',
                                    name: 'Show',
                                    iconCls: 'icon-report',
                                    text: 'Показать'
                                },
                                {
                                    xtype: 'button',
                                    name: 'Publish',
                                    iconCls: 'icon-accept',
                                    text: 'Опубликовать'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    numberRenderer: function (value) {
        return value.toFixed(2);
    }
});