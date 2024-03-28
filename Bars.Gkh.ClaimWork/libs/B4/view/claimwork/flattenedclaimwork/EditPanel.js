Ext.define('B4.view.claimwork.flattenedclaimwork.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.flattenedclaimworkeditpanel',
    title: 'Архив ПИР',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.ux.button.AcceptMenuButton'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                    {
                        xtype: 'b4enumcombo',
                        fieldLabel: 'Тип акта',
                        enumName: 'B4.enums.ActViolIdentificationType',
                        name: 'ActType'
                    },
                    {
                        xtype: 'fieldset',
                        title: 'Уведомление о вызове',
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'container',
                                padding: '0 0 5 0',
                                layout: {
                                    type: 'hbox',
                                    align: 'stretch'
                                },
                                defaults: {
                                    labelWidth: 130,
                                    labelAlign: 'right',
                                    flex:1
                                },
                                items: [
                                    {
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        name: 'SendDate',
                                        fieldLabel: 'Дата отправки'
                                    },
                                    {
                                        xtype: 'b4enumcombo',
                                        fieldLabel: 'Факт получения',
                                        enumName: 'B4.enums.FactOfReceiving',
                                        name: 'FactOfReceiving'
                                    }
                                ]
                            },
                            {
                                xtype: 'container',
                                padding: '0 0 5 0',
                                layout: {
                                    type: 'hbox',
                                    align: 'stretch'
                                },
                                defaults: {
                                    labelWidth: 130,
                                    labelAlign: 'right',
                                    flex: 1
                                },
                                items: [
                                    {
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        name: 'SignDate',
                                        fieldLabel: 'Дата подписания'
                                    },
                                    {
                                        fieldLabel: 'Время подписания',
                                        name: 'SignTime',
                                        xtype: 'textfield',
                                        regex: /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/,
                                        regexText: 'В это поле необходимо вводить время в формате 00:00'
                                    }
                                ]
                            },
                            {
                                xtype: 'textarea',
                                name: 'SignPlace',
                                fieldLabel: 'Место подписания',
                                maxLength: 500,
                                labelWidth: 130,
                                labelAlign: 'right'
                            }
                        ]
                    },
                    {
                        xtype: 'fieldset',
                        title: 'Подписание акта',
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 130,
                            labelAlign: 'right'
                        },
                        items: [
                            {
                                xtype: 'container',
                                padding: '0 0 5 0',
                                layout: {
                                    type: 'hbox',
                                    align: 'stretch'
                                },
                                defaults: {
                                    labelWidth: 130,
                                    labelAlign: 'right',
                                    flex: 1
                                },
                                items: [
                                    {
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        name: 'FormDate',
                                        fieldLabel: 'Дата составления'
                                    },
                                    {
                                        fieldLabel: 'Время составления',
                                        name: 'FormTime',
                                        xtype: 'textfield',
                                        regex: /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/,
                                        regexText: 'В это поле необходимо вводить время в формате 00:00'
                                    }
                                ]
                            },
                            {
                                xtype: 'textarea',
                                name: 'FormPlace',
                                fieldLabel: 'Место подписания',
                                maxLength: 500
                            },
                            {
                                xtype: 'textarea',
                                name: 'Persons',
                                fieldLabel: 'Лица, присутствующие при составлении',
                                maxLength: 500
                            },
                            {
                                xtype: 'b4enumcombo',
                                fieldLabel: 'Факт подписания',
                                enumName: 'B4.enums.FactOfSigning',
                                name: 'FactOfSigning'
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
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'acceptmenubutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    action: 'delete',
                                    text: 'Удалить',
                                    textAlign: 'left'
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