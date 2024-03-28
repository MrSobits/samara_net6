Ext.define('B4.view.dict.programcr.ChangeJournalGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.programcrchangejournalgrid',
    requires: [
        'B4.ux.button.Update',
        
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.TypeChangeProgramCr',
        'B4.store.dict.ProgramCrChangeJournal'
    ],

    title: 'Журнал изменений',
    closable: false,
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.dict.ProgramCrChangeJournal');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeChange',
                    flex: 1,
                    text: 'Способ формирования',
                    renderer: function (val) { return B4.enums.TypeChangeProgramCr.displayRenderer(val); }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'ChangeDate',
                    width: 100,
                    text: 'Дата'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MuCount',
                    width: 100,
                    text: 'Количество МО',
                    renderer: function (val) { return val ? val : ' - '; }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UserName',
                    flex: 1,
                    text: 'Пользователь'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Примечание'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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