Ext.define('B4.view.dict.heatseasonperiodgji.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.heatseason_period_edit',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 600,
    bodyPadding: 5,
    itemId: 'heatSeasonPeriodGjiEditWindow',
    title: 'Период',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.Panel',
        'B4.ux.grid.column.Edit',
        'B4.model.dict.HeatingSeasonResolution',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    initComponent: function () {
        var me = this,
            resolutionStore = Ext.create('B4.base.Store', {
                model: 'B4.model.dict.HeatingSeasonResolution',
                autoLoad: false,
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'HeatingSeasonResolution',
                    listAction: 'ListFull'
                }
            });

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'hidden',
                    name: 'Id'
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    anchor: '100%',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'datefield',
                    name: 'DateStart',
                    fieldLabel: 'Дата начала',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateEnd',
                    fieldLabel: 'Дата окончания',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'b4grid',
                    height: 300,
                    store: resolutionStore,
                    flex: 1,
                    columns: [
                        {
                            xtype: 'b4editcolumn'
                        },
                        {
                            text: 'Муниципальное образование',
                            dataIndex: 'MunicipalityName',
                            flex: 1,
                            xtype: 'gridcolumn',
                            filter: {
                                xtype: 'textfield', // todo
                                operand: CondExpr.operands.contains
                            }
                        },
                        {
                            text: 'Дата',
                            dataIndex: 'AcceptDate',
                            flex: 1,
                            xtype: 'datecolumn',
                            renderer: function (value) {
                                try {
                                    var year = (new Date(value)).getFullYear();
                                    if (year === 1) {
                                        return '';
                                    }
                                    return Ext.Date.format(new Date(value), 'd.m.Y');
                                } catch (e) {
                                    return '';
                                } 
                            },
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            text: 'Файл',
                            dataIndex: 'Doc',
                            flex: 1,
                            xtype: 'gridcolumn'
                        }
                    ],
                    plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                    viewConfig: {
                        loadMask: true
                    },
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: resolutionStore,
                            dock: 'bottom'
                        }
                    ]
                }
            ],
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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