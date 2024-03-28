Ext.define('B4.view.romcalctask.ROMCalcTaskGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.romcalctaskgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.KindKND',
        'B4.enums.YearEnums'
    ],

    title: 'Реестр задач по расчету категории риска',
    store: 'romcalctask.ROMCalcTask',
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
                      flex: 0.5,
                      text: 'Задача'
                  },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YearEnums',
                    text: 'Год',
                    flex: 0.5,
                    renderer: function (val) {
                        return B4.enums.YearEnums.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'combobox',
                        store: B4.enums.YearEnums.getItemsWithEmpty([null, '-']),
                        operand: CondExpr.operands.eq,
                        editable: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KindKND',
                    text: 'Вид КНД',
                    flex: 1,
                    renderer: function (val) {
                        return B4.enums.KindKND.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'combobox',
                        store: B4.enums.KindKND.getItemsWithEmpty([null, '-']),
                        operand: CondExpr.operands.eq,
                        editable: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    flex: 1,
                    text: 'Инспектор'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'TaskDate',
                    flex: 0.5,
                    text: 'Дата задачи',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CalcDate',
                    flex: 0.5,
                    text: 'Расчет на дату',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CalcState',
                    text: 'Состояние расчета',
                    flex: 1
                },
             
                 {
                     xtype: 'actioncolumn',
                     text: 'Операция',
                     action: 'openpassport',
                     width: 50,
                     items: [{
                         tooltip: 'Запустить расчет',
                         iconCls: 'icon-fill-button',
                         icon: B4.Url.content('content/img/btnRasch.png')
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