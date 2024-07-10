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
    enableColumnHide: true,
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
                    text: 'Статус задачи',
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
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'AppealState',
                    text: 'Статус обращения',
                    width: 200,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_appeal_citizens';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
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
                    dataIndex: 'CheckTime',
                    text: 'Контрольный срок',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ExtensTime',
                    text: 'Продленный срок',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 0.5,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CheckDate',
                    text: 'Срок исполнения',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'SoprDate',
                    text: 'Срок СОПР',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Num',
                    flex: 1,
                    text: 'Номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberGji',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Номер ГЖИ'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    flex: 1,
                    text: 'Контрагент',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StatementSubjects',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Тематика'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomingSources',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Источники'
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
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'CheckingInspector',
                //    flex: 1,
                //    text: 'Проверяющий(инспектор)',
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealCorr',
                    flex: 1,
                    text: 'Заявитель',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealCorrAddress',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                }
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'AppealDescription',
                //    flex: 1,
                //    text: 'Описание',
                //    filter: { xtype: 'textfield' }
                //}
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function(record) {
                    var checkDate = record.get('CheckDate'),
                        deltaDate,
                        hasAppealCitizensInWorkState = record.get('HasAppealCitizensInWorkState');
                    var isSopr = record.get('AppealState').Code;
                    if (isSopr == 'СОПР2')
                    {
                        return 'back-coralgreen';   
                    }

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