Ext.define('B4.view.longtermprobject.realaccount.OperationEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realaccountopereditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 400,
    minHeight: 240,
    maxHeight: 240,
    bodyPadding: 5,
    title: 'Операция по реальному счету',

    requires: [
        'B4.store.dict.AccountOperation',
        'B4.view.dict.accountoperation.Grid',

        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    flex: 1,
                    region: 'center',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            region: 'north',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 100
                            },
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                   xtype: 'b4selectfield',
                                   fieldLabel: 'Операция',
                                   name: 'Name',
                                  

                                   store: 'B4.store.dict.AccountOperation',
                                   editable: false,
                                   allowBlank: false
                                },
                                {
                                    xtype: 'datefield',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    name: 'OperationDate',
                                    fieldLabel: 'Дата',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Sum',
                                    fieldLabel: 'Сумма',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Receiver',
                                    fieldLabel: 'Получатель',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Payer',
                                    fieldLabel: 'Плательщик',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Purpose',
                                    fieldLabel: 'Назначение',
                                    allowBlank: false
                                }
                            ]
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