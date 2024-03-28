Ext.define('B4.view.person.DisqualificationInfoGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.persondisqualinfogrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.person.DisqualificationInfo',
        'B4.enums.TypePersonDisqualification'
    ],

    title: 'Сведения о дисквалификации',
    
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.person.DisqualificationInfo');

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
                    dataIndex: 'TypeDisqualification',
                    flex: 2,
                    text: 'Основание',
                    renderer: function (val) {
                        if (val) {
                            return B4.enums.TypePersonDisqualification.displayRenderer(val);
                        }
                        return "";
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DisqDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDisqDate',
                    text: 'Дата окончания',
                    format: 'd.m.Y',
                    width: 150
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PetitionNumber',
                    text: 'Номер ходатайства'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PetitionDate',
                    text: 'Дата ходатайства',
                    format: 'd.m.Y',
                    width: 150
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