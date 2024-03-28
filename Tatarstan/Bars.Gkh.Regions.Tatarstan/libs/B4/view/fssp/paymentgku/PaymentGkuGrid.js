Ext.define('B4.view.fssp.paymentgku.PaymentGkuGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.paymentgkugrid',
    
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Save',

        'B4.form.MonthPicker',
        'B4.view.fssp.paymentgku.PgmuSelectAddress',
        
        'B4.model.fssp.paymentgku.PaymentGku',
    ],
    
    title: 'Оплаты по ЖКУ',
    closable: true,
    
    initComponent: function(){
        var me = this,
            store = Ext.create('B4.base.Store', {
                model: 'B4.model.fssp.paymentgku.PaymentGku',
                autoLoad: false
            });
        
        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: {
                defaults: {
                    flex: 2,
                    xtype: 'gridcolumn'
                },
                items: [
                    {
                        dataIndex: 'Period',
                        xtype: 'datecolumn',
                        format: 'F Y',
                        text: 'Период',
                        flex: 1,
                        filter: { xtype: 'b4monthpicker' }
                    },
                    {
                        dataIndex: 'AccountNumber',
                        text: 'Номер лицевого счета',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'numbercolumn',
                        dataIndex: 'DebtSum',
                        text: 'Сумма задолженности на 1 число следующего месяца (руб.)',
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: true,
                            decimalSeparator: ',',
                            operand: CondExpr.operands.eq,
                            hideTrigger: true
                        }
                    },
                    {
                        xtype: 'numbercolumn',
                        dataIndex: 'Accured',
                        text: 'Начислено за текущий месяц (руб.)',
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: true,
                            decimalSeparator: ',',
                            operand: CondExpr.operands.eq,
                            hideTrigger: true
                        }
                    },
                    {
                        xtype: 'numbercolumn',
                        dataIndex: 'PayedForPreviousMonth',
                        text: 'Оплачено за предыдущий месяц (руб.)',
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: true,
                            decimalSeparator: ',',
                            operand: CondExpr.operands.eq,
                            hideTrigger: true
                        }
                    }
                ]
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    layout: {
                        type: 'vbox'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'middle',
                            },
                            defaults: {
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'b4monthpicker',
                                    dateFormat: 'F Y',
                                    name: 'PeriodStart',
                                    fieldLabel: 'Период с',
                                    labelWidth: 50,
                                    width: 170,
                                    margin: 10,
                                    editable: false,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'b4monthpicker',
                                    name: 'PeriodEnd',
                                    dateFormat: 'F Y',
                                    fieldLabel: 'по',
                                    labelWidth: 20,
                                    width: 140,
                                    margin: 10,
                                    editable: false,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'RegistrationNumber',
                                    fieldLabel: 'Регистрационный<br>номер ИП',
                                    labelWidth: 95,
                                    width: 400,
                                    margin: '10 0 10 30',
                                    readOnly: true
                                },
                            ]
                        },
                        {
                            xtype: 'pgmuselectaddress',
                            name: 'Address',
                            fieldLabel: 'Адрес',
                            labelWidth: 40,
                            width: 332,
                            margin: '5 5 5 7',
                            allowBlank: false
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                margin: '10 10 2 10',
                                cls: 'toolbar-btn'
                            },
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Применить',
                                    tooltip: 'Применить фильтрацию',
                                    action: 'applyFilter'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-cross',
                                    text: 'Cбросить',
                                    tooltip: 'Cбросить фильтрацию',
                                    action: 'dropFilter'
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
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
        });

        me.callParent(arguments);
    }
})