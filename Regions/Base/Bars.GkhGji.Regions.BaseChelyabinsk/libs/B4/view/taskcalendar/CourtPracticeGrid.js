Ext.define('B4.view.taskcalendar.CourtPracticeGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.taskcalendarcourtpracticeGrid',

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

    store: 'taskcalendar.ListCourt',
    title: 'Судебная практика',
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
                    dataIndex: 'JurInstitution',
                    text: 'Суд',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    text: 'Номер дела',
                    flex: 0.5,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlantiffChoice',
                    text: 'Истец',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DefendantChoice',
                    text: 'Ответчик',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DifferentChoice',
                    text: 'Третьи лица',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeFactViolation',
                    text: 'Предмет спора',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/TypeFactViolation/List'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Lawyer',
                    text: 'Юрист',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InLawDate',
                    text: 'Дата и время с/з',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    text: 'Инспектор',
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