Ext.define('B4.view.rosregextract.PersonEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.grid.Panel'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 1000,
    minWidth: 520,
    minHeight: 310,
    height: 510,
    bodyPadding: 5,
    itemId: 'rosregextractPersonEditWindow',
    title: 'Физ лицо',
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
                  //{
                  //    xtype: 'container',
                  //    layout: 'hbox',
                  //    defaults: {
                  //        xtype: 'datefield',
                  //        labelWidth: 100,
                  //        margin: '5 0 5 0',
                  //        labelAlign: 'right',
                  //    //    flex: 1
                  //    },
                  //    items: [
                  //        {
                  //            xtype: 'textfield',
                  //            name: 'Desc_ID_Object',
                  //            flex: 1,
                  //            fieldLabel: 'Идентификатор',
                  //        },
                  //       {
                  //           xtype: 'textfield',
                  //           name: 'Desc_CadastralNumber',
                  //           flex: 1,
                  //           fieldLabel: 'Кадастровый номер',
                  //       },
                  //         {
                  //             xtype: 'textfield',
                  //             name: 'Desc_ObjectTypeText',
                  //             flex: 1,
                  //             fieldLabel: 'Тип объекта',
                  //         }
                  //    ]
                  //},
                  //{
                  //    xtype: 'container',
                  //    layout: 'hbox',
                  //    defaults: {
                  //        xtype: 'datefield',
                  //        labelWidth: 100,
                  //        margin: '5 0 5 0',
                  //        labelAlign: 'right',
                  //        //    flex: 1
                  //    },
                  //    items: [
                  //        {
                  //            xtype: 'textfield',
                  //            name: 'Reg_RegDate',
                  //            flex: 1,
                  //            fieldLabel: 'Дата регистрации права',
                  //        },
                  //       {
                  //           xtype: 'textfield',
                  //           name: 'Reg_RegNumber',
                  //           flex: 1,
                  //           fieldLabel: 'Номер права',
                  //       }
                           
                  //    ]
                  //},
                  //{
                  //    xtype: 'container',
                  //    layout: 'hbox',
                  //    defaults: {
                  //        xtype: 'datefield',
                  //        labelWidth: 100,
                  //        margin: '5 0 5 0',
                  //        labelAlign: 'right',
                  //        //    flex: 1
                  //    },
                  //    items: [
                         
                  //         {
                  //             xtype: 'textfield',
                  //             name: 'Desc_AreaText',
                  //             flex: 0.5,
                  //             fieldLabel: 'Площадь',
                  //         },
                  //          {
                  //              xtype: 'textfield',
                  //              name: 'Desc_AddressContent',
                  //              flex: 1,
                  //              fieldLabel: 'Полный адрес',
                  //          }
                  //    ]
                  //},
                  //{
                  //    xtype: 'container',
                  //    layout: 'hbox',
                  //    defaults: {
                  //        xtype: 'datefield',
                  //        labelWidth: 100,
                  //        margin: '5 0 5 0',
                  //        labelAlign: 'right',
                  //        //    flex: 1
                  //    },
                  //    items: [
                  //        {
                  //            xtype: 'textfield',
                  //            name: 'Desc_OKATO',
                  //            flex: 0.5,
                  //            fieldLabel: 'ОКАТО',
                  //        },
                  //       {
                  //           xtype: 'textfield',
                  //           name: 'Desc_RegionName',
                  //           flex: 1,
                  //           fieldLabel: 'Регион',
                  //       },
                  //         {
                  //             xtype: 'textfield',
                  //             name: 'Desc_CityName',
                  //             flex: 1,
                  //             fieldLabel: 'Населенный пункт',
                  //         },
                  //         {
                  //             xtype: 'textfield',
                  //             name: 'Desc_Urban_District',
                  //             flex: 1,
                  //             fieldLabel: 'Территория',
                  //         }
                  //    ]
                  //},
                  // {
                  //     xtype: 'container',
                  //     layout: 'hbox',
                  //     defaults: {
                  //         xtype: 'datefield',
                  //         labelWidth: 100,
                  //         margin: '5 0 5 0',
                  //         labelAlign: 'right',
                  //         //    flex: 1
                  //     },
                  //     items: [
                  //         {
                  //             xtype: 'textfield',
                  //             name: 'Desc_Locality',
                  //             flex: 1,
                  //             fieldLabel: 'Регион',
                  //         },
                  //        {
                  //            xtype: 'textfield',
                  //            name: 'Desc_Level1Name',
                  //            flex: 1,
                  //            fieldLabel: 'Улица',
                  //        },
                  //          {
                  //              xtype: 'textfield',
                  //              name: 'Desc_Level2Name',
                  //              flex: 0.5,
                  //              fieldLabel: 'Дом',
                  //          },
                  //          {
                  //              xtype: 'textfield',
                  //              name: 'Desc_ApartmentName',
                  //              flex: 0.5,
                  //              fieldLabel: 'Квартира',
                  //          }
                  //     ]
                  // },

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
                                //{
                                //    xtype: 'b4savebutton'
                                //}
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