Ext.define('B4.view.manorg.ManOrgBilMkdWorkGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.manorg.ManOrgBilMkdWork'
    ],

    alias: 'widget.manorgbilmkdworkgrid',
    closable: false,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.manorg.ManOrgBilMkdWork');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Описание',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
               Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
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
                   store: store,
                   dock: 'bottom'
               }
            ]
        });

        me.callParent(arguments);
    }
});