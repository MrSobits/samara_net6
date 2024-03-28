Ext.define('B4.view.realestatetype.StructElemGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.structelemeditgrid',

    requires: [
        'B4.store.RealEstateTypeCommonParam',

        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Delete',

        'B4.form.ComboBox',

        'Ext.grid.plugin.CellEditing',
        'Ext.ux.CheckColumn'
    ],

    store: 'RealEstateTypeStructElement',

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StructuralElement',
                    flex: 1,
                    text: 'Группа структурного элемента',
                    renderer: function (val) {
                        return val.Group.Name;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StructuralElement',
                    flex: 1,
                    text: 'Структурный элемент',
                    renderer: function (val) {
                        return val.Name;
                    }
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'Exists',
                    width: 220,
                    text: 'КЭ присутствует/отсутствует в доме'
                },
                {
                    xtype: 'b4deletecolumn'
                }
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
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });
        me.callParent(arguments);
    }
});