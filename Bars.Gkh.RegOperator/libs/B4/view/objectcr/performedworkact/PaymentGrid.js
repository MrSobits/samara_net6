Ext.define('B4.view.objectcr.performedworkact.PaymentGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.perfworkactpaymentgrid',
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
        'B4.view.Control.GkhDecimalField',
        'B4.enums.ActPaymentType'
    ],

    store: 'objectcr.performedworkact.Payment',

    initComponent: function () {
        var me = this;

        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePayment',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата оплаты'
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeActPayment',
                    flex: 1,
                    text: 'Вид оплаты',
                    enumName: 'B4.enums.ActPaymentType'
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма по распоряжению, руб',
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
                    text: 'Оплачено, руб.',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'Document',
                    width: 100,
                    text: 'Документ',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
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
                                {
                                    xtype: 'b4addbutton',
                                    text: 'Сформировать распоряжение'
                                },
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    action: 'export',
                                    iconCls: 'icon-table-go',
                                    text: 'Выгрузка в 1С'
                                }/*,
                                {
                                    xtype: 'button',
                                    action: 'savePayments',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                }*/
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