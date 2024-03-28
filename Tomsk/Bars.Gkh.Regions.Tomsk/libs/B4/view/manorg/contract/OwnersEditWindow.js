Ext.define('B4.view.manorg.contract.OwnersEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.RealityObject',
        'B4.view.realityobj.Grid',
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.realityobj.Grid',
        'B4.enums.ManOrgContractOwnersFoundation',
        'B4.store.realityobj.ByManOrg',
        'B4.enums.ContractStopReasonEnum'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    closable: 'hide',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 853,
    minWidth: 853,
    maxWidth: 853,
    height: 550,
    minHeight: 550,
    bodyPadding: 5,
    itemId: 'manorgContractOwnersEditWindow',
    title: 'Форма редактирования договора управления (с собственниками)',
    trackResetOnLoad: true,
    autoScroll: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 160,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'fieldset',
                    layout: { type: 'vbox', align: 'stretch' },
                    title: 'Реквизиты',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            editable: false,
                            name: 'ContractFoundation',
                            fieldLabel: 'Основание',
                            displayField: 'Display',
                            valueField: 'Value',
                            store: B4.enums.ManOrgContractOwnersFoundation.getStore()
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'RealityObjectId',
                            itemId: 'sfRealityObject',
                            fieldLabel: 'Жилой дом',
                           
                            store: 'B4.store.realityobj.ByManOrg',
                            textProperty: 'Address',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                {
                                    text: 'Муниципальное образование',
                                    dataIndex: 'Municipality',
                                    flex: 1,
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
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                allowBlank: false,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'от',
                                    labelWidth: 50,
                                    maxWidth: 150
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                xtype: 'datefield',
                                labelAlign: 'right',
                                format: 'd.m.Y',
                                flex: 1,
                                labelWidth: 150
                            },
                            items: [
                                {
                                    name: 'StartDate',
                                    fieldLabel: 'Дата начала управления',
                                    allowBlank: false
                                },
                                {
                                    name: 'PlannedEndDate',
                                    fieldLabel: 'Плановая дата окончания',
                                    labelWidth: 160
                                },
                                {
                                    name: 'EndDate',
                                    fieldLabel: 'Дата окончания управления',
                                    labelWidth: 170
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'FileInfo',
                            fieldLabel: 'Файл'
                        },
                        {
                            xtype: 'textarea',
                            name: 'Note',
                            fieldLabel: 'Примечание',
                            maxLength: 300,
                            height: 40
                        }
                    ]
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    name: 'ContractStopReason',
                    fieldLabel: 'Основание завершения обслуживания',
                    allowBlank: false,
                    displayField: 'Display',
                    store: B4.enums.ContractStopReasonEnum.getStore(),
                    valueField: 'Value'
                },
                {
                    xtype: 'textfield',
                    name: 'TerminateReason',
                    fieldLabel: 'Основание расторжения',
                    maxLength: 300,
                    flex: 1
                },
                {
                    xtype: 'fieldset',
                    layout: { type: 'vbox', align: 'stretch' },
                    title: 'Сведения о включенных/Исключенных из ресстра домов',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumberOnRegistry',
                                    fieldLabel: 'Номер приказа включения в реестр',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDateOnRegistry',
                                    fieldLabel: 'Дата приказа включения в реестр',
                                    labelWidth: 200,
                                    maxWidth: 150
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
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumberOffRegistry',
                                    fieldLabel: 'Номер приказа исключения из реестра',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDateOffRegistry',
                                    fieldLabel: 'Дата приказа исключения из реестра',
                                    labelWidth: 200,
                                    maxWidth: 150
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
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});