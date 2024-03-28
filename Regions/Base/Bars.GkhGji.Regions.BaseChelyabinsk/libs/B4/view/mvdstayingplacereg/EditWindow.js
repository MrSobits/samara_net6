Ext.define('B4.view.mvdstayingplacereg.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.mvdlivingplacereg.FileInfoGrid',
        'B4.view.mvdlivingplacereg.AnswerGrid',
        'B4.ux.form.field.TabularTextArea',
        'B4.form.EnumCombo'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'mvdstayingplaceregEditWindow',
    title: 'Сведения о регистрации по месту жительства граждан РФ',
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
                    items: [                        
                        {
                            xtype: 'fieldset',
                            itemId: 'byFio',
                            defaults: {
                                labelWidth: 100,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Реквизиты физического лица',
                            items:
                            [                                
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                  
                                    defaults: {
                                        xtype: 'combobox',
                                        margin: '5 0 0 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Surname',
                                            fieldLabel: 'Фамилия',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'tfSurname'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Name',
                                            fieldLabel: 'Имя',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'tfName'
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
                                            itemId: 'tfPatronymic'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'BirthDate',
                                            fieldLabel: 'Дата рождения',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfBirthDate'
                                        }   

                                    ]
                                }                                                              
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            itemId: 'byPassport',
                            defaults: {
                                labelWidth: 100,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Реквизиты парспорта РФ',
                            items:
                            [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        margin: '5 0 0 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'PassportSeries',
                                            fieldLabel: 'Серия',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 4,
                                            itemId: 'tfPassportSeries'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'PassportNumber',
                                            fieldLabel: 'Номер',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 6,
                                            itemId: 'tfPassportNumber'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'IssueDate',
                                            fieldLabel: 'Дата выдачи',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfIssueDate'
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
                                xtype: 'mvdstayingplaceregfileinfogrid',
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
                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                        title: 'Ответ',
                        border: false,
                        bodyPadding: 10,
                        items: [
                            
                            {
                                xtype: 'container',
                                layout: {
                                    type: 'vbox',
                                    align: 'stretch'
                                },
                                defaults: {
                                    xtype: 'combobox',
                                    margin: '0 0 5 0',                                    
                                    labelAlign: 'right',
                                },
                                items: [
                                    {
                                        xtype: 'textarea',
                                        name: 'Answer',
                                        fieldLabel: 'Ответ на запрос',
                                        allowBlank: true,
                                        disabled: false,
                                        flex: 1,
                                        editable: false,
                                        maxLength: 1000
                                    },
                                    {
                                        xtype: 'htmleditor',
                                        name: 'AnswerInfo',
                                        fieldLabel: 'Информация',
                                        flex: 1
                                    }
                                    //{
                                    //    xtype: 'tabtextarea',
                                    //    name: 'AnswerInfo',
                                    //    fieldLabel: 'Информация',
                                    //    allowBlank: true,
                                    //    flex: 1,
                                    //    disabled: false,
                                    //    editable: false,
                                    //    height: 150,
                                    //    maxLength: 5000
                                    //}
                                ]
                            },
                            //{
                            //    xtype: 'mvdpassportanswergrid',
                            //        flex: 1
                                
                            //}                            
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