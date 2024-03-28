Ext.define('B4.view.integrations.gis.DataSupplierGrid',
{
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.datasuppliergrid',
    requires: [
        'Ext.selection.CheckboxModel',
        'B4.ux.grid.plugin.HeaderLocalFilters'
    ],
    columnLines: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('Ext.data.Store',
            {
                fields: ['Id', 'FullName', 'Ogrn', 'JuridicalAddress'],
                proxy: {
                    type: 'memory',
                    reader: {
                        type: 'json'
                    }
                }
            });

        Ext.applyIf(me,
        {
            store: store,
            columnLines: true,
            selModel: Ext.create('Ext.selection.CheckboxModel', { mode: 'MULTI' }),
            columns: [
                {
                    xtype: 'gridcolumn',
                    flex: 3,
                    text: 'Наименование',
                    dataIndex: 'FullName',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    width: 100,
                    text: 'ОГРН',
                    dataIndex: 'Ogrn',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    flex: 2,
                    text: 'Юридический адрес',
                    dataIndex: 'JuridicalAddress',
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            plugins: [
                {
                    ptype: 'b4gridheaderlocalfilters'
                }
            ]
        });

        me.callParent(arguments);
    }
});