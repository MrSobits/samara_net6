Ext.define('B4.view.calcaccount.CreditAddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.calcaccountcreditaddwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {type: 'vbox', align: 'stretch'},
    minHeight: 270,
    maxHeight: 270,
    width: 600,
    minWidth: 600,
    bodyPadding: 5,
    title: 'Добавление кредита',

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.CalcAccount'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150,
                allowBlank: false
            },
            items: [
            {
                xtype: 'datefield',
                name: 'DateStart',
                fieldLabel: 'Дата формирования',
                width: 250,
                maxWidth: 250
            },
            {
                xtype: 'b4selectfield',
                name: 'Account',
                fieldLabel: 'Р/с кредитуемого',
                store: 'B4.store.CalcAccount',
                columns: [
                    { text: 'Номер счета', dataIndex: 'AccountNumber', flex: 1, filter: { xtype: 'textfield' } },
                    { text: 'Кредитная организация', dataIndex: 'CreditOrg', flex: 1, filter: { xtype: 'textfield' } },
                    { text: 'Владелец счета', dataIndex: 'AccountOwner', flex: 1, filter: { xtype: 'textfield' } }
                ],
                editable: false,
                allowBlank: false,
                textProperty: 'AccountNumber',
                updateDisplayedText: function (data) {
                    var sfl = this,
                        text = '';

                    if (Ext.isString(data)) {
                        text = data;
                    } else {

                        if (data) {
                            text = data[sfl.textProperty]
                                ? data[sfl.textProperty]
                                : data.Id
                                    ? 'Без номера'
                                    : '';
                        }

                        if (Ext.isEmpty(text) && Ext.isArray(data)) {
                            text = Ext.Array.map(data, function (record) { return record[sfl.textProperty]; }).join();
                        }
                    }

                    sfl.setRawValue.call(sfl, text);
                }
            },
            {
                xtype: 'textfield',
                name: 'AccountOwner',
                fieldLabel: 'Владелец счета',
                readOnly: true
            },
            {
                xtype: 'b4filefield',
                name: 'Document',
                fieldLabel: 'Документ',
                allowBlank: true
            },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        flex: 1,
                        labelAlign: 'right',
                        labelWidth: 150,
                        decimalSeparator: ',',
                        minValue: 0,
                        allowBlank: false
                    },
                    items: [
                        {
                            fieldLabel: 'Сумма кредита',
                            name: 'CreditSum'
                        },
                        {
                            fieldLabel: 'Сумма процентов',
                            name: 'PercentSum'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        flex: 1,
                        labelAlign: 'right',
                        labelWidth: 150,
                        decimalSeparator: ',',
                        minValue: 0,
                        allowBlank: false
                    },
                    items: [
                        {
                            fieldLabel: 'Срок',
                            allowDecimals: false,
                            name: 'CreditPeriod'
                        },
                        {
                            fieldLabel: 'Процентная ставка (год)',
                            maxValue: 100,
                            name: 'PercentRate'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'component'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'PercentDebt',
                            hideTrigger: true,
                            fieldLabel: 'Сумма долга по процентам',
                            labelAlign: 'right',
                            labelWidth: 150,
                            decimalSeparator: ',',
                            minValue: 0,
                            allowBlank: false
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