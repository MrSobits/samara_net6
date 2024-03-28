Ext.define('B4.view.program.LoadProgramGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.loadprogramgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.program.LoadProgram',
        'B4.view.Control.GkhButtonImport'
    ],

    title: 'Загрузка программы',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.program.LoadProgram');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    flex: 1,
                    text: 'Номер'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Locality',
                    flex: 1,
                    text: 'Наименование населенного пункта'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Street',
                    flex: 1,
                    text: 'Улица'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'House',
                    flex: 1,
                    text: 'Номер дома'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Housing',
                    flex: 1,
                    text: 'Корпус'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommissioningYear',
                    flex: 1,
                    text: 'Год ввода в эксплуатацию'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateobject',
                    flex: 1,
                    text: 'Объект общего имущества'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Wear',
                    flex: 1,
                    text: 'Износ общего имущества'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LastOverhaulYear',
                    flex: 1,
                    text: 'Год последнего ремонта'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanOverhaulYear',
                    flex: 1,
                    text: 'Плановый год проведения капитального ремонта'
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
                                { xtype: 'gkhbuttonimport' }
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