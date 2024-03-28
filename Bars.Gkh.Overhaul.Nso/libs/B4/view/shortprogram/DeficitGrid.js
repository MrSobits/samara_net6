Ext.define('B4.view.shortprogram.DeficitGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.shortprogramdeficitgrid',

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Дефицит по МО',
    store: 'ShortProgramDeficit',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    return 'x-coralyellow-row';
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'tbseparator'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сформировать краткосрочную программу',
                                    action: 'CalcShortProgram',
                                    iconCls: 'icon-table-go'
                                },
                                {
                                    xtype: 'tbseparator'
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    itemId: 'btnUpdatePr'
                                }
                            ]
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});