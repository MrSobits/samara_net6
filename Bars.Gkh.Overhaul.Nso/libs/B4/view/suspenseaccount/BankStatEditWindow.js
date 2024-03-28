Ext.define('B4.view.suspenseaccount.BankStatEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.suspenseaccountbankstateditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 750,
    minHeight: 450,
    height: 450,
    width: 750,
    bodyPadding: 5,
    closable: false,
    title: 'Банковская выписка',

    requires: [
        'B4.form.Combobox',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.suspenseaccount.OperationGrid',
        'B4.store.LongTermPrObject',
        'B4.store.account.Bank'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                    {
                        xtype: 'b4selectfield',
                        fieldLabel: 'Объект капитального ремонта',
                        name: 'LongTermPrObject',
                        store: 'B4.store.LongTermPrObject',
                        editable: false,
                        labelWidth: 160,
                        labelAlign: 'right',
                        columns: [
                            {
                                text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                            { text: 'Наименование', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                        ],
                        textProperty: 'Address'
                    },
                    {
                        xtype: 'b4selectfield',
                        fieldLabel: 'Счет',
                        name: 'BankAccount',
                        store: 'B4.store.account.Bank',
                        editable: false,
                        labelWidth: 160,
                        labelAlign: 'right',
                        columns: [
                            { text: 'Наименование', dataIndex: 'Number', flex: 1 },
                            { xtype: 'datecolumn', text: 'Дата открытия', dataIndex: 'OpenDate', format: 'd.m.Y', flex: 1 },
                            { xtype: 'datecolumn', text: 'Дата закрытия', dataIndex: 'CloseDate', format: 'd.m.Y', flex: 1 }
                        ],
                        textProperty: 'Number'
                    },
                    {
                        xtype: 'container',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 160,
                            flex: 1,
                            labelAlign: 'right',
                            allowBlank: false
                        },
                        padding: '0 0 5 0',
                        items: [
                            {
                                xtype: 'textfield',
                                name: 'Number',
                                itemId: 'tfNumber',
                                fieldLabel: 'Номер',
                                maxLength: 20,
                                regex: /^\d*$/,
                                regexText: 'Данное поле может содержать только цифры!'
                            },
                            {
                                xtype: 'datefield',
                                name: 'DocumentDate',
                                itemId: 'dfDocumentDate',
                                format: 'd.m.Y',
                                fieldLabel: 'От'
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
                                xtype: 'numberfield',
                                hideTrigger: true,
                                readOnly: true,
                                labelWidth: 160,
                                flex: 1,
                                labelAlign: 'right',
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                decimalSeparator: ','
                            },
                            padding: '0 0 5 0',
                            items: [
                                {
                                    name: 'BalanceIncome',
                                    fieldLabel: 'Входящий остаток (руб.)'
                                },
                                {
                                    name: 'BalanceOut',
                                    fieldLabel: 'Исходящий остаток (руб.)'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            padding: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    allowBlank: true,
                                    readOnly: true,
                                    name: 'LastOperationDate',
                                    fieldLabel: 'Дата последней операции по счету',
                                    labelWidth: 160,
                                    flex: 1,
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'container'
                                }
                            ]
                        },
                        {
                            xtype: 'suspenseaccountoperationgrid',
                            columnLines: true,
                            flex: 1,
                            disabled: true
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