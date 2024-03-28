Ext.define('B4.view.appealcits.AppealCitsHeadInspectorGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [        
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete'
    ],

    title: 'Вложения',
    alias: 'widget.appealcitsheadinspectorgrid',
    columnLines: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.appealcits.AppealCitsHeadInspector');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    text: 'ФИО',
                    dataIndex: 'Inspector',
                    flex: 2,
                    renderer: function(val) {
                        return val && val.Fio;
                    },
                    filter: {
                        xtype: 'textfield',
                        filterName: 'Inspector.Fio'

                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Должность',
                    dataIndex: 'Inspector',
                    flex: 2,
                    renderer: function(val) {
                        return val && val.Position;
                    },
                    filter: {
                        xtype: 'textfield',
                        filterName: 'Inspector.Position'

                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Номер телефона',
                    dataIndex: 'Inspector',
                    flex: 1,
                    renderer: function(val) {
                        return val && val.Phone;
                    },
                    filter: {
                        xtype: 'textfield',
                        filterName: 'Inspector.Phone'

                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'E-mail',
                    dataIndex: 'Inspector',
                    flex: 1,
                    renderer: function(val) {
                        return val && val.Email;
                    },
                    filter: {
                        xtype: 'textfield',
                        filterName: 'Inspector.Email'

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