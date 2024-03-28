Ext.define('B4.view.bankstatement.PaymentOrderGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.paymentordergrid',
    requires: [
        'B4.grid.feature.Summary',
        
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.grid.plugin.HeaderFilters',
        
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.ux.grid.filter.YesNo',
        'B4.view.Control.GkhButtonImport',
        
        'B4.enums.TypePaymentOrder',
        'B4.enums.TypeFinanceSource'
    ],

    store: 'bankstatement.PaymentOrder',
    
    features: [{
        ftype: 'b4_summary'
    }],
    
    initComponent: function() {
        var me = this;
        
        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                  xtype: 'b4editcolumn',
                  scope: me
                },
                {
                     xtype: 'gridcolumn',
                     dataIndex: 'TypePaymentOrder',
                     text: 'Тип',
                     width: 80,
                     renderer: function (val) { return B4.enums.TypePaymentOrder.displayRenderer(val); },
                     filter: {
                         xtype: 'b4combobox',
                         items: B4.enums.TypePaymentOrder.getItemsWithEmpty([null, '-']),
                         editable: false,
                         operand: CondExpr.operands.eq,
                         valueField: 'Value',
                         displayField: 'Display'
                     }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
                    text: 'Муниципальное образование',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealObjName',
                    text: 'Адрес',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    text: 'Номер',
                    width: 50,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 80,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    width: 130,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    align: 'right',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeFinanceSource',
                    text: 'Разрез финансирования',
                    flex: 1,
                    renderer: function (val) { return B4.enums.TypeFinanceSource.displayRenderer(val); },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeFinanceSource.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PayerContragentName',
                    text: 'Плательщик',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReceiverContragentName',
                    text: 'Получатель',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PayPurpose',
                    text: 'Назначение платежа',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RepeatSend',
                    width: 70,
                    text: 'Повторно',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                                    xtype: 'b4updatebutton'
                                },
                                { xtype: 'gkhbuttonimport' },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
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