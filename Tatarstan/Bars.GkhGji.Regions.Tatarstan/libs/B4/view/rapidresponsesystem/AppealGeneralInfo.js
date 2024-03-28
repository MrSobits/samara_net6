Ext.define('B4.view.rapidresponsesystem.AppealGeneralInfo', {
    extend: 'Ext.form.Panel',
    alias: 'widget.appealgeneralinfo',
    
    itemId: 'appealGeneralInfoTab',
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    border: false,
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },

    requires: [
        'B4.form.EnumCombo',
        'B4.form.FileField',
        'B4.form.EnumCombo',
        'B4.view.rapidresponsesystem.AppealResponsePanel',
        'B4.enums.TypeCorrespondent'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                margin: 5,
                padding: 5
            },
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Корреспондент',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        readOnly: true
                    },
                    items: [
                        {
                            xtype: 'b4enumcombo',
                            name: 'TypeCorrespondent',
                            fieldLabel: 'Тип корреспондента',
                            enumName: 'B4.enums.TypeCorrespondent'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Correspondent',
                            fieldLabel: 'Корреспондент'
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150,
                                readOnly: true,
                                flex: 1
                            },
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'CorrespondentAddress',
                                    fieldLabel: 'Адрес корреспондента'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'CorrespondentFlatNum',
                                    fieldLabel: 'Номер квартиры'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150,
                                readOnly: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'CorrespondentEmail',
                                    fieldLabel: 'Электронная почта'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'CorrespondentPhone',
                                    fieldLabel: 'Контактный телефон'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Обращение',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        readOnly: true
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150,
                                readOnly: true,
                                flex: 1
                            },
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'AppealKind',
                                    fieldLabel: 'Вид обращения'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'QuestionsCount',
                                    fieldLabel: 'Количество вопросов'
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'Subjects',
                            fieldLabel: 'Тематики'
                        },
                        {
                            xtype: 'textarea',
                            name: 'ProblemDescription',
                            fieldLabel: 'Описание проблемы'
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150,
                                readOnly: true,
                            },
                            margin: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'AppealFileName',
                                    fieldLabel: 'Файл',
                                    readOnly: true,
                                    flex: 6
                                },
                                {
                                    xtype: 'buttongroup',
                                    layout: 'anchor',
                                    items: [
                                        {
                                            xtype: 'button',
                                            action: 'AppealFileDownload',
                                            text: 'Скачать',
                                            anchor: '100%'
                                        }
                                    ],
                                    flex: 1
                                }
                            ]
                        },
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Ответ',
                    itemId: 'AppealResponseSet',
                    items: [
                        {
                            xtype: 'appealresponsepanel'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});