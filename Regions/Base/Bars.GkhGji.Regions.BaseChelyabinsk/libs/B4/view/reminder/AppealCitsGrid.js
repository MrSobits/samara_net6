Ext.define('B4.view.reminder.AppealCitsGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.form.GridStateColumn'
    ],

    alias: 'widget.reminderAppealCitsGrid',
    title: 'Задачи по обращениям',
    itemId: 'reminderAppealCitsGrid',
    closable: true,
    
    initComponent: function() {
        var me = this;
        me.store = Ext.create('B4.store.reminder.AppealCitsReminder');
        
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
                    text: 'Статус рассмотрения',
                    width: 200,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_appcits_executant';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
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
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    text: 'Дата',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CheckDate',
                    text: 'Контрольный срок',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Num',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Номер'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Guarantor',
                    flex: 1,
                    text: 'Поручитель',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    flex: 1,
                    text: 'Исполнитель',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CheckingInspector',
                    flex: 1,
                    text: 'Проверяющий(инспектор)',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealCorr',
                    flex: 1,
                    text: 'ФИО',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealCorrAddress',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealDescription',
                    flex: 1,
                    text: 'Описание',
                    filter: { xtype: 'textfield' }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function(record) {
                    var checkDate = record.get('CheckDate'),
                        deltaDate,
                        hasAppealCitizensInWorkState = record.get('HasAppealCitizensInWorkState');

                    deltaDate = ((new Date(checkDate)).getTime() - (new Date()).getTime()) / (1000 * 60 * 60 * 24);
                    if (deltaDate >= 0 && deltaDate <= 5 && !hasAppealCitizensInWorkState) {
                        return 'back-coralyellow';
                    }

                    if (deltaDate < 0 && !hasAppealCitizensInWorkState) {
                        return 'back-coralred';
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
                                
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});