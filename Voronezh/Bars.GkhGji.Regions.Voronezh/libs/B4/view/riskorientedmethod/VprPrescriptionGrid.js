Ext.define('B4.view.riskorientedmethod.VprPrescriptionGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.vprprescriptiongrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Предписания Vпр',
    store: 'riskorientedmethod.VprPrescription',
    itemId: 'vprPrescriptionGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Prescription',
                    flex: 1,
                    text: 'номер предписания'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PrescriptionDate',
                    flex: 0.5,
                    text: 'Дата предписания',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StateName',
                    flex: 1,
                    text: 'Статус'
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