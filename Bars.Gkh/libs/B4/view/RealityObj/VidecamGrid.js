Ext.define('B4.view.realityobj.VidecamGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realityobjvidecamgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.YesNoNotSet'
    ],

    title: 'Камеры видеонаблюдения',
    store: 'realityobj.RealityObjectVidecam',
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
                    dataIndex: 'Workability',
                    text: 'Функционирует',
                    flex: 1,
                    renderer: function (val) {
                        return B4.enums.YesNoNotSet.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnicalNumber',
                    flex: 1,
                    text: 'Номер'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InstallPlace',
                    flex: 2,
                    text: 'Место установки'
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
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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