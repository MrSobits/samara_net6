Ext.define('B4.view.objectcr.TypeWorkCrPotentialWorksGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.typeworkcrpotentialworksgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
    ],

    title: 'Работы для добавления',
    store: 'objectcr.TypeWorkCrPotentialWorks',
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 0.5,
                    text: 'Работа'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    flex: 0.5,
                    text: 'Год'
                },
                {
                    xtype: 'actioncolumn',
                    action: 'addWork',
                    width: 18,
                    align: 'center',
                    icon: B4.Url.content('content/img/icons/add.png'),
                    tooltip: 'Добавить',
                },
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
                            columns: 2,
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});