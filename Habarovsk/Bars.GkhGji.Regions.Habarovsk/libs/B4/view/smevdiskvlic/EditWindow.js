Ext.define('B4.view.smevdiskvlic.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.smevdiskvlic.FileInfoGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'smevdiskvlicEditWindow',
    title: 'Запрос реестр дискв. лиц',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [{
                xtype: 'tabpanel',
                border: false,
                flex: 1,
                defaults: {
                    border: false
                },
                items: [{
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 100,
                            margin: '5 0 5 0',
                            align: 'stretch',
                            labelAlign: 'right'
                        },
                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                        title: 'Форма запроса',
                        border: false,
                        bodyPadding: 10,
                        items: [{
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 100,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Реквизиты субъекта запроса',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'BirthPlace',
                                            fieldLabel: 'Место рождения',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfBirthPlace'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'BirthDate',
                                            fieldLabel: 'Дата рождения',
                                            allowBlank: false,
                                            flex: 0.5,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfBirthDate'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        //     margin: '10 0 5 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'FamilyName',
                                            fieldLabel: 'Фамилия',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfFamilyName'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FirstName',
                                            fieldLabel: 'Имя',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfFirstName'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Patronymic',
                                            fieldLabel: 'Отчество',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfPatronymic'
                                        }

                                    ]
                                },

                                ]
                            },
                            {
                                xtype: 'tabpanel',
                                border: false,
                                flex: 1,
                                defaults: {
                                    border: false
                                },
                                items: [{
                                    xtype: 'smevdiskvlicfileinfogrid',
                                    flex: 1
                                }]
                            }
                        ]
                    },
                    {
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 170,
                            falign: 'stretch',
                            labelAlign: 'right'
                        },
                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                        title: 'Ответ',
                        border: false,
                        bodyPadding: 10,
                        items: [{
                                xtype: 'container',
                                layout: 'hbox',
                                defaults: {
                                    xtype: 'combobox',
                                    margin: '0 0 5 0',                                    
                                    labelAlign: 'right',
                                },
                                items: [{
                                        xtype: 'button',
                                        text: 'Получить сведения',
                                        tooltip: 'Получить сведения',
                                        iconCls: 'icon-accept',
                                        width: 200,
                                        //    action: 'romExecute',
                                        itemId: 'sendGetrequestButton'
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'Answer',
                                        fieldLabel: 'Ответ на запрос',
                                        itemId: 'dfAnswerGet',
                                        allowBlank: true,
                                        disabled: false,
                                        flex: 1,
                                        editable: false,
                                        maxLength: 1000,
                                        labelWidth: 100,
                                        readOnly: true
                                    },
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 250,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Сведения о дисквалификации',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'FormDate',
                                            fieldLabel: 'Дата формирования',
                                            format: 'd.m.Y',
                                            flex: 1,
                                            allowBlank: true,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'EndDisqDate',
                                            fieldLabel: 'Дата окончания',
                                            format: 'd.m.Y',
                                            flex: 1,
                                            allowBlank: true,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'RegNumber',
                                            fieldLabel: 'Номер записи',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right'
                                        },
                                        items: [
                                            {
                                                xtype: 'label',
                                                name: 'label',
                                                text: 'Срок дисквалификации:',
                                                labelWidth: 120
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'DisqDays',
                                                fieldLabel: 'дней',
                                                allowBlank: true,
                                                disabled: false,
                                                editable: false,
                                                flex: 1,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'DisqMonths',
                                                fieldLabel: 'месяцев',
                                                allowBlank: true,
                                                disabled: false,
                                                editable: false,
                                                flex: 1,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'DisqYears',
                                                fieldLabel: 'лет',
                                                allowBlank: true,
                                                disabled: false,
                                                editable: false,
                                                flex: 1,
                                                readOnly: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            margin: '5 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [{
                                            xtype: 'textfield',
                                            name: 'Article',
                                            fieldLabel: 'Статья КоАП',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: false,
                                            readOnly: true
                                        }]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            margin: '5 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'LawName',
                                                fieldLabel: 'Наименование суда',
                                                allowBlank: true,
                                                //   flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            margin: '5 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [
                                            {
                                                xtype: 'datefield',
                                                name: 'LawDate',
                                                fieldLabel: 'Дата постановления',
                                                format: 'd.m.Y',
                                                allowBlank: true,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'CaseNumber',
                                                fieldLabel: 'Номер Дела',
                                                allowBlank: true,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                        xtype: 'buttongroup',
                        columns: 2,
                        items: [{
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
                        items: [{
                            xtype: 'b4closebutton'
                        }]
                    }
                ]
            }]
        });

        me.callParent(arguments);
    }
});