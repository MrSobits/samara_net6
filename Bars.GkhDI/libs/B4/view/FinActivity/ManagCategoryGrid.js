Ext.define('B4.view.finactivity.ManagCategoryGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.finmanagcatgrid',
    store: 'finactivity.ManagCategory',
    itemId: 'finActivityManagCategoryGrid',
    title: 'Управление по категориям',

    requires: [
        'B4.view.Control.GkhDecimalField',
        
        'B4.enums.TypeCategoryHouseDi'
    ],

    initComponent: function() {
        var me = this;

        var renderer = function (val, meta, rec, index) {
            if (rec.get('IsInvalid').split(';')[index] == true) {
                meta.style = 'background: #f9bdbb;';
                meta.tdAttr = 'data-qtip="Введенное значение не совпадает с суммой по столбцу"';
            }
            return val;
        };

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeCategoryHouseDi',
                    flex: 1,
                    text: 'Категория дома',
                    renderer: function (val) { return B4.enums.TypeCategoryHouseDi.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomeManaging',
                    flex: 1,
                    text: 'Доход от управления (тыс.руб.)',
                    editor: 'gkhdecimalfield',
                    renderer: function (val, meta, rec) {
                        val = renderer(val, meta, rec, 0);
                        
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomeUsingGeneralProperty',
                    flex: 1,
                    text: 'Доход от использования общего имущества (тыс.руб.)',
                    editor: 'gkhdecimalfield',
                    renderer: function (val, meta, rec) {
                        val = renderer(val, meta, rec, 3);
                        
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExpenseManaging',
                    flex: 1,
                    text: 'Расходы на управление (тыс.руб.)',
                    editor: 'gkhdecimalfield',
                    renderer: function (val, meta, rec) {
                        val = renderer(val, meta, rec, 4);
                        
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExactPopulation',
                    flex: 1,
                    text: 'Оплачено населением (тыс.руб.)',
                    editor: 'gkhdecimalfield',
                    renderer: function (val, meta, rec) {
                        val = renderer(val, meta, rec, 5);
                        
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtPopulationStart',
                    flex: 1,
                    text: 'Задолженность населения на начало (тыс.руб.)',
                    editor: 'gkhdecimalfield',
                    renderer: function (val, meta, rec) {
                        val = renderer(val, meta, rec, 1);
                        
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtPopulationEnd',
                    flex: 1,
                    text: 'Задолженность населения на конец (тыс.руб.)',
                    editor: 'gkhdecimalfield',
                    renderer: function (val, meta, rec) {
                        val = renderer(val, meta, rec, 2);
                        
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'saveManagCategoryButton',
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept'
                                }
                            ]
                        }
                    ]
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});