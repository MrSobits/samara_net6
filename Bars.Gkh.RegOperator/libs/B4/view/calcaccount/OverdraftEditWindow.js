Ext.define('B4.view.calcaccount.OverdraftEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.calcaccountoverdrafteditwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 600,
    bodyPadding: 5,
    title: 'Овердрафт',

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
                labelWidth: 160,
                allowBlank: false
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'DateStart',
                    fieldLabel: 'Дата формирования',
                    maxWidth: 260
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Account',
                    fieldLabel: 'Р/с для установки овердрафта',
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
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        flex: 1,
                        labelAlign: 'right',
                        labelWidth: 160,
                        decimalSeparator: ',',
                        minValue: 0,
                        allowBlank: false
                    },
                    items: [
                        {
                            fieldLabel: 'Лимит по овердрафту',
                            name: 'OverdraftLimit'
                        },
                        {
                            fieldLabel: 'Процентная ставка (день)',
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
                            name: 'OverdraftPeriod',
                            hideTrigger: true,
                            fieldLabel: 'Срок беспроцентного овердрафта',
                            labelAlign: 'right',
                            labelWidth: 160,
                            allowDecimals: false,
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