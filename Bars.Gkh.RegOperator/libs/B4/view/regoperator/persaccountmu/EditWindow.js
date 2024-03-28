Ext.define('B4.view.regoperator.persaccountmu.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.regoperpersaccmueditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 600,
    minHeight: 410,
    maxHeight: 410,
    bodyPadding: 5,
    title: 'Лицевой счет по МО',
    closable: false,
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.Municipality',
        'B4.form.SelectField',
        'B4.store.regoperator.MunicipalityForSelect',
        'B4.view.dict.municipality.Grid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 200
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Mуниципальное образование',
                    name: 'Municipality',
                    store: 'B4.store.regoperator.MunicipalityForSelect',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'PersAccountNum',
                    fieldLabel: 'Лицевой счет',
                    maxLength: 150,
                    regex: /^\d+$/,
                    regexText: 'В это поле можно вводить только цифры'
                },
                {
                    xtype: 'textfield',
                    name: 'OwnerFio',
                    fieldLabel: 'ФИО собственника',
                    maxLength: 150
                },
                {
                    xtype: 'numberfield',
                    name: 'PaidContributions',
                    fieldLabel: 'Оплачено взносов (руб.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'CreditContributions',
                    fieldLabel: 'Начислено взносов (руб.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'CreditPenalty',
                    fieldLabel: 'Начислено пени  (руб.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'PaidPenalty',
                    fieldLabel: 'Оплачено пени  (руб.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'SubsidySumLocalBud',
                    fieldLabel: 'Сумма субсидии из МБ (руб.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'SubsidySumSubjBud',
                    fieldLabel: 'Сумма субсидии из БС (руб.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'SubsidySumFedBud',
                    fieldLabel: 'Сумма субсидии из ФБ (руб.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'SumAdopt',
                    fieldLabel: 'Сумма заимствования (руб.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'RepaySumAdopt',
                    fieldLabel: 'Погашенная сумма заимствования (руб.)',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    minValue: 0
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