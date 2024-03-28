Ext.define('B4.view.dict.service.BilNormativStorageGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.MonthPicker',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Норматив',
    alias: 'widget.bilnormativstoragedictgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.service.BilNormativStorage');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Municipality',
                        flex: 1,
                        text: 'Город/район',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Name',
                        flex: 1,
                        text: 'Норматив',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Value',
                        flex: 1,
                        text: 'Значение',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Measure',
                        flex: 1,
                        text: 'Ед. измерения',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Description',
                        flex: 1,
                        text: 'Описание',
                        filter: { xtype: 'textfield' }
                    }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
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
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'b4monthpicker',
                            name: 'Period',
                            width: 200,
                            fieldLabel: 'Период',
                            editable: false,
                            labelWidth: 40,
                            labelAlign: 'right',
                            format: 'F, Y'
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