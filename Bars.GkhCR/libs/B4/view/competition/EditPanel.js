Ext.define('B4.view.competition.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.competitionEditPanel',
    closable: true,
    minWidth: 700,
    bodyPadding: 5,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.ux.button.Save',
        'B4.form.FileField'
    ],

    initComponent: function () {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                falign: 'stretch',
                labelAlign: 'right'
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
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
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Извещение',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'NotifNumber',
                                    fieldLabel: 'Номер извещения',
                                    allowBlank: false,
                                    maxLength: 100
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'NotifDate',
                                    fieldLabel: 'Дата извещения',
                                    allowBlank: false,
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            fieldLabel: 'Файл'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Рассмотрение заявок',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'ReviewDate',
                                    fieldLabel: 'Дата рассмотрения',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'timefield',
                                    fieldLabel: 'Время рассмотрения',
                                    format: 'H:i',
                                    submitFormat: 'Y-m-d H:i:s',
                                    minValue: '07:00',
                                    maxValue: '23:00',
                                    name: 'ReviewTime'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'ReviewPlace',
                            fieldLabel: 'Место рассмотрения',
                            maxLength: 300
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Подведение итогов',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'ExecutionDate',
                                    fieldLabel: 'Дата проведения',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'timefield',
                                    fieldLabel: 'Время проведения',
                                    format: 'H:i',
                                    submitFormat: 'Y-m-d H:i:s',
                                    minValue: '07:00',
                                    maxValue: '23:00',
                                    name: 'ExecutionTime'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'ExecutionPlace',
                            fieldLabel: 'Место проведения',
                            maxLength: 300
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});