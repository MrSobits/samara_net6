Ext.define('B4.view.appealcits.AppealCitsCategoryGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [        
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete'
    ],

    title: 'Категории корреспондента',
    alias: 'widget.appealcitscategorygrid',
    columnLines: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.appealcits.AppealCitsCategory');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    text: 'Наименование',
                    dataIndex: 'ApplicantCategory',
                    flex: 1,
                    filter: {
                        xtype: 'textfield',
                        filterName: 'ApplicantCategory.Name'
                    },
                    renderer: function(val) {
                        return val && val.Name
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
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
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});