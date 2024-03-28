Ext.define('B4.view.romcalctask.ROMCalcTaskEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.KindKND',
        'B4.enums.YearEnums',
        'B4.form.SelectField',
        'B4.view.romcalctask.ROMCalcTaskManOrgGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'rOMCalcTaskEditWindow',
    title: 'Задача по массовому расчету категории риска',
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
                      xtype: 'container',
                      layout: 'hbox',
                      defaults: {
                          xtype: 'combobox',
                          labelWidth: 170,
                          labelAlign: 'right',
                          flex: 1
                      },
                      items: [
                           {
                               xtype: 'combobox',
                               name: 'YearEnums',
                               fieldLabel: 'Год расчета',
                               displayField: 'Display',
                               store: B4.enums.YearEnums.getStore(),
                               valueField: 'Value',
                               allowBlank: false,
                               editable: false
                           },
                         {
                             xtype: 'combobox',
                             name: 'KindKND',
                             fieldLabel: 'Вид КНД',
                             displayField: 'Display',
                             store: B4.enums.KindKND.getStore(),
                             valueField: 'Value',
                             allowBlank: false,
                             editable: false
                         },
                          {
                              xtype: 'datefield',
                              name: 'CalcDate',
                              fieldLabel: 'Расчет на дату',
                              format: 'd.m.Y',
                              labelWidth: 150,
                              allowBlank: false
                          }
                      ]
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
                             xtype: 'textfield',
                             name: 'Inspector',
                             fieldLabel: 'Инспектор',
                             allowBlank: true,
                             disabled: true,
                             editable: false,
                             maxLength: 500
                         },
                        {
                            xtype: 'textfield',
                            name: 'TaskDate',
                            fieldLabel: 'Дата задачи',
                            allowBlank: true,
                            disabled: true,
                            editable: false,
                            maxLength: 500
                        },
                      
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
                             xtype: 'romcalctaskmanorggrid',
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