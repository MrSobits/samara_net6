Ext.define('B4.view.objectcr.performedworkact.PaymentGrid', {
    extend: 'B4.ux.grid.Panel',
    
    requires: [
        'B4.grid.feature.Summary',
        
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.view.Control.GkhDecimalField',
        'B4.enums.ActPaymentType'
    ],

    alias: 'widget.perfworkactpaymentgrid',
    store: 'objectcr.performedworkact.Payment',

    initComponent: function () {
        var me = this;

        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePayment',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата оплаты',
                    editor: 'datefield' 
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeActPayment',
                    flex: 1,
                    text: 'Вид оплаты',
                    enumName: 'B4.enums.ActPaymentType',
                    editor: {
                        xtype: 'b4enumcombo',
                        enumName: 'B4.enums.ActPaymentType'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма, руб.',
                    editor: {
                        xtype: 'gkhdecimalfield',
                        minValue: 0.01
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return  Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    dataIndex: 'Paid',
                    flex: 1,
                    text: 'Сумма оплачено, руб',
                    editor: {
                        xtype: 'gkhdecimalfield',
                        minValue: 0.01
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return  Ext.util.Format.currency(val);
                    },
                    dataIndex: 'Percent',
                    flex: 1,
                    text: 'Процент оплаты',
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
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
            features: [{
                ftype: 'b4_summary'
            }],
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
                                {
                                    xtype: 'button',
                                    action: 'savePayments',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                }
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