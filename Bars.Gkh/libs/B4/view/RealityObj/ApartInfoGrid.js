Ext.define('B4.view.realityobj.ApartInfoGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realityobjapartinfogrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhIntField',
        'B4.form.ComboBox',
        
        'B4.enums.YesNoNotSet'
    ],

    title: 'Сведения о квартирах',
    store: 'realityobj.ApartInfo',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumApartment',
                    flex: 1,
                    text: '№ квартиры',
                    editor: {
                        type: 'textfield',
                        maxLength: 300,
                        minValue: 0
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaTotal',
                    flex: 1,
                    decimalSeparator: ',',
                    text: 'Общая площадь',
                    allowNegative: false,
                    editor: {
                        xtype: 'gkhdecimalfield',
                        allowNegative: false,
                        minValue: 0
                    },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        allowNegative: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaLiving',
                    flex: 1,
                    decimalSeparator: ',',
                    text: 'Жилая площадь',
                    allowNegative: false,
                    editor: {
                        xtype: 'gkhdecimalfield',
                        allowNegative: false,
                        minValue: 0
                    },
                    filter: {
                        xtype: 'gkhdecimalfield',
                        allowNegative: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountPeople',
                    flex: 1,
                    text: 'Количество жителей',
                    editor: {
                        xtype: 'gkhintfield',
                        allowNegative: false,
                        operand: CondExpr.operands.eq,
                        minValue: 0
                    },
                    filter: {
                        xtype: 'gkhintfield',
                        allowNegative: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Privatized',
                    flex: 1,
                    text: 'Приватизировано',
                    renderer: function (val) {
                        return B4.enums.YesNoNotSet.displayRenderer(val);
                    },
                    editor: {
                        xtype: 'b4combobox',
                        editable: false,
                        store: B4.enums.YesNoNotSet.getStore(),
                        displayField: 'Display',
                        valueField: 'Value'
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.YesNoNotSet.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FioOwner',
                    flex: 1,
                    text: 'ФИО собственника',
                    editor: {
                        type: 'textfield',
                        maxLength: 500
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Phone',
                    flex: 1,
                    text: 'Телефон',
                    editor: {
                        type: 'textfield',
                        maxLength: 300
                    },
                    filter: { xtype: 'textfield' }
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
                            columns: 3,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' },
                                { xtype: 'b4savebutton' }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});