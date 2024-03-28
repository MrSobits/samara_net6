Ext.define('B4.view.dict.repairprogram.MunicipalityGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.repprogrammunicipalitygrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',

        'B4.ux.grid.plugin.HeaderFilters'
    ],

    title: 'Муниципальные образования',
    store: 'dict.RepairProgramMunicipality',
    itemId: 'repProgramMunicipalityGrid',    
    closable: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
                    flex: 1,
                    text: 'Наименование'
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4addbutton'
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