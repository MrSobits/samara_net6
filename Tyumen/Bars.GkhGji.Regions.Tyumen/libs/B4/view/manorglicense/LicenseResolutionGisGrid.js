Ext.define('B4.view.manorglicense.LicenseResolutionGisGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.manorglicenseresolutiongisgrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeManOrgTypeDocLicense',
        'B4.store.manorglicense.LicenseResolutionGis',
        'B4.ux.grid.column.Edit',
         'B4.enums.TypeBase'
    ],

    title: 'Постановления по неисполненным предписаниям',
    //store: 'view.Resolution',
    //itemId: 'docsGjiRegisterResolutionGrid',
    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.manorglicense.LicenseResolutionGis');

        var renderer = function (val, meta, rec) {
            return val;
        };


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
                     dataIndex: 'DocNum',
                     text: 'Номер постановления',
                     flex: 1,
                     filter: {
                         xtype: 'textfield'
                     },
                     width: 100
                 },                
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocDate',
                    text: 'Дата постановления',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executant',
                    text: 'Исполнитель',
                    flex: 2,
                    filter: {
                        xtype: 'textfield'
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeBase',
                    width: 150,
                    text: 'Основание проверки',
                    renderer: function (val, meta, rec) {
                        val = renderer(val, meta, rec);
                        if (val != 60)
                            return B4.enums.TypeBase.displayRenderer(val);
                        return '';
                    }
                },
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                //{
                //    xtype: 'toolbar',
                //    dock: 'top',
                //    items: [
                //        {
                //            xtype: 'b4updatebutton'
                //        }
                //    ]
                //},
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