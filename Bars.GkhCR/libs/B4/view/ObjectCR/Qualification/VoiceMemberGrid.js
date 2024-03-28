Ext.define('B4.view.objectcr.qualification.VoiceMemberGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.qualvoicemembergrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeAcceptQualification',
        'B4.store.objectcr.qualification.VoiceMember'
    ],

    title: 'Голоса участников квалификационного отбора',

    initComponent: function() {
        var me = this,
           store = Ext.create('B4.store.objectcr.qualification.VoiceMember');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MemberName',
                    flex: 1,
                    text: 'Орган'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeAcceptQualification',
                    flex: 1,
                    text: 'Принято',
                    renderer: function(val) { return B4.enums.TypeAcceptQualification.displayRenderer(val); },
                    editor: {
                        xtype: 'combobox',
                        editable: false,
                        store: B4.enums.TypeAcceptQualification.getStore(),
                        displayField: 'Display',
                        valueField: 'Value'
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    editor: 'datefield',
                    dataIndex: 'DocumentDate',
                    width: 100,
                    text: 'Дата'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Reason',
                    flex: 1,
                    text: 'Причина',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 2000
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    actionName: 'voiceQualificationMemberSaveButton',
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});