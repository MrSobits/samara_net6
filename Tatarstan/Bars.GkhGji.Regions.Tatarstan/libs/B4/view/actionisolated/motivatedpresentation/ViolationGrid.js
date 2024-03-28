Ext.define('B4.view.actionisolated.motivatedpresentation.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    alias: 'widget.motivatedpresentationviolationgrid',
    itemId: 'motivatedPresentationViolationGrid',
    title: 'Перечень нарушений',
    
    store: 'actionisolated.motivatedpresentation.ViolationInfo',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Violations',
                    text: 'Выявленные нарушения',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
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
                                    xtype: 'b4updatebutton',
                                    handler: function() {
                                        var grid = this.up('motivatedpresentationviolationgrid');

                                        grid.getStore().load();
                                    }
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