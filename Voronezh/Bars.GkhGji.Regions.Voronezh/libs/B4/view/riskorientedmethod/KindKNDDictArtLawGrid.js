Ext.define('B4.view.riskorientedmethod.KindKNDDictArtLawGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.Koefficients',
        'B4.ux.grid.toolbar.Paging',
         'B4.form.ComboBox',
    ],
    alias: 'widget.kindknddictartlawgrid',
    title: 'Статьи закона',
    store: 'riskorientedmethod.KindKNDDictArtLaw',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Статья закона',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                   {
                       xtype: 'gridcolumn',
                       dataIndex: 'Koefficients',
                       flex: 0.5,
                       text: 'Коэффициент',
                       renderer: function (val) {
                           return B4.enums.Koefficients.displayRenderer(val);
                       },
                       editor: {
                           xtype: 'b4combobox',
                           valueField: 'Value',
                           displayField: 'Display',
                           items: B4.enums.Koefficients.getItems(),
                           editable: false
                       }
                   },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                   Ext.create('Ext.grid.plugin.CellEditing', {
                       clicksToEdit: 1,
                       pluginId: 'cellEditing'
                   })
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
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