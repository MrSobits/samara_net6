Ext.define('B4.view.smevlivingplace.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.enums.SMEVStayingPlaceDocType',
        'B4.view.smevlivingplace.FileInfoGrid',
        'B4.form.EnumCombo'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    height: 550,
    bodyPadding: 10,
    itemId: 'smevlivingplaceEditWindow',
    title: 'Запрос',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
             items: [
             {
                xtype: 'textfield',
                name: 'Answer',
                fieldLabel: 'Ответ',
                readOnly: true,
                flex: 1,
                align: 'stretch',
                labelAlign: 'right'
             },
             {
                 xtype: 'tabpanel',
                 border: false,
                 flex: 1,
                 defaults: {
                     border: false
                 },
                 items: [
                     {
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
                            defaults: {
                                labelWidth: 250,
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
                                        //     margin: '10 0 5 0',
                                        labelWidth: 120,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'CitizenLastname',
                                            fieldLabel: 'Фамилия',
                                            allowBlank: false,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'CitizenFirstname',
                                            fieldLabel: 'Имя',
                                            allowBlank: false,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'CitizenGivenname',
                                            fieldLabel: 'Отчество',
                                            allowBlank: true,
                                            flex: 1
                                        }
                                    ]
                                },
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
                                            name: 'CitizenBirthday',
                                            fieldLabel: 'Дата рождения',
                                            allowBlank: false,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'CitizenSnils',
                                            fieldLabel: 'СНИЛС',
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            enumName: 'B4.enums.SMEVStayingPlaceDocType',
                                            name: 'DocType',
                                            fieldLabel: 'Тип документа',
                                            allowBlank: false,
                                            flex: 1
                                        }
                                    ]
                                },
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
                                            name: 'DocSerie',
                                            fieldLabel: 'Серия',
                                            allowBlank: false,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'DocNumber',
                                            fieldLabel: 'Номер',
                                            allowBlank: false,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DocIssueDate',
                                            fieldLabel: 'Дата выдачи',
                                            allowBlank: false,
                                            flex: 1
                                        }
                                        //{
                                        //    xtype: 'textfield',
                                        //    name: 'DocCountry',
                                        //    fieldLabel: 'Страна выдачи',
                                        //    allowBlank: false,
                                        //    flex: 1
                                        //}
                                    ]
                                },
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
                                            name: 'RegionCode',
                                            fieldLabel: 'Регион запроса',
                                            allowBlank: false,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'LPlaceRegion',
                                            fieldLabel: 'Регион регистрации',
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'LPlaceDistrict',
                                            fieldLabel: 'Район',
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'LPlaceCity',
                                            fieldLabel: 'Населенный пункт',
                                            allowBlank: true,
                                            flex: 1
                                        }
                                    ]
                                },
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
                                            name: 'LPlaceStreet',
                                            fieldLabel: 'Улица',
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'LPlaceHouse',
                                            fieldLabel: 'Дом',
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'LPlaceBuilding',
                                            fieldLabel: 'Корпус',
                                            allowBlank: true,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'LPlaceFlat',
                                            fieldLabel: 'Квартира',
                                            allowBlank: true,
                                            flex: 1
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
                                items: [
                                {
                                        xtype: 'smevlivingplacefileinfogrid',
                                    flex: 1
                                }
                                ]
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