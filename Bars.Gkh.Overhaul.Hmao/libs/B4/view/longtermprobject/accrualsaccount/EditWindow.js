Ext.define('B4.view.longtermprobject.accrualsaccount.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.accrualsaccounteditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 700,
    minHeight: 400,
    height: 400,
    width: 700,
    title: 'Cчет начислений',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.longtermprobject.accrualsaccount.OperationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 200
            },
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
                                     items: [
                                         {
                                             xtype: 'textfield',
                                             name: 'TotalDebit',
                                             fieldLabel: 'Итого начислено',
                                             allowBlank: false
                                         },
                                         {
                                             xtype: 'textfield',
                                             name: 'TotalCredit',
                                             fieldLabel: 'Итого оплачено',
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
                                     items: [
                                         {
                                             xtype: 'textfield',
                                             name: 'Balance',
                                             fieldLabel: 'Сальдо по счету',
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
                                     xtype: 'numberfield',
                                     hideTrigger: true,
                                     name: 'OpeningBalance',
                                     fieldLabel: 'Входящее сальдо',
                                     keyNavEnabled: false,
                                     mouseWheelEnabled: false,
                                     decimalSeparator: ',',
                                     minValue: 0
                                 }
                             ]
                         },
                         {
                             xtype: 'accrualaccountopergrid',
                             columnLines: true,
                             flex: 1
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