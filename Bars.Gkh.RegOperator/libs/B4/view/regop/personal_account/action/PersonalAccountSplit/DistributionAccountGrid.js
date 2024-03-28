Ext.define('B4.view.regop.personal_account.action.PersonalAccountSplit.DistributionAccountGrid', {
    extend: 'Ext.grid.Panel',

    alias: 'widget.splitdistributionaccountgrid',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.grid.column.AreaShare',
        'B4.model.regop.personal_account.SplitAccountInfo'
    ],

    columnLines: true,
    cls: 'x-large-head',
    _decimalStyle: '<div style="font-size: 11px; line-height: 13px;">{0}</div>',
    _textStyle: '<div style="font-size: 11px; line-height: 13px; text-align: right">{0}</div>',

    initComponent: function () {
        var me = this,
            decimalEditorCfg = {
                xtype: 'gkhdecimalfield'
            },
            renderer = function (val) {
                return Ext.isNumber(val) ? Ext.util.Format.currency(val) : '';
            },
            summaryRenderer = function (val, summaryData, dataIndex) {
                return Ext.isNumber(val) ? Ext.String.format(me._decimalStyle, Ext.util.Format.currency(val)) : '';
            };

        Ext.applyIf(me, {
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccountNum',
                    text: 'Номер ЛС',
                    flex: 1,
                    minWidth: 80,
                    maxWidth: 100,
                    renderer: function(val) {
                        return val || 'Новый ЛС';
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'OpenDate',
                    text: 'Дата открытия',
                    width: 80
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerType',
                    text: 'Тип абонента',
                    flex: 1.2,
                    maxWidth: 200,
                    renderer: function(val) { return B4.enums.regop.PersonalAccountOwnerType.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerName',
                    text: 'ФИО/Наименование абонента',
                    flex: 1.5,
                    summaryRenderer: function(val, summaryData) {
                        return Ext.String.format(me._textStyle, 'Итого:');
                    }
                },
                {
                    xtype: 'areasharecolumn',
                    dataIndex: 'NewAreaShare',
                    text: 'Доля собственности новая',
                    width: 90,
                    summaryType: 'sum',
                    summaryRenderer: summaryRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BaseTariffDebt',
                    text: 'Сальдо по базовому тарифу',
                    summaryType: 'sum',
                    renderer: renderer,
                    summaryRenderer: summaryRenderer
                    
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewBaseTariffDebt',
                    text: 'Распределение сальдо по базовому тарифу',
                    summaryType: 'sum',
                    editor: decimalEditorCfg,
                    renderer: renderer,
                    summaryRenderer: summaryRenderer
                    
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DecisionTariffDebt',
                    text: 'Сальдо по тарифу решения',
                    summaryType: 'sum',
                    renderer: renderer,
                    summaryRenderer: summaryRenderer
                    
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewDecisionTariffDebt',
                    text: 'Распределение сальдо по тарифу решения',
                    summaryType: 'sum',
                    editor: decimalEditorCfg,
                    renderer: renderer,
                    summaryRenderer: summaryRenderer
                    
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PenaltyDebt',
                    text: 'Сальдо по пени',
                    summaryType: 'sum',
                    renderer: renderer,
                    summaryRenderer: summaryRenderer
                    
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewPenaltyDebt',
                    text: 'Распределение сальдо по пени',
                    summaryType: 'sum',
                    editor: decimalEditorCfg,
                    renderer: renderer,
                    summaryRenderer: summaryRenderer
                }
            ],
            plugins: [Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        beforeedit: function(editor, e, eOpts) {
                            return me.up('window').down('combobox[name=DistributionType]').getValue() ===
                                B4.enums.regop.SplitAccountDistributionType.Manual;
                        }
                    }
                })
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function(record, rowIndex, rowParams, store) {
                    return record.getId() ? 'back-coralred' : null;
                }
            },
            features: [{
                ftype: 'summary'
            }]
        });

        me.callParent(arguments);
    }
});