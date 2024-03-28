Ext.define('B4.view.sstuexporttask.SSTUExportTaskGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.sstuexporttaskgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.SSTUExportState'
    ],

    title: 'Реестр задач по экспорту в ССТУ',
    store: 'sstuexporttask.SSTUExportTask',
  //  itemId: 'sSTUExportTaskGrid',
    closable: true,

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
                      dataIndex: 'Id',
                      flex: 1,
                      text: 'Задача'
                  },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Operator',
                    flex: 1,
                    text: 'Оператор'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'TaskDate',
                    flex: 1,
                    text: 'Срок исполнения',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SSTUExportState',
                    text: 'Состояние выгрузки',
                    flex: 1,
                    renderer: function (val) {
                        return B4.enums.SSTUExportState.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'combobox',
                        store: B4.enums.SSTUExportState.getItemsWithEmpty([null, '-']),
                        operand: CondExpr.operands.eq,
                        editable: false
                    }
                },
                 {
                     xtype: 'actioncolumn',
                     text: 'Операция',
                     action: 'openpassport',
                     width: 50,
                     items: [{
                         tooltip: 'Запустить выгрузку',
                     //    iconCls: 'icon-fill-button',
                         icon: B4.Url.content('content/img/gkh_arrow_ns.png')
                     }]
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileInfo',
                    width: 100,
                    text: 'Отчет',
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
            viewConfig: {
                loadMask: true
            },
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
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