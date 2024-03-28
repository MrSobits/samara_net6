Ext.define('B4.view.disposal.SurveyPurposeGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.disposalsurveypurposegrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Цели проверки',
    store: 'disposal.SurveyPurpose',

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    text: 'Код',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    editor: 'textfield',
                    flex: 2,
                    text: 'Наименование'
                },
                {
                    xtype: 'b4deletecolumn'
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
                                        columns: 3,
                                        items: [
                                            {
                                                xtype: 'b4addbutton'
                                            },
                                            {
                                                xtype: 'b4updatebutton'
                                            },
                                            {
                                                xtype: 'button',
                                                itemId: 'disposalSurveyPurposeGridSaveButton',
                                                iconCls: 'icon-accept',
                                                text: 'Сохранить'
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