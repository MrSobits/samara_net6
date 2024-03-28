Ext.define('B4.view.objectcr.performedworkact.PaymentDetailsWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.perfworkactpaymentdetailwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 500,
    minHeight: 250,
    height: 450,
    width: 700,
    bodyPadding: 5,
    closable: false,
    title: 'Оплата акта выполненных работ',
    modal: true,

    requires: [
        'B4.enums.ActPaymentType',
        'B4.view.objectcr.performedworkact.PaymentDetailsGrid',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.Panel',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.grid.plugin.HeaderFilters',
        'Ext.grid.plugin.CellEditing',
        'B4.model.PaymentOrderDetail',
        'B4.form.EnumCombo'
    ],

    initComponent: function() {
        var me = this,
            detailsStore = Ext.create('B4.base.Store', {
                autoLoad: false,
                model: 'B4.model.PaymentOrderDetail'
            });
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    height: 30,
                    layout: { type: 'hbox' },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            labelAlign: 'right',
                            name: 'DateDisposal',
                            fieldLabel: 'Дата распоряжения',
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                         {
                             xtype: 'datefield',
                             labelAlign: 'right',
                             name: 'DatePayment',
                             format: 'd.m.Y',
                             fieldLabel: 'Дата оплаты',
                             readOnly: true
                         }
                     ]
                },
                {
                    xtype: 'container',
                    height: 30,
                    layout: { type: 'hbox' },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },
                    items: [
                        {
                             xtype: 'b4enumcombo',
                             editable: false,
                             floating: false,
                             name: 'TypeActPayment',
                             fieldLabel: 'Вид оплаты',
                             enumName: 'B4.enums.ActPaymentType',
                             allowBlank: false
                        },
                        {
                            xtype: 'numberfield',
                            name: 'Paid',
                            format: '0.00',
                            fieldLabel: 'Оплачено, руб.',
                            readOnly: true
                        }
                    ]
                },
                
                {
                    xtype: 'b4grid',
                    store: detailsStore,
                    flex: 1, columnLines: true,
                    emptyText: 'У дома нет доступных средств',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'WalletName',
                            flex: 1,
                            text: 'Источник поступления'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Balance',
                            width: 100,
                            text: 'Сальдо, руб.'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Amount',
                            width: 100,
                            text: 'Оплата, руб.',
                            editor: {
                                xtype: 'gkhdecimalfield',
                                minValue: 0.00
                            }
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
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});