Ext.define('B4.view.riskorientedmethod.ROMCategoryEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.romcategoryeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 1000,
    minWidth: 520,
    minHeight: 310,
    height: 510,
    bodyPadding: 5,
    itemId: 'romcategoryeditwindow',
    title: 'Форма расчета категории риска',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.store.riskorientedmethod.ListManOrg',
        'B4.ux.button.Save',
        'B4.enums.KindKND',
        'B4.enums.YearEnums',
        'B4.store.riskorientedmethod.ROMCategory',
        'B4.view.riskorientedmethod.VpResolutionGrid',
        'B4.view.riskorientedmethod.VnResolutionGrid',
        'B4.view.riskorientedmethod.VprResolutionGrid',
        'B4.view.riskorientedmethod.VprPrescriptionGrid',
        'B4.view.riskorientedmethod.ROMCategoryMKDGrid',
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.RiskCategory',
        'B4.store.dict.Inspector'
    ],

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
                          labelWidth: 170,
                          labelAlign: 'right',
                          flex: 1
                      },
                      items: [
                         {
                             xtype: 'combobox',
                             name: 'KindKND',
                             fieldLabel: 'Вид КНД',
                             itemId: 'sfKindKND',
                             displayField: 'Display',
                             store: B4.enums.KindKND.getStore(),
                             valueField: 'Value',
                             allowBlank: false,
                             editable: false
                         },
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
                              xtype: 'datefield',
                              name: 'CalcDate',
                              fieldLabel: 'Дата расчета',
                              format: 'd.m.Y',
                              labelWidth: 150,
                              allowBlank: false
                          }
                      ]
                  },               
              {
                  xtype: 'b4selectfield',
                  name: 'Contragent',
                  fieldLabel: 'Управляющая организация',
                  store: 'B4.store.riskorientedmethod.ListManOrg',
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
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    name: 'Inspector',
                    editable: false,
                    disabled: true,
                    fieldLabel: 'Оператор',
                    textProperty: 'Fio',
                    isGetOnlyIdProperty: true,
                    columns: [
                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Описание', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'numberfield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'Vp',
                            fieldLabel: 'Vп',
                            itemId: 'dfVp',
                            maxLength: 20
                        },
                        {
                            xtype: 'numberfield',
                            name: 'Vn',
                            itemId: 'dfVn',
                            fieldLabel: 'Vн',
                            maxLength: 20
                        },
                        {
                            xtype: 'numberfield',
                            name: 'Vpr',
                            itemId: 'dfVpr',
                            fieldLabel: 'Vпр',
                            maxLength: 20
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'numberfield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'MonthCount',
                            fieldLabel: 'R',
                            itemId: 'dfR',
                            maxLength: 20
                        },
                        {
                            xtype: 'numberfield',
                            name: 'MkdAreaTotal',
                            fieldLabel: 'S',
                            itemId: 'dfS',
                            maxLength: 20
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'numberfield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'Result',
                            fieldLabel: 'Результат',
                            itemId: 'dfResult',
                            maxLength: 20
                        },
                        {
                            xtype: 'combobox',
                            name: 'RiskCategory',
                            fieldLabel: 'Категория',
                            displayField: 'Display',
                            itemId: 'dfRiskCategory',
                            store: B4.enums.RiskCategory.getStore(),
                            valueField: 'Value',
                            allowBlank: true,
                            editable: false
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
                        xtype: 'romcategorymkdgrid',
                        flex: 1
                    },
                    {
                        xtype: 'vpresolutiongrid',
                        flex: 1
                    },
                    {
                        xtype: 'vnresolutiongrid',
                        flex: 1
                    },
                    {
                        xtype: 'vprresolutiongrid',
                        flex: 1
                    },
                    {
                        xtype: 'vprprescriptiongrid',
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Пересчитать',
                                    tooltip: 'Пересчитать',
                                    iconCls: 'icon-accept',
                                //    action: 'romExecute',
                                    itemId: 'romExecuteButton'
                                }
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