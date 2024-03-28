//ToDo данный js перекрыт в связи с тем что понадобилось в ННовгород добавить для всех сонований поле Ликвидацию ЮЛ в котором
//ToDo при change поля Контрагент срабатывает получение ликвидации и вывода информации
Ext.define('B4.view.basestatement.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    itemId: 'baseStatementEditPanel',
    title: 'Проверка по обращению граждан',
    trackResetOnLoad: true,
    autoScroll: true,
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 900,
    minWidth: 750,
    height: 500,
    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.RevenueSourceGji',
        'B4.store.dict.RevenueFormGji',

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
            items: [
                 {
                     xtype: 'panel',
                     layout: 'form',
                     height: 125,
                     split: false,
                     collapsible: false,
                     bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
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
                                  xtype: 'combobox', editable: false,
                                  displayField: 'Display',
                                  valueField: 'Value',
                                  flex: 1
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
                              xtype: 'container',
                              padding: '0 0 5 0',
                              defaults: {
                                  labelWidth: 150,
                                  labelAlign: 'right',
                                  flex: 1
                              },
                              layout: 'hbox',
                              items: [
                                  {
                                      xtype: 'b4selectfield',
                                      name: 'Contragent',
                                      textProperty: 'ShortName',
                                      fieldLabel: 'Юридическое лицо',
                                      editable: false,
                                      itemId: 'sfContragent',
                                      store: 'B4.store.Contragent',
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
                                      xtype: 'textfield',
                                      name: 'ActivityInfo',
                                      fieldLabel: 'Ликвидация ЮЛ',
                                      readOnly: true
                                  }
                              ]
                          },
                          {
                              xtype: 'textfield',
                              labelAlign: 'right',
                              labelWidth: 150,
                              name: 'PhysicalPerson',
                              fieldLabel: 'ФИО',
                              itemId: 'tfPhysicalPerson'
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
                                       xtype: 'gkhtriggerfield',
                                       name: 'appealCitizens',
                                       itemId: 'trigfAppealCitizens',
                                       fieldLabel: 'Обращение(я)'
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
                 {
                     xtype: 'tabpanel',
                     border: false,
                     flex: 1,
                     defaults: {
                         border: false
                     },
                     items: [
                         {
                             xtype: 'baseStatementRealityObjGrid',
                             bodyStyle: 'backrgound-color:transparent;'
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
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Отменить',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
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