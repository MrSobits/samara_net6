Ext.define('B4.view.specaccowner.SpecialAccountOwnerEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.municipality.Grid',
        'B4.store.Contragent',
        'B4.enums.OrgStateRole',
        'B4.enums.GroundsTermination',
        'B4.form.SelectField',
        'B4.view.specaccowner.SPAccOwnerRealityObjectGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
  //  layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'specialAccountOwnerEditWindow',
    title: 'Владелец спецсчета',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150
            },
            items: [
                 {
                     xtype: 'combobox',
                     name: 'OrgStateRole',
                     fieldLabel: 'Статус владельца спецсчета',
                     displayField: 'Display',
                     store: B4.enums.OrgStateRole.getStore(),
                     valueField: 'Value',
                     allowBlank: false,
                     editable: false
                 },
                    {
                        xtype: 'b4selectfield',
                        name: 'Contragent',
                        fieldLabel: 'Владелец спецсчета',
                        store: 'B4.store.Contragent',
                        editable: false,
                        itemId: 'sfContragent',
                        allowBlank: false,
                        columns: [
                            { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
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
                            { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                        ]
                    },
                   {
                       xtype: 'textarea',
                       name: 'Description',
                       fieldLabel: 'Описание',
                       maxLength: 5000,
                       flex: 1
                   },
                 {
                     xtype: 'container',
                     layout: 'hbox',
                     defaults: {
                         xtype: 'textfield',
                         labelWidth: 170,
                         labelAlign: 'right',
                         flex: 1
                     },
                     items: [
                         {
                             xtype: 'combobox',
                             name: 'ActivityGroundsTermination',
                             itemId: 'cbCheked',
                             fieldLabel: 'Основание прекращения деятельности',
                             displayField: 'Display',
                             store: B4.enums.GroundsTermination.getStore(),
                             valueField: 'Value',
                             allowBlank: true,
                             disabled: true,
                             editable: false
                         },
                          {
                              xtype: 'datefield',
                              name: 'ActivityDateEnd',
                              fieldLabel: 'Дата прекращения деятельности',
                              format: 'd.m.Y',
                              flex: 0.5,
                              labelWidth: 150,
                              allowBlank: true
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
                              xtype: 'specaccownerrobjectgrid',
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