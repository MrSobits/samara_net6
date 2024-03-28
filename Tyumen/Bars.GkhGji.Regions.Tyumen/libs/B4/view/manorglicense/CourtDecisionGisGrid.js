Ext.define('B4.view.manorglicense.CourtDecisionGisGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.manorglicenscourtdecisiongisgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.CourtDecisionType'
    ],
    itemId: 'manorglicenscourtdecisiongisgrid',
    store: 'manorglicense.CourtDecisionGis',
    title: 'Решения судебных участков',
    
    closable: false,

    initComponent: function () {
        var me = this;
        var renderer = function (val, meta, rec) {
            return val;
        };

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                 {
                     xtype: 'b4editcolumn',
                     scope: me
                 },
                  {
                      xtype: 'gridcolumn',
                      dataIndex: 'CourtDecisionType',
                      width: 150,
                      text: 'Признак правомощности решения',
                      renderer: function (val, meta, rec) {
                          val = renderer(val, meta, rec);
                          return B4.enums.CourtDecisionType.displayRenderer(val);
                         
                      }
                  },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'DecisionNumber',
                     text: 'Номер решения',
                     flex: 2,
                     filter: {
                         xtype: 'textfield'
                     },
                     width: 100
                 },
                 {
                     xtype: 'datecolumn',
                     dataIndex: 'DecisionDate',
                     text: 'Дата решения',
                     format: 'd.m.Y',
                     width: 100
                 },
                  {
                      xtype: 'gridcolumn',
                      dataIndex: 'Violation',
                      text: 'Нарушение',
                      flex: 2,
                      filter: {
                          xtype: 'textfield'
                      },
                      width: 100
                  },
                   {
                       xtype: 'gridcolumn',
                       dataIndex: 'Inspector',
                       text: 'Инспектор',
                       flex: 2,
                       filter: {
                           xtype: 'textfield'
                       },
                       width: 100
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
                           items: [
                               { xtype: 'b4addbutton' }
                           ]
                       }
                   ]
               },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});