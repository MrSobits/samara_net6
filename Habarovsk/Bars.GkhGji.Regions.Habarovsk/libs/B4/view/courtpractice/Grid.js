Ext.define('B4.view.courtpractice.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.courtpracticeGrid',

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
        'B4.enums.DisputeCategory',
        'B4.enums.DisputeType',
        'B4.enums.CourtPracticeState'
    ],

    store: 'courtpractice.CourtPractice',
    title: 'Реестр судебной практики',
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
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 100,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'courtpractice';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
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
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeFactViolation',
                    text: 'Предмет спора',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentGJINumber',
                    text: 'Номер документа ГЖИ',
                    flex: 1,
                    filter: { xtype: 'textfield' }
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
                    dataIndex: 'Inspector',
                    text: 'Инспектор',
                    flex: 1,
                    filter: { xtype: 'textfield' }
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
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'DateCourtMeeting',
                //    text: 'Дата и время с/з',
                //    format: 'd.m.Y',
                //    flex: 0.5,
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.CourtMeetingResult',
                    dataIndex: 'CourtMeetingResult',
                    text: 'Результат рассмотрения',
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
                        },
                        {
                            xtype: 'button',
                            iconCls: 'icon-table-go',
                            text: 'Экспорт',
                            textAlign: 'left',
                            itemId: 'btnExport'
                        }, 
                        {
                            xtype: 'checkbox',
                            itemId: 'cbShowCloseAppeals',
                            boxLabel: 'Показать закрытые',
                            labelAlign: 'right',
                            checked: false,
                            margin: '10px 10px 0 0'
                        },      
                        {
                            xtype: 'datefield',
                            labelWidth: 60,
                            fieldLabel: 'Период с',
                            width: 160,
                            itemId: 'dfDateStart',
                            value: new Date(new Date().getFullYear(), 0, 1)
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 30,
                            fieldLabel: 'по',
                            width: 130,
                            itemId: 'dfDateEnd',
                            value: new Date(new Date().getFullYear(), 11, 31)
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