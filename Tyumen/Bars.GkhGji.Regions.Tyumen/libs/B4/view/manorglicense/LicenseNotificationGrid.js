Ext.define('B4.view.manorglicense.LicenseNotificationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.manorglicensnotificationgrid',
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

    closable: true,

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
                      dataIndex: 'RealityObject',
                      text: 'Адрес МКД',
                      flex: 1,
                      filter: {
                          xtype: 'textfield'
                      },
                      width: 100
                  },
                   {
                       xtype: 'gridcolumn',
                       dataIndex: 'Municipality',
                       text: 'Муниципальное образование',
                       flex: 1,
                       filter: {
                           xtype: 'textfield'
                       },
                       width: 100
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


//Ext.define('B4.view.manorglicense.LicenseNotificationGrid', {
//    extend: 'B4.ux.grid.Panel',
//    alias: 'widget.manorglicensnotificationgrid',
//    requires: [
//        'B4.ux.button.Update',
//        'B4.ux.grid.column.Edit',
//        'B4.ux.grid.plugin.HeaderFilters',
//        'B4.ux.grid.toolbar.Paging',
//        'B4.store.manorglicense.LicenseNotificationGis'
//    ],

//    title: 'Реестр извещений ',

    
//    closable: true,

//    initComponent: function () {
//        var me = this,
//            store = Ext.create('B4.store.manorglicense.LicenseNotificationGis');

//        Ext.applyIf(me, {
//            store: store,
//            columnLines: true,
//            columns: [
//                 {
//                     xtype: 'b4editcolumn',
//                     scope: me
//                 },
//                 {
//                     xtype: 'gridcolumn',
//                     dataIndex: 'LicenseNotificationNumber',
//                     text: 'Регистрационный номер',
//                     flex: 2,
//                     filter: {
//                         xtype: 'textfield'
//                     },
//                     width: 100
//                 },
//                 {
//                     xtype: 'gridcolumn',
//                     dataIndex: 'LocalGovernment',
//                     text: 'ОМС',
//                     flex: 1,
//                     filter: {
//                         xtype: 'textfield'
//                     },
//                     width: 100
//                 },
                
//                 {
//                     xtype: 'datecolumn',
//                     dataIndex: 'NoticeOMSSendDate',
//                     text: 'Дата',
//                     format: 'd.m.Y',
//                     width: 100
//                 }
//            ],
//            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
//            viewConfig: {
//                loadMask: true
//            },
//            dockedItems: [
//               {
//                   xtype: 'toolbar',
//                   dock: 'top',
//                   items: [
//                       {
//                           xtype: 'buttongroup',
//                           items: [
//                           {
//                               xtype: 'b4updatebutton'
//                           }
//                           ]
//                       }
//                   ]
//               },
//                {
//                    xtype: 'b4pagingtoolbar',
//                    displayInfo: true,
//                    store: store,
//                    dock: 'bottom'
//                }
//            ]
//        });

//        me.callParent(arguments);
//    }
//});