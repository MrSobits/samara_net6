Ext.define('B4.view.dict.multipurpose.GlossaryGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.multiGlossaryGrid',

    requires: [
        'B4.ux.breadcrumbs.Breadcrumbs',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging'
    ],
    store: 'dict.multipurpose.MultipurposeGlossary',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columns: [
                { xtype: 'b4editcolumn', scope: me },
                {
                    xtype: 'gridcolumn',
                    header: 'Код',
                    dataIndex: 'Code',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    header: 'Наименование',
                    dataIndex: 'Name',
                    flex: 2
                },
                { xtype: 'b4deletecolumn', scope: me }
            ],
            title: 'Справочники',
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        { xtype: 'b4addbutton' },
                        { xtype: 'b4updatebutton' }
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

        this.callParent(arguments);
    }
});