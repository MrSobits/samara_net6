Ext.define('B4.view.smevmvd.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.MVDTypeAddress',
        'B4.store.dict.RegionCodeMVD',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.SelectField',
        'B4.view.smevmvd.FileInfoGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'smevmvdEditWindow',
    title: 'Запрос в МВД',
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
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                           {
                                               xtype: 'datefield',
                                               name: 'BirthDate',
                                               fieldLabel: 'Дата рождения',
                                               format: 'd.m.Y',
                                               labelWidth: 150,
                                               flex: 0.5,
                                               allowBlank: false
                                           },
                                            {
                                                xtype: 'textfield',
                                                name: 'SNILS',
                                                fieldLabel: 'СНИЛС',
                                                allowBlank: true,
                                                width: 300,
                                                flex: 1,
                                                disabled: false,
                                                editable: true,
                                                maxLength: 20,
                                                regex: /^(\d{11})$/
                                            }
                                    ]
                                },
                                   {
                                       xtype: 'container',
                                       layout: 'hbox',
                                       defaults: {
                                           xtype: 'combobox',
                                           //    margin: '10 0 5 0',
                                           labelWidth: 100,
                                           labelAlign: 'right',
                                           flex: 1
                                       },
                                       items: [
                                               {
                                                   xtype: 'textfield',
                                                   name: 'Surname',
                                                   fieldLabel: 'Фамилия',
                                                   allowBlank: false,
                                                   disabled: false,
                                                   editable: true,
                                                   width: 300,
                                                   maxLength: 50
                                               },
                                               {
                                                   xtype: 'textfield',
                                                   name: 'Name',
                                                   fieldLabel: 'Имя',
                                                   allowBlank: false,
                                                   width: 200,
                                                   disabled: false,
                                                   editable: true,
                                                   maxLength: 50
                                               },
                                                 {
                                                     xtype: 'textfield',
                                                     name: 'PatronymicName',
                                                     width: 300,
                                                     fieldLabel: 'Отчество',
                                                     allowBlank: true,
                                                     disabled: false,
                                                     editable: true,
                                                     maxLength: 50
                                                 },
                                       ]
                                   }
                                   ]
                               },
                               {
                                   xtype: 'fieldset',
                                   defaults: {
                                       labelWidth: 250,
                                       anchor: '100%',
                                       labelAlign: 'right'
                                   },
                                   title: 'Место рождения/регистрации/местанахождения (обязательно)',
                                   items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        //     margin: '10 0 5 0',
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                             {
                                                 xtype: 'combobox',
                                                 name: 'MVDTypeAddressPrimary',
                                                 fieldLabel: 'Тип адреса',
                                                 displayField: 'Display',
                                                 flex: 0.5,
                                                 store: B4.enums.MVDTypeAddress.getStore(),
                                                 valueField: 'Value',
                                                 allowBlank: false,
                                                 editable: false
                                             },
                                           {
                                               xtype: 'b4selectfield',
                                               name: 'RegionCodePrimary',
                                               fieldLabel: 'Регион',
                                               store: 'B4.store.dict.RegionCodeMVD',
                                               editable: false,
                                               flex: 1,
                                               itemId: 'sfRegionCodePrimary',
                                               allowBlank: false,
                                               columns: [
                                                    { text: 'Код', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                                                   { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                                  
                                               ]
                                           }
                                    ]
                                },
                                   {
                                       xtype: 'container',
                                       layout: 'hbox',
                                       defaults: {
                                           xtype: 'combobox',
                                           //    margin: '10 0 5 0',
                                           labelWidth: 100,
                                           labelAlign: 'right',
                                           flex: 1
                                       },
                                       items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'AddressPrimary',
                                                    fieldLabel: 'Адрес (как в паспорте)',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    editable: true,
                                                    width: 300,
                                                    maxLength: 500
                                                }
                                       ]
                                   }
                                   ]
                               },
                               {
                                   xtype: 'fieldset',
                                   defaults: {
                                       labelWidth: 250,
                                       anchor: '100%',
                                       labelAlign: 'right'
                                   },
                                   title: 'Дополнительное место рождения/регистрации/местанахождения (не обязательно)',
                                   items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        //     margin: '10 0 5 0',
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                             {
                                                 xtype: 'combobox',
                                                 name: 'MVDTypeAddressAdditional',
                                                 fieldLabel: 'Тип адреса',
                                                 displayField: 'Display',
                                                 flex: 0.5,
                                                 store: B4.enums.MVDTypeAddress.getStore(),
                                                 valueField: 'Value',
                                                 allowBlank: true,
                                                 editable: false
                                             },
                                           {
                                               xtype: 'b4selectfield',
                                               name: 'RegionCodeAdditional',
                                               fieldLabel: 'Регион',
                                               store: 'B4.store.dict.RegionCodeMVD',
                                               editable: false,
                                               flex: 1,
                                               itemId: 'sfRegionCodePrimary',
                                               allowBlank: true,
                                               columns: [
                                                    { text: 'Код', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                                                   { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }

                                               ]
                                           }
                                    ]
                                },
                                   {
                                       xtype: 'container',
                                       layout: 'hbox',
                                       defaults: {
                                           xtype: 'combobox',
                                           //    margin: '10 0 5 0',
                                           labelWidth: 100,
                                           labelAlign: 'right',
                                           flex: 1
                                       },
                                       items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'AddressAdditional',
                                                    fieldLabel: 'Адрес (как в паспорте)',
                                                    allowBlank: true,
                                                    disabled: false,
                                                    editable: true,
                                                    width: 300,
                                                    maxLength: 500
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
                                        xtype: 'smevmvdfileinfogrid',
                                        flex: 1
                                    }
                                 ]
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
                        items: [
                              {
                                  xtype: 'textarea',
                                  name: 'Answer',
                                  fieldLabel: 'Ответ на запрос',
                                  allowBlank: true,
                                  disabled: false,
                                  editable: false,
                                  maxLength: 1000
                            },
                            {
                                xtype: 'textarea',
                                name: 'AnswerInfo',
                                fieldLabel: 'Информация',
                                allowBlank: true,
                                disabled: false,
                                editable: false,
                                height: 150,
                                maxLength: 5000
                            },
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
                                },
                                {
                                    xtype: 'gkhbuttonprint'
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