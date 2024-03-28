Ext.define('B4.view.claimwork.buildcontract.ViolGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.claimwork.BuildContractClwViol'
    ],

    title: 'Нарушения условий договора',
    cls: 'x-large-head',
    alias: 'widget.buildcontractviolgrid',

    closable: false,
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.claimwork.BuildContractClwViol');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Violation',
                    flex: 1,
                    text: 'Нарушение',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Note',
                    flex: 1,
                    text: 'Примечание',
                    filter: { xtype: 'textfield' }
                }
            ],

            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});

