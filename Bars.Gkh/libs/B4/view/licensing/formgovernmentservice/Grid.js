Ext.define('B4.view.licensing.formgovernmentservice.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',

        'B4.form.ComboBox',

        'B4.store.licensing.FormGovernmentService',

        'B4.enums.Quarter',
        'B4.enums.FormGovernmentServiceType'
    ],

    title: 'Форма 1-ГУ',
    alias: 'widget.formgovernmentservicegrid',
    closable: true,
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.licensing.FormGovernmentService');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    flex: 1,
                    text: 'Год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        allowDecimals: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Quarter',
                    flex: 1,
                    text: 'Квартал',
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.Quarter.getItems(),
                        editable: false
                    },
                    renderer: function(val) {
                        return val ? B4.enums.Quarter.displayRenderer(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GovernmentServiceType',
                    flex: 2,
                    text: 'Услуга',
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.FormGovernmentServiceType.getItems(),
                        editable: false
                    },
                    renderer: function (val) {
                        return val ? B4.enums.FormGovernmentServiceType.displayRenderer(val) : '';
                    }
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});