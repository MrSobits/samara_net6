Ext.define('B4.view.publicservorg.ContractTimingInformation', {
    extend: 'Ext.form.Panel',
    alias: 'widget.contracttiminginformationpanel',

    requires: [],
    mixins: ['B4.mixins.window.ModalMask'],

    bodyPadding: 3,

    closeAction: 'hide',

    closable: false,
    title: 'Сведения о сроках',

    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    trackResetOnLoad: true,
    header: true,
    border: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'fieldset',
                    layout: { type: 'vbox', align: 'stretch' },
                    title: 'Периоды выставления платёжных документов',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 5 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 270,
                                labelAlign: 'right',
                                flex: 1,
                                allowBlank: false
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'TermBillingPaymentNoLaterThan',
                                    fieldLabel: 'Срок выставления счетов к оплате, не позднее (число месяца, следующего за расчетным)'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'TermPaymentNoLaterThan',
                                    fieldLabel: 'Срок оплаты, не позднее (число месяца, следующего за расчетным)'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 5 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 270,
                                labelAlign: 'right',
                                width: 375,
                                allowBlank: false
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'DeadlineInformationOfDebt',
                                    fieldLabel: 'Срок предоставления информации о поступивших задолженностях'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: { type: 'vbox', align: 'stretch' },
                    title: 'Периоды ввода показанй приборов учёта',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 5 5 0',
                            layout: { type: 'hbox', align: 'stretch' },
                            defaults: {
                                xtype: 'fieldset',
                                layout: { type: 'vbox', align: 'stretch' },
                                flex: 1,
                                defaults: {
                                    labelAlign: 'right',
                                    labelWidth: 200,
                                    allowBlank: false
                                }
                            },
                            items: [
                                {
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            name: 'DayStart',
                                            fieldLabel: ' Дата начала',
                                            maxValue: 30,
                                            minValue: 1,
                                            allowDecimals: false
                                        },
                                        {
                                            xtype: 'radiogroup',
                                            itemId: 'startEnter',
                                            name: 'StartDeviceMetteringIndication',
                                            width: 250,
                                            padding: '0 0 5 0',
                                            columns: 2,
                                            items: [
                                                {
                                                    name: 'StartDeviceMetteringIndication',
                                                    boxLabel: 'Текущего месяца',
                                                    itemId: 'ThisMonth',
                                                    inputValue: 0
                                                },
                                                {
                                                    name: 'StartDeviceMetteringIndication',
                                                    boxLabel: 'Следующего месяца',
                                                    itemId: 'NextMonth',
                                                    inputValue: 1
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            name: 'DayEnd',
                                            fieldLabel: 'Дата окончания',
                                            maxValue: 31,
                                            minValue: 2,
                                            allowDecimals: false
                                        },
                                        {
                                            xtype: 'radiogroup',
                                            itemId: 'endEnter',
                                            name: 'EndDeviceMetteringIndication',
                                            width: 250,
                                            padding: '0 0 5 0',
                                            columns: 2,
                                            items: [
                                                {
                                                    name: 'EndDeviceMetteringIndication',
                                                    boxLabel: 'Текущего месяца',
                                                    itemId: 'ThisMonth',
                                                    inputValue: 0
                                                },
                                                {
                                                    name: 'EndDeviceMetteringIndication',
                                                    boxLabel: 'Следующего месяца',
                                                    itemId: 'NextMonth',
                                                    inputValue: 1
                                                }
                                            ]
                                        }
                                    ]
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