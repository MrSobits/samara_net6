Ext.define('B4.view.dict.qtestsettings.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.grid.plugin.HeaderFilters',
        
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Настройки квалификационного экзамена',
    store: 'dict.qtestsettings.QualifyTestSettings',
    alias: 'widget.qtestsettingsgrid',
    closable: true,
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                  xtype: 'b4editcolumn',
                  scope: me
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'QuestionsCount',
                     flex: 0.5,
                     text: 'Количество вопросов'
                 },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'AcceptebleRate',
                     flex: 0.5,
                     text: 'Проходной балл'
                 },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'CorrectBall',
                     flex: 0.5,
                     text: 'Баллов за ответ'
                 },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'TimeStampMinutes',
                     flex: 0.5,
                     text: 'Минут на экзамен'
                 },
                 {
                     xtype: 'datecolumn',
                     dataIndex: 'DateFrom',
                     text: 'Актуально c',
                     flex: 0.5,
                     format: 'd.m.Y',
                     width: 100
                 },
                 {
                     xtype: 'datecolumn',
                     dataIndex: 'DateTo',
                     text: 'Актуально по',
                     flex: 0.5,
                     format: 'd.m.Y',
                     width: 100
                 },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});