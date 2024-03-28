Ext.define('B4.view.realityobj.PublicServiceOrgGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realobjpublicserviceorggrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    title: 'Поставщики ресурсов',
    store: 'realityobj.PublicServiceOrg',
    itemId: 'realObjPublicServOrgGrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PublicServiceOrg',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    text: 'Дата начала',
                    width: 200,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    text: 'Дата окончания',
                    width: 150,
                    format: 'd.m.Y'
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});