Ext.define('B4.view.regoperator.accounts.SpecialAccountEditWin', {
    extend: 'B4.form.Window',
    alias: 'widget.regopspecacceditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    height: 550,
    width: 850,

    title: 'Расчетный счет',
    
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.creditorg.Grid',
        'B4.store.CreditOrg'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                 {
                     xtype: 'tabpanel',
                     border: false,
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
                         items: [
                             {
                                 xtype: 'hidden',
                                 name: 'IsSpecial'
                             },
                             {
                                 xtype: 'container',
                                 margins: '0 0 7 0',
                                 layout: {
                                     type: 'hbox',
                                     align: 'stretch'
                                 },
                                 defaults: {
                                     flex: 1,
                                     labelAlign: 'right',
                                     allowBlank: false,
                                     xtype: 'textfield',
                                     maxLength: 20,
                                     regex: /^\d*$/,
                                     regexText: 'Данное поле может содержать только цифры!'
                                 },
                                 items: [
                                     {
                                         name: 'Number',
                                         fieldLabel: 'Номер',
                                         labelWidth: 210
                                     }, {
                                         name: 'CorrAcc',
                                         labelWidth: 100,
                                         fieldLabel: 'Корр. счет'
                                     }, {
                                         name: 'Bik',
                                         labelWidth: 100,
                                         fieldLabel: 'БИК'
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
                                     flex: 1,
                                     labelWidth: 150,
                                     labelAlign: 'right'
                                 },
                                 items: [
                                     {
                                         xtype: 'b4selectfield',
                                         fieldLabel: 'Кредитная организация',
                                         labelWidth: 210,
                                         name: 'CreditOrg',
                                         store: 'B4.store.CreditOrg',
                                         editable: false
                                     },
                                     {
                                         xtype: 'numberfield',
                                         name: 'CreditLimit',
                                         fieldLabel: 'Лимит по кредиту',
                                         decimalSeparator: ',',
                                         hideTrigger: true,
                                         allowBlank: true
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
                                     flex: 1,
                                     xtype: 'datefield',
                                     labelWidth: 150,
                                     format: 'd.m.Y',
                                     labelAlign: 'right'
                                 },
                                 margins: '7 0 0 0',
                                 items: [
                                     {
                                         name: 'OpenDate',
                                         fieldLabel: 'Дата открытия',
                                         labelWidth: 210,
                                         allowBlank: false
                                     },
                                     {
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
                                 margins: '7 0 15 0',
                                 items: [
                                     {
                                         flex: 1,
                                         xtype: 'datefield',
                                         format: 'd.m.Y',
                                         labelAlign: 'right',
                                         name: 'LastOperationDate',
                                         labelWidth: 210,
                                         fieldLabel: 'Дата последней операции по счету'
                                     }
                                 ]
                             },
                             {
                                 xtype: 'fieldset',
                                 flex: 1,
                                 layout: {
                                     type: 'hbox',
                                     align: 'stretch'
                                 },
                                 title: 'Движение денежных средств',
                                 items: [
                                     {
                                         xtype: 'fieldset',
                                         title: 'Дебет',
                                         flex: 1,
                                         layout: 'fit',
                                         items: [{
                                             xtype: 'gridpanel',
                                             flex: 1,
                                             columns: [
                                                 {
                                                     text: 'Дата',
                                                     dataIndex: 'Date',
                                                     xtype: 'datecolumn',
                                                     format: 'd.m.Y',
                                                     flex: 1,
                                                     filter: {
                                                         xtype: 'datefield'
                                                     }
                                                 },
                                                 {
                                                     xtype: 'b4enumcolumn',
                                                     enumName: 'B4.enums.regop.OperationStatus',
                                                     text: 'Статус',
                                                     dataIndex: 'OperationStatus',
                                                     flex: 1
                                                 },
                                                 {
                                                     xtype: 'b4enumcolumn',
                                                     enumName: 'B4.enums.regop.PaymentOperationType',
                                                     text: 'Тип операции',
                                                     dataIndex: 'OperationType',
                                                     flex: 1
                                                 },
                                                 {
                                                     text: 'Сумма (руб.)',
                                                     dataIndex: 'OperationSum',
                                                     xtype: 'numbercolumn',
                                                     flex: 1,
                                                     filter: {
                                                         xtype: 'numberfield',
                                                         allowDecimals: true,
                                                         hideTrigger: true
                                                     }
                                                 }
                                             ]
                                         }]
                                     },
                                     {
                                         xtype: 'fieldset',
                                         title: 'Кредит',
                                         flex: 1,
                                         layout: 'fit',
                                         items: [{
                                             xtype: 'gridpanel',
                                             columns: [
                                                 {
                                                     text: 'Дата',
                                                     dataIndex: 'Date',
                                                     xtype: 'datecolumn',
                                                     format: 'd.m.Y',
                                                     flex: 1,
                                                     filter: {
                                                         xtype: 'datefield'
                                                     }
                                                 },
                                                 {
                                                     xtype: 'b4enumcolumn',
                                                     enumName: 'B4.enums.regop.OperationStatus',
                                                     text: 'Статус',
                                                     dataIndex: 'OperationStatus',
                                                     flex: 1
                                                 },
                                                 {
                                                     xtype: 'b4enumcolumn',
                                                     enumName: 'B4.enums.regop.PaymentOperationType',
                                                     text: 'Тип операции',
                                                     dataIndex: 'OperationType',
                                                     flex: 1
                                                 },
                                                 {
                                                     text: 'Сумма (руб.)',
                                                     dataIndex: 'OperationSum',
                                                     xtype: 'numbercolumn',
                                                     flex: 1,
                                                     filter: {
                                                         xtype: 'numberfield',
                                                         allowDecimals: true,
                                                         hideTrigger: true
                                                     }
                                                 }
                                             ]
                                         }]
                                     }
                                 ]
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