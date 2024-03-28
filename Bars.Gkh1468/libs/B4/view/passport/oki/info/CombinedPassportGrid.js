Ext.define('B4.view.passport.oki.info.CombinedPassportGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.combinedokipassportgrid',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.passport.OkiCombined'
    ],

    itemId: 'combinedOkiPassportGrid',
    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.passport.OkiCombined');
        
        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PpNumber',
                    flex: 1,
                    text: '№п/п',
                    renderer: function (v, m, r) {
                        if (r.data.IsMultiple) m.style = 'background: #FFDAB9';
                        return v;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 4,
                    text: 'Наименование',
                    renderer: function (v, m, r) {
                        if (r.data.IsMultiple) m.style = 'background: #FFDAB9';
                        return v;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Value',
                    flex: 2,
                    text: 'Значение',
                    renderer: function (v, m, r) {
                        if (r.data.IsMultiple) m.style = 'background: #FFDAB9';
                        return v;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InfoSupplier',
                    flex: 4,
                    text: 'Примечание',
                    renderer: function (v, m, r) {
                        if (r.data.IsMultiple) m.style = 'background: #FFDAB9';
                        return v;
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            //dockedItems: [
            //    {
            //        xtype: 'b4pagingtoolbar',
            //        displayInfo: true,
            //        store: this.store,
            //        dock: 'bottom'
            //    }
            //]
        });

        me.callParent(arguments);
    }
});