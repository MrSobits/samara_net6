Ext.define('B4.view.billingaddressmatching.ImportedGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.filter.YesNo',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    alias: 'widget.importedgrid',

    initComponent: function() {
        var me = this,
            store = Ext.create("B4.store.billingaddressmatching.ImportedAddress");

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'actioncolumn',
                    tooltip: 'Сопоставить адрес вручную',
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
                    text: 'Тип импорта',
                    dataIndex: 'ImportType',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Наименование файла импорта',
                    dataIndex: 'ImportFilename',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Код адреса из сторонней системы',
                    dataIndex: 'AddressCodeRemote',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Адрес из сторонней системы',
                    dataIndex: 'AddressRemote',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Код адреса МЖФ',
                    dataIndex: 'AddressCode',
                    flex: 2,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Адрес',
                    dataIndex: 'Address',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Сопоставлен',
                    dataIndex: 'IsMatched',
                    flex: 2,
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'actioncolumn',
                    name: 'mismatch',
                    tooltip: 'Разорвать связь дом - код ЕРЦ',
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

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    name: 'billingAddrInfo'
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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