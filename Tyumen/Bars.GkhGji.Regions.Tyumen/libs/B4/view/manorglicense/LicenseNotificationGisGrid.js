Ext.define('B4.view.manorglicense.LicenseNotificationGisGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.manorglicensnotificationgisgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.manorglicense.LicenseNotificationGis'
    ],

    title: 'Уведомления',
    
    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.manorglicense.LicenseNotificationGis');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                 {
                     xtype: 'b4editcolumn',
                     scope: me
                 },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'LicenseNotificationNumber',
                     text: 'Регистрационный номер',
                     flex: 2,
                     filter: {
                         xtype: 'textfield'
                     },
                     width: 100
                 },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'LocalGovernment',
                     text: 'ОМС',
                     flex: 1,
                     filter: {
                         xtype: 'textfield'
                     },
                     width: 100
                 },
                
                 {
                     xtype: 'datecolumn',
                     dataIndex: 'NoticeOMSSendDate',
                     text: 'Дата',
                     format: 'd.m.Y',
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});