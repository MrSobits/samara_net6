Ext.define('B4.view.fssp.courtordergku.ImportedAddressMatchingRegistry', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Update',

        'B4.model.fssp.addressmatching.ImportedAddress'
    ],

    alias: 'widget.importedaddressmatchingregistry',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                model: 'B4.model.fssp.addressmatching.ImportedAddress',
                autoLoad: false
            }),
            controller = Ext.create('B4.controller.fssp.AddressMatching', {
                application: b4app,
                containerSelector: '#' + me.getId()
            });

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'actioncolumn',
                    tooltip: 'Сопоставить адрес вручную',
                    name: 'match',
                    align: 'center',
                    width: 20,
                    icon: B4.Url.content('content/img/icons/database_edit.png'),
                    handler: function(gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this;
                        var scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'match', rec);
                    },
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileName',
                    text: 'Наименование файла импорта',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileAddress',
                    text: 'Адрес в файле',
                    filter: { xtype: 'textfield' },
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SystemAddress',
                    text: 'Адрес в системе',
                    filter: { xtype: 'textfield' },
                    flex: 2
                },
                {
                    xtype: 'actioncolumn',
                    name: 'mismatch',
                    tooltip: 'Разорвать связь с адресом ПГМУ',
                    align: 'center',
                    width: 20,
                    icon: B4.Url.content('content/img/icons/cog.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this;
                        var scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'mismatch', rec);
                    },
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

        controller.init();
    }
});