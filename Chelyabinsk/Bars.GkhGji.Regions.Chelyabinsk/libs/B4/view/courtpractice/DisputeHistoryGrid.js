Ext.define('B4.view.courtpractice.DisputeHistoryGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.courtpracticeDisputeHistoryGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'Ext.ux.grid.FilterBar',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum',
        'B4.enums.CourtMeetingResult',
        'B4.store.dict.InstanceGji',
        'B4.enums.DisputeCategory',
        'B4.enums.DisputeType',
        'B4.enums.CourtPracticeState'
    ],

    store: 'courtpractice.DisputeHistory',
    title: 'История обжалования',
    closable: true,
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
                    dataIndex: 'InstanceGji',
                    text: 'Инстанция',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/InstanceGji/List'
                    }
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'JurInstitution',
                    text: 'Суд',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },  
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.DisputeType',
                    dataIndex: 'DisputeType',
                    text: 'Вид спора',
                    flex: 0.7,
                    filter: true,
                },  
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.CourtPracticeState',
                    dataIndex: 'CourtPracticeState',
                    text: 'Статус рассмотрения',
                    flex: 0.5,
                    filter: true,
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateCourtMeetingDate',
                    text: 'Дата и время с/ з',
                    format: 'd.m.Y H:i:s',
                    width: 100,
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.CourtMeetingResult',
                    dataIndex: 'CourtMeetingResult',
                    text: 'Результат обжалования',
                    flex: 0.5,
                    filter: true,
                },                
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            xtype: 'b4addbutton'
                        },
                        {
                            xtype: 'b4updatebutton'
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