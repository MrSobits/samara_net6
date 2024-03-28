Ext.define('B4.view.taskcalendar.ResolProsGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.taskcalendarresolprosGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.form.GridStateColumn',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'Ext.ux.grid.FilterBar',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum',
        'B4.enums.CourtMeetingResult',
        'B4.enums.DisputeCategory',
        'B4.enums.DisputeType',
        'B4.enums.CourtPracticeState'
    ],

    store: 'taskcalendar.ListResolPros',
    title: 'Постановления прокуратуры',
    closable: false,
    enableColumnHide: true,

    initComponent: function () {
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
                    dataIndex: 'DocumenNumber',
                    text: 'Номер постановления',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FIO',
                    text: 'ФИО',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContrAgentName',
                    text: 'Контрагент',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateResolPros',
                    text: 'Дата рассмотрения',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },                
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ResolProsTime',
                    text: 'Время рассмотрения',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                } 
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            //     plugins: [{ ptype: 'filterbar', renderHidden: false, showShowHideButton: false, showClearAllButton: false }],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {                  
                    var control = record.get('InstanceGjiCode');
                    if (control == '02') {
                        return 'back-red';
                    }
                    else if (control == '01') {
                        return 'back-yellow';
                    }

                    return '';
                }
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
                                    itemId: 'updatebutton',
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