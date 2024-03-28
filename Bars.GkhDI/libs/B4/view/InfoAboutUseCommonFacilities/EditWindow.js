Ext.define('B4.view.infoaboutusecommonfacilities.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'border',
    width: 655,
    height: 540,
    autoScroll: true,
    bodyPadding: 5,
    itemId: 'infoAboutUseCommonFacilitiesEditWindow',
    title: 'Редактирование записи',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        'B4.enums.TypeContractDi'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    height: 150,
                    region: 'north',
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'KindCommomFacilities',
                            fieldLabel: 'Вид общего имущества',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'AppointmentCommonFacilities',
                            fieldLabel: 'Назначение общего имущества'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'AreaOfCommonFacilities',
                            fieldLabel: 'Площадь общего имущества (заполняется в отношении помещений и земельных участков) (кв.м)',
                            decimalSeparator: ',',
                            allowDecimals: true,
                            allowBlank: true,
                            minValue: 0
                        },
                        {
                            xtype: 'fieldset',
                            layout: 'anchor',
                            title: 'Протокол общего собрания собственников помещений',
                            anchor: '100%',
                            bodyPadding: 5,
                            defaults: {
                                labelWidth: 190,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    defaults: {
                                        xtype: 'container',
                                        layout: 'anchor',
                                        flex: 1,
                                        defaults: {
                                            labelAlign: 'right',
                                            labelWidth: 190,
                                            anchor: '100%'
                                        }
                                    },
                                    layout: {
                                        type: 'hbox',
                                        pack: 'start',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Number',
                                                    fieldLabel: 'Номер',
                                                    allowBlank: false,
                                                    maxLength: 300
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    name: 'From',
                                                    fieldLabel: 'Дата',
                                                    allowBlank: false,
                                                    width: 290
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            layout: 'anchor',
                            title: 'Договор',
                            anchor: '100%',
                            bodyPadding: 5,
                            defaults: {
                                labelWidth: 190,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                    {
                                        xtype: 'textfield',
                                        name: 'Lessee',
                                        fieldLabel: 'Наименование арендатора',
                                        allowBlank: false,
                                        maxLength: 300
                                    },
                                    {
                                        xtype: 'textfield',
                                        fieldLabel: 'Номер договора',
                                        name: 'ContractNumber'
                                    },
                                    {
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        name: 'ContractDate',
                                        fieldLabel: 'Дата договора'
                                    },
                                    {
                                        xtype: 'numberfield',
                                        name: 'CostByContractInMonth',
                                        fieldLabel: 'Стоимость по договору в месяц (руб.)',
                                        decimalSeparator: ',',
                                        allowDecimals: true,
                                        allowBlank: true,
                                        minValue: 0
                                    },
                                    {
                                        xtype: 'combobox',
                                        editable: false,
                                        fieldLabel: 'Тип договора',
                                        store: B4.enums.TypeContractDi.getStore(),
                                        displayField: 'Display',
                                        valueField: 'Value',
                                        name: 'TypeContract'
                                    },
                                    {
                                        xtype: 'container',
                                        anchor: '100%',
                                        defaults: {
                                            xtype: 'container',
                                            layout: 'anchor',
                                            flex: 1,
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 190,
                                                anchor: '100%'
                                            }
                                        },
                                        layout: {
                                            type: 'hbox',
                                            pack: 'start',
                                            align: 'stretch'
                                        },
                                        items: [
                                                    {
                                                        items: [{
                                                            xtype: 'datefield',
                                                            format: 'd.m.Y',
                                                            name: 'DateStart',
                                                            fieldLabel: 'Дата начала',
                                                            width: 290,
                                                            allowBlank: false
                                                        }]
                                                    },
                                                    {
                                                        items: [{
                                                            xtype: 'datefield',
                                                            format: 'd.m.Y',
                                                            name: 'DateEnd',
                                                            fieldLabel: 'Дата окончания',
                                                            width: 290,
                                                            allowBlank: false
                                                        }]
                                                    }
                                        ]
                                    },
                                    {
                                        xtype: 'gkhdecimalfield',
                                        name: 'CostContract',
                                        fieldLabel: 'Сумма договора',
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
                            columns: 1,
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
                            columns: 1,
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