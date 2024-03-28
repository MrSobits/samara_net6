Ext.define('B4.view.constructionobject.contract.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 600,
    minHeight: 100,
    bodyPadding: 5,
    alias: 'widget.constructobjcontracteditwindow',
    title: 'Договор',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [        
        'B4.ux.button.Close',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.enums.ConstructionObjectContractType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    labelAlign: 'right',
                    fieldLabel: 'Договор',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox'
                    },
                    padding: '0 0 5 0',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Number',
                            fieldLabel: 'Номер',
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'Date',
                            fieldLabel: 'от',
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    labelAlign: 'right',
                    allowBlank: false,
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Contragent',
                    fieldLabel: 'Подрядная организация',
                    allowBlank: false,
                    labelAlign: 'right',
                    store: 'B4.store.Contragent',
                    itemId: 'sfContragent',
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'combobox',
                    fieldLabel: 'Тип договора',
                    labelAlign: 'right',
                    store: B4.enums.ConstructionObjectContractType.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'Type',
                    allowBlank: false
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'Sum',
                    fieldLabel: 'Сумма договора (руб.)'
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelWidth: 160,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateStart',
                            fieldLabel: 'Дата начала действия договора',
                            format: 'd.m.Y',
                            listeners: {
                                change: function () {
                                    var me = this;
                                    me.up().down('datefield[name = DateEnd]').minValue = me.value;
                                }
                            }
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateEnd',
                            fieldLabel: 'Дата окончания действия договора',
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelWidth: 160,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateStartWork',
                            fieldLabel: 'Дата начала работ',
                            format: 'd.m.Y',
                            listeners: {
                                change: function () {
                                    var me = this;
                                    me.up().down('datefield[name = DateEndWork]').minValue = me.value;
                                }
                            }
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateEndWork',
                            fieldLabel: 'Дата окончания работ',
                            format: 'd.m.Y'
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
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
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