Ext.define('B4.view.payreg.RequestsGrid', {
    extend: 'B4.ux.grid.Panel',    
    alias: 'widget.payregrequestsgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'Ext.ux.grid.FilterBar',
        'B4.enums.RequestState',
        'B4.enums.GisGmpChargeType'
    ],

   // title: 'Запросы оплат',
    store: 'smev.PayRegRequests',
    closable: false,
    layout: 'fit',
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                //{
                  //  xtype: 'b4editcolumn',
                    //scope: me
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',                    
                    text: 'Номер запроса',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MessageId',
                    flex: 2,
                    text: 'Номер в СМЭВ',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                //{
                //    xtype: 'b4enumcolumn',
                //    enumName: 'B4.enums.GisGmpChargeType',
                //    dataIndex: 'PayRegChargeType',
                //    text: 'Тип запроса',
                //    flex: 1, 
                //    filter: true,
                //},
                {
                    xtype: 'datecolumn',
                    dataIndex: 'GetPaymentsStartDate',
                    flex: 1,
                    text: 'Дата, с',
                    format: 'd.m.Y H:i',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'GetPaymentsEndDate',
                    flex: 1,
                    text: 'Дата, по',
                    format: 'd.m.Y H:i',
                    filter: {
                        xtype: 'textfield',
                    },
                },
               {
                   xtype: 'gridcolumn',
                   dataIndex: 'Inspector',
                   flex: 1,
                   text: 'Инспектор',
                   filter: {
                       xtype: 'textfield',
                   },
               },
               
               {
                   xtype: 'b4enumcolumn',
                   enumName: 'B4.enums.RequestState',
                   filter: true,
                   text: 'Состояние запроса',
                   dataIndex: 'RequestState',
                   flex: 2
               },
{
                   xtype: 'gridcolumn',
                   dataIndex: 'Answer',
                   flex: 3,
                   text: 'Ответ СМЭВ',
                   filter: {
                       xtype: 'textfield',
                   },
               },
               
                
                {
                    xtype: 'b4deletecolumn',
                    scope: me,
                    hidden: true
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: true,
                    showClearAllButton: true,
                    pluginId: 'headerFilter'
                }
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
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