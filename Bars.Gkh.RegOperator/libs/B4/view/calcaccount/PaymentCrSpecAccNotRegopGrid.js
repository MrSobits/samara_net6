Ext.define('B4.view.calcaccount.PaymentCrSpecAccNotRegopGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'Ext.grid.feature.Summary',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.calcaccount.PaymentCrSpecAccNotRegop'
    ],

    title: 'Взносы на КР',

    alias: 'widget.paymentCrSpecAccNotRegopGrid',
    closable: true,
    store: 'calcaccount.PaymentCrSpecAccNotRegop',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'Municipality',
                     width: 160,
                     text: 'Муниципальный район',
                     filter: {
                         xtype: 'b4combobox',
                         operand: CondExpr.operands.eq,
                         storeAutoLoad: false,
                         hideLabel: true,
                         editable: false,
                         valueField: 'Name',
                         emptyItem: { Name: '-' },
                         url: '/Municipality/ListMoAreaWithoutPaging'
                     }
                 },
                {
                    xtype: 'gridcolumn',
                    text: 'Муниципальное образование',
                    flex: 1,
                    dataIndex: 'Settlement',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListSettlementWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Адрес',
                    flex: 1,
                    dataIndex: 'Address'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'InputDate',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата ввода'
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Сумма поступления',
                    flex: 1,
                    dataIndex: 'AmountIncome'
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Остаток на конец года',
                    flex: 1,
                    dataIndex: 'EndYearBalance'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл',
                    flex: 1,
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4updatebutton'
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.regop.ChargePeriod',
                            textProperty: 'Name',
                            editable: false,
                            width: 300,
                            windowContainerSelector: 'paymentCrSpecAccNotRegopGrid',
                            windowCfg: {
                                modal: true
                            },
                            trigger2Cls: '',
                            columns: [
                                {
                                    text: 'Наименование',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'Дата открытия',
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    dataIndex: 'StartDate',
                                    flex: 1,
                                    filter: { xtype: 'datefield' }
                                },
                                {
                                    text: 'Дата закрытия',
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    dataIndex: 'EndDate',
                                    flex: 1,
                                    filter: { xtype: 'datefield' }
                                },
                                {
                                    text: 'Состояние',
                                    dataIndex: 'IsClosed',
                                    flex: 1,
                                    renderer: function (value) {
                                        return value ? 'Закрыт' : 'Открыт';
                                    }
                                }
                            ],
                            name: 'ChargePeriod',
                            fieldLabel: 'Период',
                            labelWidth: 50
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            features: [{
                ftype: 'summary'
            }],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});