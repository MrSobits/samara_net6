Ext.define('B4.view.EmsedGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.emsedGrid',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Update'
    ],

    store: 'DocsForSendInEmsed',
    itemId: 'emsedGrid',
    closable: false,
    title: 'Документы',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentName',
                    flex: 1,
                    text: 'Документы',
                    filter: { xtype: 'textfield' }
                }
            ],
            selModel: Ext.create('Ext.selection.CheckboxModel', { mode: 'MULTI' }),
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
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Отправить в ЕМСЭД',
                                    textAlign: 'left',
                                    itemId: 'btnSendDocuments'
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});