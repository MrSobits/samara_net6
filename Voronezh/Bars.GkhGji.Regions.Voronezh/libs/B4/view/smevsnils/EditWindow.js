Ext.define('B4.view.smevsnils.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.enums.SMEVGender',
        'B4.enums.SnilsPlaceType',
        'B4.form.SelectField',
        'B4.view.smevdiskvlic.FileInfoGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'smevsnilsEditWindow',
    title: 'Запрос СНИЛС',
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
                                            xtype: 'b4enumcombo',
                                            name: 'SMEVGender',
                                            labelWidth: 60,
                                            fieldLabel: 'Пол',
                                            enumName: 'B4.enums.SMEVGender',
                                            flex: 0.5,
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
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Country',
                                            fieldLabel: 'Страна рождения',
                                            allowBlank: true,
                                            flex: 0.5,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 60,
                                            itemId: 'dfCountry'
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'SnilsPlaceType',
                                            labelWidth: 80,
                                            fieldLabel: 'Тип места рождения',
                                            enumName: 'B4.enums.SnilsPlaceType',
                                            flex: 0.5,
                                        },
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
                                            name: 'Surname',
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
                                            name: 'Name',
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
                                            name: 'PatronymicName',
                                            fieldLabel: 'Отчество',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfPatronymic'
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
                                            name: 'Region',
                                            fieldLabel: 'Область',
                                            allowBlank: true,
                                            flex: 1,
                                            maxLength: 150
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'District',
                                            fieldLabel: 'Район',
                                            allowBlank: true,
                                            flex: 1,
                                            maxLength: 150,
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Settlement',
                                            fieldLabel: 'Населенный пункт',
                                            allowBlank: true,
                                            flex: 1,
                                            maxLength: 150,
                                        }

                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        labelWidth: 100,
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    title: 'Реквизиты документа, удостоверяющего личность',
                                    items: [
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
                                                    name: 'Series',
                                                    fieldLabel: 'Серия',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    maxLength: 10
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Number',
                                                    fieldLabel: 'Номер',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    maxLength: 10
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'IssueDate',
                                                    labelAlign: 'left',
                                                    allowBlank: true,
                                                    fieldLabel: 'Дата выдачи',
                                                    format: 'd.m.Y',
                                                    labelWidth: 80,
                                                    width: 200
                                                },    

                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Issuer',
                                            fieldLabel: 'Кем выдан',
                                            allowBlank: true,
                                            flex: 1,
                                            maxLength: 100
                                        }
                                    ]
                                }
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
                                    xtype: 'smevsnilsfileinfogrid',
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
                                title: 'Ответ СМЭВ',
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
                                            xtype: 'textfield',
                                            name: 'SNILS',
                                            fieldLabel: 'СНИЛС',
                                            allowBlank: true,
                                            flex: 1,
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