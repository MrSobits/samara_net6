Ext.define('B4.view.longtermprobject.realaccount.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realaccounteditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 700,
    minHeight: 400,
    height: 400,
    width: 700,
    title: 'Реальные счета',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.longtermprobject.realaccount.OperationGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    enableTabScroll: true,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Основная информация',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    allowBlank: false,
                                    maxLength: 50,
                                    name: 'Number',
                                    fieldLabel: 'Номер'
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 150,
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    padding: '0 0 5 0',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            labelAlign: 'right',
                                            format: 'd.m.Y',
                                            name: 'OpenDate',
                                            fieldLabel: 'Дата открытия',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'datefield',
                                            labelAlign: 'right',
                                            format: 'd.m.Y',
                                            name: 'CloseDate',
                                            fieldLabel: 'Дата закрытия'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 150,
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    padding: '0 0 5 0',
                                    items:[
                                        {
                                            xtype: 'textfield',
                                            name: 'TotalIncome',
                                            fieldLabel: 'Итого по приходу',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TotalOut',
                                            fieldLabel: 'Итого по расходу',
                                            allowBlank: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 150,
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    padding: '0 0 5 0',
                                    items:[
                                        {
                                            xtype: 'textfield',
                                            name: 'Balance',
                                            fieldLabel: 'Исходящее сальдо',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'datefield',
                                            labelAlign: 'right',
                                            format: 'd.m.Y',
                                            name: 'LastOperationDate',
                                            fieldLabel: 'Последняя операция',
                                            allowBlank: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Владелец счета',
                                    name: 'AccountOwner',
                                    store: 'B4.store.Contragent',
                                    columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }],
                                    editable: false,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'OverdraftLimit',
                                    fieldLabel: 'Лимит по овердрафту',
                                    allowBlank: false,
                                    editable: false
                                }
                            ]
                        },
                        {
                            xtype: 'realaccountopergrid',
                            columnLines: true,
                            flex: 1,
                            itemId: 'RealAccountOperationsGrid'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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
                            columns: 2,
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