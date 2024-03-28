Ext.define('B4.view.manorglicense.LicenseNotificationGisEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.manorglicensenotificationgiseditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 1000,
    minWidth: 1000,
    minHeight: 500,
    maxHeight: 600,
    bodyPadding: 5,
    title: 'Окно редактирования уведомления',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.store.LocalGovernment',
        'B4.store.manorglicense.ListManOrg',
        'B4.enums.OMSNoticeResult',
        'B4.view.localgov.Grid',
        //'B4.view.manorglicense.ListManOrg',
        'B4.store.manorglicense.LicenseNotificationGis',
        'B4.store.Contragent',
        'B4.view.contragent.Grid',
        'B4.model.LocalGovernment',
         //'B4.store.manorglicense.CourtDecisionGis',
        //'B4.view.manorglicense.CourtDecisionGis',
        'B4.view.Control.GkhButtonPrint'
    ],

    initComponent: function () {
        var me = this;
        debugger;
        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 170
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'LicenseNotificationNumber',
                            fieldLabel: 'Номер извещения',
                            allowBlank: false,
                            flex: 1
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.LocalGovernment',
                            textProperty: 'ContragentName',
                            name: 'LocalGovernment',
                            fieldLabel: 'ОМС',
                            editable: false,
                            columns: [
                                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' } },
                                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            allowBlank: false
                        },
                        //{
                        //    xtype: 'b4selectfield',
                        //    labelAlign: 'right',
                        //    labelWidth: 50,
                        //    name: 'LocalGovernment',
                        //    fieldLabel: 'ОМС',
                        //    store: 'B4.store.LocalGovernment',
                        //    readOnly: false,
                        //    editable: false,
                        //    flex: 1
                        //},
                        {
                            xtype: 'component',
                            width: 120
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
                       padding: '0 0 5 0',
                       labelAlign: 'right'
                   },
                   items: [
                       {
                           xtype: 'datefield',
                           name: 'NoticeOMSSendDate',
                           labelWidth: 200,
                           width: 300,
                           fieldLabel: 'Дата направления извещения в ОМС',
                           format: 'd.m.Y'
                       },
                        {
                            xtype: 'textfield',
                            name: 'RegistredNumber',
                            fieldLabel: 'Номер регистрации',
                            allowBlank: true
                        },
                         {
                             xtype: 'datefield',
                             name: 'NoticeResivedDate',
                             labelWidth: 100,
                             width: 275,
                             allowBlank: true,
                             fieldLabel: 'Дата получения',
                             format: 'd.m.Y'
                         },
                       {
                           xtype: 'component',
                           width: 120
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
                        padding: '0 0 5 0',
                        labelAlign: 'right'
                    },
                    items: [
                       {
                           xtype: 'combobox',
                           labelWidth: 90,
                           editable: false,
                           fieldLabel: 'Результат',
                           store: B4.enums.OMSNoticeResult.getStore(),
                           displayField: 'Display',
                           allowBlank: true,
                           flex: 1,
                           valueField: 'Value',
                           name: 'OMSNoticeResult'
                       },
                        {
                            xtype: 'textfield',
                            name: 'OMSNoticeResultNumber',
                            fieldLabel: 'Номер решения',
                            allowBlank: true,
                            flex: 1
                        },
                         {
                             xtype: 'datefield',
                             name: 'OMSNoticeResultDate',
                             labelWidth: 100,
                             width: 275,
                             allowBlank: true,
                             fieldLabel: 'Дата решения',
                             format: 'd.m.Y'
                         },
                        {
                            xtype: 'component',
                            width: 120
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
                         padding: '0 0 5 0',
                         labelAlign: 'right'
                     },
                     items: [
                       {
                           xtype: 'textarea',
                           name: 'Comment',
                           fieldLabel: 'Комментарий',
                           maxLength: 2000,
                           flex: 1,
                           allowBlank: true
                       },
                         {
                             xtype: 'component',
                             width: 120
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
                          padding: '0 0 5 0',
                          labelAlign: 'right'
                      },
                      items: [
                        {
                            xtype: 'b4selectfield',
                            labelAlign: 'right',
                            labelWidth: 200,
                            name: 'Contragent',
                            fieldLabel: 'Управляющая организация',
                            store: 'B4.store.manorglicense.ListManOrg',
                            readOnly: false,
                            flex: 1
                        },
                         {
                             xtype: 'datefield',
                             name: 'MoDateStart',
                             labelWidth: 100,
                             width: 275,
                             allowBlank: true,
                             fieldLabel: 'Дата начала управления',
                             format: 'd.m.Y'
                         },
                          {
                              xtype: 'component',
                              width: 120
                          }
                      ]
                  },
                  //{
                  //    xtype: 'tabpanel',
                  //    border: false,
                  //    flex: 1,
                  //    defaults: {
                  //        border: false
                  //    },
                  //    items: [
                  //       {
                  //           xtype: 'manorglicenscourtdecisiongisgrid',
                  //           flex: 1
                  //       }
                  //    ]
                  //}
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            items: [                              
                                {
                                    xtype: 'gkhbuttonprint'
                                }
                            ]
                        },     
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});