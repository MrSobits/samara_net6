Ext.define('B4.view.rosregextract.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.Panel',
        'B4.ux.button.ChangeValue',
        'B4.form.SelectField',
        'B4.view.rosregextract.PersonGrid',
        'B4.view.rosregextract.OrgGrid',
        'B4.view.rosregextract.GovGrid',
        'B4.store.AccountsForComparsion'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 1000,
    minWidth: 520,
    minHeight: 310,
    height: 510,
    bodyPadding: 5,
    itemId: 'rosregextractEditWindow',
    title: 'Просмотр выписки',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 100,
                        margin: '5 0 5 0',
                        labelAlign: 'right',
                        //    flex: 1
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Room_id',
                            fieldLabel: 'Лицевой счет',
                            store: 'B4.store.AccountsForComparsion',
                            editable: false,
                            flex: 1,
                            textProperty: 'Address',
                            itemId: 'sfPersAcc',
                            allowBlank: true,
                            columns: [
                                 {
                                     text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },                               
                                { text: 'Собственник', dataIndex: 'OwnerName', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Помещение', dataIndex: 'CnumRoom', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Площадь', dataIndex: 'AreaRoom', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Доля', dataIndex: 'AreaShare', flex: 1, filter: { xtype: 'textfield' } },
                            ]
                        }
                    ]
                },
                  {
                      xtype: 'container',
                      layout: 'hbox',
                      defaults: {
                          xtype: 'datefield',
                          labelWidth: 100,
                          margin: '5 0 5 0',
                          labelAlign: 'right',
                      //    flex: 1
                      },
                      items: [
                          
                          {
                              xtype: 'textfield',
                              name: 'Desc_ID_Object',
                              flex: 1,
                              fieldLabel: 'Идентификатор',
                          },
                         {
                             xtype: 'textfield',
                             name: 'Desc_CadastralNumber',
                             flex: 1,
                             fieldLabel: 'Кадастровый номер',
                         },
                           {
                               xtype: 'textfield',
                               name: 'Desc_ObjectTypeText',
                               flex: 1,
                               fieldLabel: 'Тип объекта',
                           }
                      ]
                  },
                  {
                      xtype: 'container',
                      layout: 'hbox',
                      defaults: {
                          xtype: 'datefield',
                          labelWidth: 100,
                          margin: '5 0 5 0',
                          labelAlign: 'right',
                          //    flex: 1
                      },
                      items: [
                          {
                              xtype: 'textfield',
                              name: 'Reg_RegDate',
                              flex: 1,
                              fieldLabel: 'Дата регистрации права',
                          },
                         {
                             xtype: 'textfield',
                             name: 'Reg_RegNumber',
                             flex: 1,
                             fieldLabel: 'Номер права',
                         }
                           
                      ]
                  },
                  {
                      xtype: 'container',
                      layout: 'hbox',
                      defaults: {
                          xtype: 'datefield',
                          labelWidth: 100,
                          margin: '5 0 5 0',
                          labelAlign: 'right',
                          //    flex: 1
                      },
                      items: [
                         
                           {
                               xtype: 'textfield',
                               name: 'Desc_AreaText',
                               flex: 0.5,
                               fieldLabel: 'Площадь',
                           },
                            {
                                xtype: 'textfield',
                                name: 'Desc_AddressContent',
                                flex: 1,
                                fieldLabel: 'Полный адрес',
                            }
                      ]
                  },
                  {
                      xtype: 'container',
                      layout: 'hbox',
                      defaults: {
                          xtype: 'datefield',
                          labelWidth: 100,
                          margin: '5 0 5 0',
                          labelAlign: 'right',
                          //    flex: 1
                      },
                      items: [
                          {
                              xtype: 'textfield',
                              name: 'Desc_OKATO',
                              flex: 0.5,
                              fieldLabel: 'ОКАТО',
                          },
                         {
                             xtype: 'textfield',
                             name: 'Desc_RegionName',
                             flex: 1,
                             fieldLabel: 'Регион',
                         },
                           {
                               xtype: 'textfield',
                               name: 'Desc_CityName',
                               flex: 1,
                               fieldLabel: 'Населенный пункт',
                           },
                           {
                               xtype: 'textfield',
                               name: 'Desc_Urban_District',
                               flex: 1,
                               fieldLabel: 'Территория',
                           }
                      ]
                  },
                   {
                       xtype: 'container',
                       layout: 'hbox',
                       defaults: {
                           xtype: 'datefield',
                           labelWidth: 100,
                           margin: '5 0 5 0',
                           labelAlign: 'right',
                           //    flex: 1
                       },
                       items: [
                           {
                               xtype: 'textfield',
                               name: 'Desc_Locality',
                               flex: 1,
                               fieldLabel: 'Регион',
                           },
                          {
                              xtype: 'textfield',
                              name: 'Desc_StreetName',
                              flex: 1,
                              fieldLabel: 'Улица',
                          },
                            {
                                xtype: 'textfield',
                                name: 'Desc_Level1Name',
                                flex: 0.5,
                                fieldLabel: 'Дом',
                            },
                            {
                                xtype: 'textfield',
                                name: 'Desc_ApartmentName',
                                flex: 0.5,
                                fieldLabel: 'Квартира',
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
                           xtype: 'rosregextractpersongrid',
                           flex: 1
                       },
                        {
                            xtype: 'rosregextractorggrid',
                            flex: 1
                        },
                        {
                            xtype: 'rosregextractgovgrid',
                            flex: 1
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-page',
                                    itemId: 'PrintExtract',
                                    action: 'PrintExtract',
                                    href: '/ExtractPrinter/PrintExtractForDescription/?id=',
                                    text: 'Выписка'
                                },
                                {
                                    xtype: 'changevalbtn',
                                    itemId: 'ChangeRoomAreaButton',
                                    className: 'Room',
                                    propertyName: 'Area',
                                    entityId: 0,
                                    text: 'Смена площади комнаты',
                                    margins: '0 0 5 0',
                                    width: 160,
                                    maxWidth: 160
                                },
                                {
                                    xtype: 'button',
                                    hidden: true,
                                    iconCls: 'icon-accept',
                                    itemId: 'ChangeFIO',
                                    action: 'ChangeFIO',
                                    text: 'Обновить ФИО'
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