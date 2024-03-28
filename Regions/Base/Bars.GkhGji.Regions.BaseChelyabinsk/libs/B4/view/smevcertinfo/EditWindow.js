Ext.define('B4.view.smevcertinfo.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.smevcertinfo.FileInfoGrid',
        'B4.enums.RequestType',
        'B4.form.FileField'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'smevCertInfoEditWindow',
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
                                labelWidth: 120,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Реквизиты объекта запроса',
                                 items: [
                                     {
                                         xtype: 'b4filefield',
                                         name: 'FileInfo',
                                         fieldLabel: 'Сертификат',
                                         flex: 1,
                                         allowBlank: false
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
                                        xtype: 'smevcertinfofileinfogrid',
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
                                      xtype: 'smevcertinfofileinfogrid',
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