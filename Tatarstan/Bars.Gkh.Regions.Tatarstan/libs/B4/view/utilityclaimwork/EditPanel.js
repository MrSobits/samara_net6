Ext.define('B4.view.utilityclaimwork.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.utilitydebtorclaimworkeditpanel',
    title: 'Задолженность по оплате ЖКУ',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.SelectField',
        'B4.store.RealityObject',
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.ux.button.AcceptMenuButton',
        'B4.enums.OwnerType',
        'B4.view.Control.GkhDecimalField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.RealityObject',
                    textProperty: 'Address',
                    editable: false,
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
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    name: 'RealityObject',
                    fieldLabel: 'Адрес',
                    labelAlign: 'right',
                    labelWidth: 150,
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    name: 'AccountOwner',
                                    fieldLabel: 'Aбонент'
                                },
                                {
                                    name: 'PersonalAccountState',
                                    fieldLabel: 'Статус ЛС'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'ChargeDebt',
                                    fieldLabel: 'Сумма долга'
                                },
                                {
                                    name: 'CountDaysDelay',
                                    fieldLabel: 'Количество дней просрочки'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',                           
                                    fieldLabel: 'Тип абонента',
                                    enumName: 'B4.enums.OwnerType',
                                    name: 'OwnerType'
                                },
                                {
                                    name: 'PersonalAccountNum',
                                    fieldLabel: 'Номер ЛС'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'PenaltyDebt',
                                    fieldLabel: 'Сумма долга по пени'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DebtPaidDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата погашения'
                                }
                            ]
                        }
                    ]
                },

                {
                    xtype: 'checkbox',
                    labelWidth: 150,
                    name: 'IsDebtPaid',
                    fieldLabel: 'Задолженность погашена'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'acceptmenubutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    name: 'State',
                                    text: 'Статус',
                                    menu: []
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