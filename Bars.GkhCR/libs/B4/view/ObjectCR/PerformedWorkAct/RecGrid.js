Ext.define('B4.view.objectcr.performedworkact.RecGrid', {
    extend: 'B4.ux.grid.Panel',
    
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.view.Control.GkhButtonImport',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.dict.UnitMeasure',
        'B4.view.Control.GkhDecimalField'
    ],

    order: false,
    title: 'Показатели актов выполненных работ',
    alias: 'widget.perfworkactrecgrid',
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.objectcr.performedworkact.Record');
        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    flex: 1,
                    text: 'Номер',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 250
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Reason',
                    flex: 2,
                    text: 'Обоснование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1000
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1000
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    flex: 1,
                    text: 'Ед. изм.',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    dataIndex: 'OnUnitCount',
                    flex: 1,
                    text: 'Кол. на ед.',
                    editor: 'gkhdecimalfield',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    dataIndex: 'TotalCount',
                    flex: 1,
                    text: 'Кол. всего',
                    editor: 'gkhdecimalfield',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    dataIndex: 'OnUnitCost',
                    flex: 1,
                    text: 'Стоимость на ед.',
                    editor: 'gkhdecimalfield',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    dataIndex: 'TotalCost',
                    flex: 1,
                    text: 'Общая стоимость',
                    editor: 'gkhdecimalfield',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    dataIndex: 'BaseSalary',
                    flex: 1,
                    text: 'Осн. з/п',
                    editor: 'gkhdecimalfield',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    dataIndex: 'MachineOperatingCost',
                    flex: 1,
                    text: 'Эк. маш.',
                    editor: 'gkhdecimalfield',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    dataIndex: 'MechanicSalary',
                    flex: 1,
                    text: 'З/п мех.',
                    editor: 'gkhdecimalfield',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    dataIndex: 'MaterialCost',
                    flex: 1,
                    text: 'Материалы',
                    editor: 'gkhdecimalfield',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    dataIndex: 'BaseWork',
                    flex: 1,
                    text: 'Т/з осн.раб.',
                    editor: 'gkhdecimalfield',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    dataIndex: 'MechanicWork',
                    flex: 1,
                    text: 'Т/з мех.',
                    editor: 'gkhdecimalfield',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'gkhbuttonimport' },
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    itemId: 'btnSaveRecs',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },
                                { 
                                    xtype: 'checkbox',
                                    actionName: 'cbShowEstimMatch',
                                    checked: true,
                                    boxLabel: 'Соответствует смете',
                                    labelWidth: 150
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});