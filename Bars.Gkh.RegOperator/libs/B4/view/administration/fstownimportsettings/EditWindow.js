Ext.define('B4.view.administration.fstownimportsettings.EditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.Panel',
        'B4.store.administration.fsTownImportSettingsSubData'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    
    layout: 'fit',

    width: 600,
    height: 400,

    bodyPadding: 3,

    alias: 'widget.fstownimportsettingseditwindow',

    title: 'Добавить запись',

    initComponent: function () {
        var me = this,
            headStore = Ext.create('B4.store.administration.fsTownImportSettingsSubData', {
                autoLoad: false,
                listeners: {
                    beforeload: function (store, operation) {
                        var rec = me.getRecord();
                        operation.params.infoId = parseInt(rec.getId());
                        operation.params.complexFilter = Ext.JSON.encode({
                            left: 'IsMeta',
                            right: true,
                            op: CondExpr.operands.eq
                        });
                    }
                }
            }),
            dataStore = Ext.create('B4.store.administration.fsTownImportSettingsSubData', {
                autoLoad: false,
                listeners: {
                    beforeload: function (store, operation) {
                        var rec = me.getRecord();
                        operation.params.infoId = parseInt(rec.getId());
                        operation.params.complexFilter = Ext.JSON.encode({
                            left: 'IsMeta',
                            right: false,
                            op: CondExpr.operands.eq
                        });
                    }
                }
            });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            title: 'Основное',
                            bodyStyle: Gkh.bodyStyle,
                            bodyPadding: 5,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'top'
                            },
                            items: [
                                {
                                    xtype: 'hiddenfield',
                                    name: 'Id'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Code',
                                    fieldLabel: 'Код',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    fieldLabel: 'Наименование',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Delimiter',
                                    fieldLabel: 'Разделитель',
                                    emptyText: ';'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DataHeadIndex',
                                    fieldLabel: 'Номер строки начала области данных'
                                },
                                {
                                    xtype: 'textareafield',
                                    name: 'Description',
                                    fieldLabel: 'Описание'
                                }
                            ]
                        },
                        {
                            title: 'Шапка',
                            disabled: true,
                            xtype: 'gridpanel',
                            store: headStore,
                            flex: 1,
                            border: false,
                            name: 'top',
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
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            columns: [
                                {
                                    xtype: 'b4editcolumn'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    flex: 1,
                                    dataIndex: 'PropertyName',
                                    text: 'Наименование'
                                },
                                {
                                    xtype: 'b4deletecolumn'
                                }
                            ]
                        },
                        {
                            xtype: 'gridpanel',
                            title: 'Области данных',
                            disabled: true,
                            store: dataStore,
                            flex: 1,
                            border: false,
                            name: 'data',
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
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            columns: [
                                {
                                    xtype: 'b4editcolumn'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    flex: 1,
                                    dataIndex: 'PropertyName',
                                    text: 'Наименование'
                                },
                                {
                                    xtype: 'b4deletecolumn'
                                }
                            ]
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
                        { xtype: 'tbfill' },
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