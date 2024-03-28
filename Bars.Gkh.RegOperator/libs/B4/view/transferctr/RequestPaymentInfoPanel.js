Ext.define('B4.view.transferctr.RequestPaymentInfoPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.requestpaymentinfopanel',

    requires: [
        'B4.form.ComboBox',
        'B4.enums.TypePaymentRfCtr',
        'B4.enums.KindPayment',
        'B4.enums.TypeCalculationNds',
        'B4.view.transferctr.PaymentDetailGrid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'b4combobox',
                                labelWidth: 150,
                                labelAlign: 'right',
                                editable: false,
                                flex: 1
                            },
                            items: [
                                {
                                    name: 'PaymentType',
                                    fieldLabel: 'Тип платежа',
                                    items: B4.enums.TypePaymentRfCtr.getItems()
                                },
                                {
                                    fieldLabel: 'Вид платежа',
                                    name: 'KindPayment',
                                    items: B4.enums.KindPayment.getItems()
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'PaymentPriority',
                                    fieldLabel: 'Очередность платежа',
                                    maxLength: 250
                                },
                                {
                                    xtype: 'b4combobox',
                                    fieldLabel: 'Вид расчета НДС',
                                    name: 'TypeCalculationNds',
                                    items: B4.enums.TypeCalculationNds.getItems()
                                }
                            ]
                        },
                        {
                            xtype: 'checkbox',
                            name: 'IsEditPurpose',
                            labelWidth: 150,
                            labelAlign: 'right',
                            fieldLabel: 'Редактировать поле'
                        },
                        {
                            xtype: 'textarea',
                            fieldLabel: 'Назначение платежа',
                            name: 'PaymentPurposeDescription',
                            labelWidth: 150,
                            labelAlign: 'right',
                            height: 50
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'numberfield',
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1,
                                readOnly: true,
                                hideTrigger: true
                            },
                            items: [
                                {
                                    name: 'Sum',
                                    fieldLabel: 'Сумма заявки, руб.'
                                },
                                {
                                    name: 'PaidSum',
                                    fieldLabel: 'Сумма оплаты, руб.'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата оплаты',
                                    name: 'PaymentDate',
                                    format: 'd.m.Y',
                                    labelWidth: 150,
                                    labelAlign: 'right',
                                    flex: 1,
                                    readOnly: true
                                },
                                {
                                    xtype: 'component',
                                    flex: 1
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'transferctrpaydetailgrid',
                    flex: 1,
                    title: 'Источники финансирования'
                }
            ]
        });

        me.callParent(arguments);
    }
});
