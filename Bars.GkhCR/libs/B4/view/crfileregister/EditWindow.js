Ext.define('B4.view.crfileregister.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.CrFileRegister',
        'B4.store.RealityObject'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    width: 600,
    height: 180,
    layout: 'form',
    itemId: 'crfileregisterEditWindow',
    title: 'Запись реестра',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'combobox',
                        margin: '10 0 0 10',
                        labelWidth: 80,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'button',
                            text: 'Сформировать архив файлов',
                            tooltip: 'Сформировать',
                            iconCls: 'icon-accept',
                            width: 200,
                            itemId: 'formation'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'vbox',
                    defaults: {
                        xtype: 'combobox',
                        margin: '5 5 5 0',
                        labelWidth: 80,
                        align: 'stretch',
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                labelAlign: 'right',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    width: 550,
                                    name: 'RealityObject',
                                    fieldLabel: 'Жилой дом',
                                    textProperty: 'Address',
                                    store: 'B4.store.RealityObject',
                                    editable: false,
                                    flex: 1,
                                    itemId: 'sfRealityObject',
                                    allowBlank: false,
                                    hidden: false,
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
                                }
                            ]
                        },                      
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                labelAlign: 'right',
                                align: 'stretch',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    name: 'DateFrom',
                                    itemId: 'dfDateFrom',
                                    fieldLabel: 'Дата с'
                                },
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    name: 'DateTo',
                                    itemId: 'dfDateTo',
                                    fieldLabel: 'Дата по'
                                }
                            ]
                        }
                    ]
                },
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