Ext.define('B4.view.manorglicense.LicenseGridGis', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.form.GridStateColumn',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.manorglicense.LicenseGis'
    ],

    title: 'Реестр лицензий с домами',
    store: 'manorglicense.LicenseGis',
    alias: 'widget.manorglicensegridgis',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                 {
                     xtype: 'b4editcolumn',
                     scope: me
                 },
                   {
                       xtype: 'gridcolumn',
                       dataIndex: 'mcId',
                       text: 'Ид записи',
                       filter: {
                           xtype: 'numberfield',
                           hideTrigger: true,
                           minValue: 0,
                           operand: CondExpr.operands.eq
                       },
                       width: 100
                 },
                   {
                       xtype: 'gridcolumn',
                       dataIndex: 'StateName',
                       width: 80,
                       text: 'Статус лицензии',
                       filter: {
                           xtype: 'textfield'
                       }
                   },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LicNum',
                    text: 'Номер',
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
                    dataIndex: 'DateIssued',
                    text: 'Дата выдачи',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentMunicipality',
                    width: 160,
                    text: 'Муниципальное образование',
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
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    flex: 1,
                    text: 'Управляющая организация',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                  {
                      xtype: 'gridcolumn',
                      dataIndex: 'Inn',
                      flex: 1,
                      text: 'ИНН Лицензиата',
                      filter: {
                          xtype: 'textfield'
                      }
                  },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ROMunicipality',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'ROAddress',
                     flex: 1,
                     text: 'Адрес МКД',
                     filter: {
                         xtype: 'textfield'
                     }
                 },
                  {
                      xtype: 'datecolumn',
                      dataIndex: 'ManStartDate',
                      text: 'Дата начала управления',
                      format: 'd.m.Y',
                      filter: {
                          xtype: 'datefield',
                          format: 'd.m.Y'
                      },
                      width: 100
                 },
                  {
                      xtype: 'datecolumn',
                      dataIndex: 'ManEndDate',
                      text: 'Дата окончания управления',
                      format: 'd.m.Y',
                      filter: {
                          xtype: 'datefield',
                          format: 'd.m.Y'
                      },
                      width: 100
                  },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'postCount',
                     text: 'Количество постановлений',
                     filter: {
                         xtype: 'numberfield',
                         hideTrigger: true,
                         minValue: 0,
                         operand: CondExpr.operands.eq
                     },
                     width: 100
                 },

            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
            
            getRowClass: function(record) {
                var chekNum = record.get('postCount');
                debugger;
                if (chekNum >= 2) {
                    return 'back-red';
                }

                else if (chekNum == 1) {
                    return 'back-yellow';
                }

                return '';
            }
        },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4updatebutton'
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbShowEnded',
                            checked: false,
                            boxLabel: 'Показывать выведеные из управления'
                        },
                          {
                              xtype: 'checkbox',
                              itemId: 'cbShowOnly2andMore',
                              checked: false,
                              boxLabel: 'Показать МКД с двумя и более постановлениями'
                          },
                          {
                              xtype: 'checkbox',
                              itemId: 'showall',
                              checked: false,
                              boxLabel: 'Не учитывать сроки предписаний'
                          }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });
       
        me.callParent(arguments);
    }
});