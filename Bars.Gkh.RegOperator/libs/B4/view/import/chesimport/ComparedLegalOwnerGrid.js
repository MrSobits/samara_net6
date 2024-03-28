Ext.define('B4.view.import.chesimport.ComparedLegalOwnerGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete'
    ],

    alias: 'widget.chesimportcomparedlegalownergrid',
    columnLines: true,
    title :'Юридические лица',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.import.chesimport.ChesMatchLegalAccountOwner');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccountNumber',
                    flex: 1,
                    text: 'Номер ЛС из файла',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExternalName',
                    flex: 3,
                    text: 'ЮЛ из файла',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExternalInn',
                    flex: 1,
                    text: 'ИНН из файла',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExternalKpp',
                    flex: 1,
                    text: 'КПП из файла',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 3,
                    text: 'ЮЛ из системы',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inn',
                    flex: 1,
                    text: 'ИНН  из системы',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Kpp',
                    flex: 1,
                    text: 'КПП из системы',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
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
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
