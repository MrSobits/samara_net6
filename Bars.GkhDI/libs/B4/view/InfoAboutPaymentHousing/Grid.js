Ext.define('B4.view.infoaboutpaymenthousing.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.grid.plugin.HeaderFilters',
        
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField'
    ],

    title: 'Сведения об оплатах жилищных услуг',
    store: 'InfoAboutPaymentHousing',
    itemId: 'infoAboutPaymentHousingGrid',
    closable: true,
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BaseServiceName',
                    flex: 3,
                    text: 'Наименование услуги'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProviderName',
                    flex: 3,
                    text: 'Поставщик услуги'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GeneralAccrual',
                    flex: 1,
                    text: 'Общие начисления',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Collection',
                    flex: 1,
                    text: 'Сбор',
                    editor: 'gkhdecimalfield'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })],
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
                                },
                                {
                                    xtype: 'b4savebutton'
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