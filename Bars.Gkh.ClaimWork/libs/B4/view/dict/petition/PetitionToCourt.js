Ext.define('B4.view.dict.petition.PetitionToCourt', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.petitiontocourt',
    requires: [
        'Ext.grid.plugin.CellEditing',

        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Delete',

        'B4.store.dict.PetitionToCourt'
    ],
    closable: true,
    title: 'Заявления в суд',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.PetitionToCourt');

        Ext.apply(me, {
            columns: [
                { header: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield', operand: CondExpr.operands.contains }, editor: { xtype: 'textfield', allowBlank: false } },
                { header: 'Краткое наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield', operand: CondExpr.operands.contains }, editor: { xtype: 'textfield', allowBlank: false } },
                { header: 'Полное наименование', dataIndex: 'FullName', flex: 1, filter: { xtype: 'textfield', operand: CondExpr.operands.contains }, editor: { xtype: 'textfield', allowBlank: false } },
                { xtype: 'b4deletecolumn' }
            ],
            store: store,
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4savebutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        }
                    ]
                },
                { xtype: 'b4pagingtoolbar', dock: 'bottom', displayInfo: true, store: store }
            ],
            plugins: [
                { ptype: 'cellediting', pluginId: 'cellEditing', clicksToEdit: 1 },
                { ptype: 'b4gridheaderfilters' }
            ]
        });

        me.callParent(arguments);
    }
});