Ext.define('B4.view.manorglicense.LicensePrescriptionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.form.GridStateColumn',
        'B4.enums.YesNoNotSet',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.manorglicense.LicensePrescription'
    ],

    title: 'Постановления',
    store: 'manorglicense.LicensePrescription',
    alias: 'widget.licenseprescriptiongrid',
    closable: false,
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
                    dataIndex: 'DocumentNumber',
                    text: 'Номер постановления',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата постановления',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    flex: 0.5
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ActualDate',
                    text: 'Дата вступления в силу',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    flex: 0.5
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ArticleLawGji',
                    flex: 1,
                    text: 'Статья закона',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                  {
                      xtype: 'gridcolumn',
                      dataIndex: 'SanctionGji',
                      flex: 1,
                      text: 'Вид санкции',
                      filter: {
                          xtype: 'textfield'
                      }
                  },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Penalty',
                    flex: 0.5,
                    text: 'Сумма штрафа',
                    filter: {
                        xtype: 'textfield'
                    }
                  },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    filter: true,
                    header: 'Оспорено',
                    dataIndex: 'YesNoNotSet',
                    flex: 3,
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileInfo',
                    flex: 1,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],        
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4addbutton'
                        },
                        {
                            xtype: 'b4updatebutton'
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