Ext.define('B4.view.baselicensereissuance.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.baselicensereissuanceeditpanel',
    closable: true,
    title: 'Проверка лицензиата',
    autoScroll: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.RevenueSourceGji',
        'B4.store.dict.RevenueFormGji',
        'B4.store.licensereissuance.LicenseReissuance',

        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.Control.GkhTriggerField',
        'B4.store.Contragent',

        'B4.enums.PersonInspection',
        'B4.enums.FormCheck',
        'B4.enums.TypeJurPerson',
        'B4.DisposalTextValues',
        'B4.view.GjiDocumentCreateButton'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                height: 700
            },
            items: [
                 {
                     xtype: 'panel',
                     layout: 'form',
                     //height: 500,
                     split: false,
                     collapsible: false,
                     bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6;',
                     border: false,
                     defaults: {
                         anchor: '100%'
                     },
                     items: [
                          {
                              xtype: 'container',
                              layout: 'hbox',
                              items: [
                                  {
                                      xtype: 'textfield',
                                      name: 'InspectionNumber',
                                      itemId: 'tfInspectionNumber',
                                      fieldLabel: 'Номер',
                                      labelAlign: 'right',
                                      labelWidth: 150,
                                      width: 230,
                                      readOnly: true
                                  }
                              ]
                          },
                          {
                              xtype: 'container',
                              padding: '0 0 5 0',
                              defaults: {
                                  labelWidth: 150,
                                  labelAlign: 'right',
                                  xtype: 'combobox',
                                  editable: false,
                                  displayField: 'Display',
                                  valueField: 'Value',
                                  flex: 1,
                                  readOnly: true
                              },
                              layout: 'hbox',
                              items: [
                                  {
                                      xtype: 'b4combobox',
                                      name: 'TypeJurPerson',
                                      fieldLabel: 'Тип юридического лица',
                                      displayField: 'Display',
                                      valueField: 'Id',
                                      itemId: 'cbTypeJurPerson',
                                      editable: false,
                                      storeAutoLoad: true,
                                      url: '/Inspection/ListJurPersonTypes'
                                  },
                                  {
                                      xtype: 'b4combobox',
                                      name: 'PersonInspection',
                                      fieldLabel: 'Объект проверки',
                                      displayField: 'Display',
                                      itemId: 'cbPersonInspection',
                                      editable: false,
                                      storeAutoLoad: true,
                                      valueField: 'Id',
                                      url: '/Inspection/ListPersonInspection'
                                  }
                              ]
                          },
                          {
                              xtype: 'b4selectfield',
                              labelAlign: 'right',
                              labelWidth: 150,
                              name: 'Contragent',
                              textProperty: 'ShortName',
                              fieldLabel: 'Юридическое лицо',
                              editable: false,
                              itemId: 'sfContragent',
                              store: 'B4.store.Contragent',
                              readOnly: true,
                              columns: [
                                  { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
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
                                  { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                              ]
                          },
                          {
                              xtype: 'container',
                              padding: '0 0 5 0',
                              defaults: {
                                  labelWidth: 150,
                                  labelAlign: 'right',
                                  flex: 1,
                                  editable: false
                              },
                              layout: 'hbox',
                              items: [
                                    {
                                        xtype: 'b4selectfield',
                                        name: 'LicenseReissuance',
                                        textProperty: 'RegisterNum',
                                        fieldLabel: 'Обращение',
                                        store: 'B4.store.licensereissuance.LicenseReissuance',
                                        columns: [
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'RegisterNum',
                                                text: 'Номер заявки',
                                                filter: {
                                                    xtype: 'numberfield',
                                                    hideTrigger: true,
                                                    minValue: 0,
                                                    operand: CondExpr.operands.eq
                                                },
                                                width: 100
                                            },
                                            {
                                                xtype: 'datecolumn',
                                                dataIndex: 'ReissuanceDate',
                                                text: 'Дата заявки',
                                                format: 'd.m.Y',
                                                filter: {
                                                    xtype: 'datefield',
                                                    ormat: 'd.m.Y'
                                                },
                                                width: 100
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'Contragent',
                                                flex: 1,
                                                text: 'Управляющая организация',
                                                filter: {
                                                    xtype: 'textfield'
                                                }
                                            }
                                        ],
                                        editable: false
                                    },
                                   {
                                       xtype: 'combobox', editable: false,
                                       name: 'FormCheck',
                                       itemId: 'cbFormCheck',
                                       store: B4.enums.FormCheck.getStore(),
                                       displayField: 'Display',
                                       valueField: 'Value',
                                       fieldLabel: 'Форма проверки'
                                   }
                              ]
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
                 //        {
                 //            xtype: 'baselicensereissuancerealobjgrid',
                 //            bodyStyle: 'backrgound-color:transparent;'
                 //        }
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
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'right',
                                    itemId: 'btnDelete'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
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