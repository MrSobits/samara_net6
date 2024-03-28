Ext.define('B4.view.smevegrn.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.RegionCodeMVD',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.SelectField',
        'B4.view.smevegrn.FileInfoGrid',
        'B4.store.dict.EGRNApplicantType',
        'B4.store.dict.EGRNObjectType',
        'B4.enums.RequestType',
        'B4.store.RealityObject',
        'B4.store.smev.SMEVEGRNRoom'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'smevegrnEditWindow',
    title: 'Запрос в ЕГРН',
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
                                             xtype: 'b4selectfield',
                                             name: 'EGRNApplicantType',
                                             fieldLabel: 'Категория заявителя',
                                             store: 'B4.store.dict.EGRNApplicantType',
                                             editable: false,
                                             flex: 1,
                                             itemId: 'sfEGRNApplicantType',
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
                                        //     margin: '10 0 5 0',
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                          {
                                              xtype: 'combobox',
                                              name: 'RequestType',
                                              fieldLabel: 'Тип запроса',
                                              displayField: 'Display',
                                              itemId: 'dfRequestType',
                                              flex: 1,
                                              store: B4.enums.RequestType.getStore(),
                                              valueField: 'Value',
                                              allowBlank: false,
                                              editable: false
                                          }
                                    ]
                                },
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
                                               xtype: 'b4selectfield',
                                               name: 'RegionCode',
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
                                    xtype: 'textfield',
                                    name: 'CadastralNUmber',
                                    fieldLabel: 'Кадастровый номер',
                                    allowBlank: true,
                                    disabled: false,
                                    flex: 1,
                                    editable: true
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'QualityPhone',
                                    fieldLabel: 'Телефон для службы опроса качества',
                                    allowBlank: false,
                                    disabled: false,
                                    editable: true,
                                    width: 300,
                                    maxLength: 50,
                                    regex: /\+(9[976][0-9]|8[987530][0-9]|6[987][0-9]|5[90][0-9]|42[0-9]|3[875][0-9]|2[98654321][0-9]|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1)[0-9]{1,14}/,
                                    regexText: 'Не соответствует формату номера телефона'
                                },
                                //{
                                //    xtype: 'container',
                                //    layout: 'hbox',
                                //    defaults: {
                                //        xtype: 'combobox',
                                //        //    margin: '10 0 5 0',
                                //        labelWidth: 100,
                                //        labelAlign: 'right',
                                //        flex: 1
                                //    },
                                //    items: [
                                //            {
                                //                xtype: 'textfield',
                                //                name: 'PersonSurname',
                                //                fieldLabel: 'Фамилия',
                                //                allowBlank: false,
                                //                disabled: false,
                                //                editable: true,
                                //                width: 300,
                                //                maxLength: 50
                                //            },
                                //            {
                                //                xtype: 'textfield',
                                //                name: 'PersonName',
                                //                fieldLabel: 'Имя',
                                //                allowBlank: false,
                                //                width: 200,
                                //                disabled: false,
                                //                editable: true,
                                //                maxLength: 50
                                //            },
                                //              {
                                //                  xtype: 'textfield',
                                //                  name: 'PersonPatronymic',
                                //                  width: 300,
                                //                  fieldLabel: 'Отчество',
                                //                  allowBlank: true,
                                //                  disabled: false,
                                //                  editable: true,
                                //                  maxLength: 50
                                //              },
                                //    ]
                                //},
                                //{
                                //    xtype: 'container',
                                //    layout: 'hbox',
                                //    defaults: {
                                //        xtype: 'combobox',
                                //        //    margin: '10 0 5 0',
                                //        labelWidth: 100,
                                //        labelAlign: 'right',
                                //        flex: 1
                                //    },
                                //    items: [
                                //            {
                                //                xtype: 'textfield',
                                //                name: 'DocumentSerial',
                                //                fieldLabel: 'Серия паспорта',
                                //                allowBlank: false,
                                //                disabled: false,
                                //                editable: true,
                                //                width: 300,
                                //                maxLength: 50
                                //            },
                                //            {
                                //                xtype: 'textfield',
                                //                name: 'DocumentNumber',
                                //                fieldLabel: 'Номер паспорта',
                                //                allowBlank: false,
                                //                width: 200,
                                //                disabled: false,
                                //                editable: true,
                                //                maxLength: 50
                                //            }
                                //    ]
                                //}
                                   
                             ]
                             },
                             {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 250,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Реквизиты объекта запроса',
                                 items: [
                                     {
                                         xtype: 'b4selectfield',
                                         name: 'EGRNObjectType',
                                         fieldLabel: 'Тип объекта запроса',
                                         store: 'B4.store.dict.EGRNObjectType',
                                         editable: false,
                                         flex: 1,
                                         itemId: 'sfEGRNObjectType',
                                         allowBlank: false,
                                         columns: [
                                             { text: 'Код', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                                             { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }

                                         ]
                                     },
                                     {
                                         xtype: 'b4selectfield',
                                         name: 'RealityObject',
                                         fieldLabel: 'Жилой дом',
                                         textProperty: 'Address',
                                         store: 'B4.store.RealityObject',
                                         editable: false,
                                         flex: 1,
                                         itemId: 'sfRealityObject',
                                         allowBlank: true,
                                         hidden: true,
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
                                     },
                                    {
                                        xtype: 'b4selectfield',
                                        name: 'Room',
                                        fieldLabel: 'Помещение',
                                        textProperty: 'RoomNum',
                                        store: 'B4.store.smev.SMEVEGRNRoom',
                                        editable: false,
                                        flex: 1,
                                        itemId: 'sfRoom',
                                        allowBlank: true,
                                        disabled: true,
                                        hidden: true,
                                        columns: [
                                            {
                                                text: 'Номер помещения',
                                                dataIndex: 'RoomNum',
                                                flex: 1,
                                                filter: { xtype: 'textfield' }
                                            },
                                            {
                                                text: 'Кадастровый номер',
                                                dataIndex: 'CadastralNumber',
                                                flex: 1,
                                                filter: { xtype: 'textfield' }
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
                                    xtype: 'smevegrnfileinfogrid',
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
                                  xtype: 'textfield',
                                  name: 'Answer',
                                  fieldLabel: 'Ответ на запрос',
                                  allowBlank: true,
                                  disabled: false,
                                  flex: 1,
                                  editable: false,
                                  maxLength: 1000
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
                                      xtype: 'smevegrnfileinfogrid',
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