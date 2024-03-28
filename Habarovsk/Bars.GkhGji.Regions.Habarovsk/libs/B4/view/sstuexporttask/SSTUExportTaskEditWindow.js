Ext.define('B4.view.sstuexporttask.SSTUExportTaskEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.municipality.Grid',
        'B4.store.CreditOrg',
     //   'B4.form.FiasSelectAddress',
        'B4.enums.RISExportState',
          'B4.enums.SSTUSource',
        'B4.form.SelectField',
        'B4.view.sstuexporttask.SSTUExportTaskAppealGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'sSTUExportTaskEditWindow',
    title: 'Задача по экспорту в ССТУ',
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
                     xtype: 'combobox',
                     name: 'SSTUSource',
                     fieldLabel: 'Источник поступления',
                     displayField: 'Display',
                     store: B4.enums.SSTUSource.getStore(),
                     valueField: 'Value',
                     allowBlank: false,
                     editable: false
                 },
                     {
                         xtype: 'checkbox',
                         itemId: 'cbExportExported',
                         name: 'ExportExported',
                         fieldLabel: 'Только выгруженные ранее',
                         labelWidth: 170
                       //  flex: 0.1
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
                             name: 'Operator',
                             fieldLabel: 'Пользователь',
                             allowBlank: true,
                             disabled: true,
                             editable: false,
                             maxLength: 500
                         },
                        {
                            xtype: 'textfield',
                            name: 'TaskDate',
                            fieldLabel: 'Дата экспорта',
                            allowBlank: true,
                            disabled: true,
                            editable: false,
                            maxLength: 500
                        },
                      
                     ]
                 },
                      {
                          xtype: 'combobox',
                          name: 'SSTUExportState',
                          itemId: 'cbCheked',
                          fieldLabel: 'Состояние выгрузки',
                          displayField: 'Display',
                          store: B4.enums.RISExportState.getStore(),
                          valueField: 'Value',
                          allowBlank: true,
                          disabled: true,
                          editable: false
                      },
                // {
                //     xtype: 'textfield',
                //     name: 'ShortName',
                //     fieldLabel: 'Краткое наименование',
                //     maxLength: 300
                // },
                //{
                //    xtype: 'b4selectfield',
                //    name: 'Municipality',
                //    fieldLabel: 'Муниципальный район',
                //    anchor: '100%',
                //    store: 'B4.store.dict.Municipality',
                //    editable: false,
                //    allowBlank: false
                //},
                // {
                //     xtype: 'container',
                //     layout: 'hbox',
                //     defaults: {
                //         xtype: 'textfield',
                //         labelWidth: 170,
                //         labelAlign: 'right',
                //         flex: 1
                //     },
                //     items: [
                //          {
                //              xtype: 'textfield',
                //              name: 'Town',
                //              fieldLabel: 'Населенный пункт',
                //              maxLength: 100
                //          },
                //         {
                //             xtype: 'textfield',
                //             name: 'Street',
                //             fieldLabel: 'Улица',
                //             maxLength: 100
                //         }
                //     ]
                // },
                //   {
                //       xtype: 'b4selectfield',
                //       store: 'B4.store.CreditOrg',
                //       textProperty: 'Name',
                //       name: 'CreditOrg',
                //       fieldLabel: 'Банк',
                //       editable: false,
                //       columns: [
                //           { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                //           { header: 'ОКТМО', xtype: 'gridcolumn', dataIndex: 'Oktmo', flex: 1, filter: { xtype: 'textfield' } }
                //       ],
                //       allowBlank: false
                //   },
                //    {
                //        xtype: 'container',
                //        layout: 'hbox',
                //        defaults: {
                //            xtype: 'textfield',
                //            labelWidth: 170,
                //            labelAlign: 'right',
                //            flex: 1
                //        },
                //        items: [
                //            {
                //                xtype: 'textfield',
                //                name: 'BankAccount',
                //                fieldLabel: 'Расчетный счет',
                //                maxLength: 100
                //            },
                //            {
                //                xtype: 'textfield',
                //                name: 'KBK',
                //                fieldLabel: 'КБК',
                //                maxLength: 100
                //            }
                //        ]
                //    }
                  {
                      xtype: 'tabpanel',
                      border: false,
                      flex: 1,
                      defaults: {
                          border: false
                      },
                      items: [
                         {
                             xtype: 'sstuexporttaskappealgrid',
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