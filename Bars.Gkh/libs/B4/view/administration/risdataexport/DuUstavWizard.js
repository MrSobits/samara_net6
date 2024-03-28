Ext.define('B4.view.administration.risdataexport.DuUstavWizard', {
    extend: 'Ext.form.Panel',
    alias: 'widget.risdataexportduustavwizard',

    requires: [
        'B4.store.manorg.contract.Base',

        'B4.enums.FormatDataExportType',

        'B4.form.SelectField',
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.selection.CheckboxModel',
        'Ext.ux.grid.FilterBar',

        'B4.mixins.WizardPage',
    ],

    mixins: {
        page: 'B4.mixins.WizardPage'
    },

    allowSection: [
        'DuEntityGroup',
        'UstavEntityGroup'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.manorg.contract.Base', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'ManOrgBaseContract',
                    listAction: 'FormatDataExportList'
                }
            });

        store.on('beforeload', me.onBeforeLoadStore, me);

        Ext.apply(me, {
            bodyPadding: 5,
            disabled: true,
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'checkbox',
                            fieldLabel: 'Не актуализировать вложения',
                            margin: '0 0 5 -30',
                            name: 'WithoutAttachment',
                            listeners: {
                                change: function () {
                                    store.load();
                                }
                            }
                        },
                        {
                            xtype: 'checkbox',
                            fieldLabel: 'Показать действующие',
                            margin: '0 0 5 0',
                            name: 'ShowValid',
                            listeners: {
                                change: function () {
                                    store.load();
                                }
                            }
                        },
                    ]
                },
                {
                    xtype: 'b4grid',
                    name: 'DuUstavList',
                    flex: 1,
                    selModel: Ext.create('B4.ux.grid.selection.CheckboxModel'),
                    store: store,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'TypeContractString',
                            text: 'Тип договора',
                            filter: {
                                xtype: 'textfield'
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'StartDate',
                            text: 'Дата начала',
                            format: 'd.m.Y',
                            width: 100,
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'EndDate',
                            text: 'Дата окончания',
                            format: 'd.m.Y',
                            width: 100,
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Address',
                            flex: 1,
                            text: 'Адрес',
                            itemId: 'addressGridColumn',
                            filter: { xtype: 'textfield' }
                        },
                    ],
                    plugins: [
                        {
                            ptype: 'filterbar',
                            renderHidden: false,
                            showShowHideButton: false,
                            showClearAllButton: false
                        }
                    ],
                    viewConfig: {
                        loadMask: true,
                    },
                    dockedItems: [
                        {
                            xtype: 'toolbar',
                            dock: 'top',
                            name: 'buttons',
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    columns: 1,
                                    items: [
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
                            store: store,
                            dock: 'bottom'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    onBeforeLoadStore: function (store, operation) {
        var me = this,
            baseFormWizard = me.up('risdataexportwizard'),
            duUstavWizard = baseFormWizard.down('risdataexportduustavwizard'),
            contragentId = baseFormWizard.down('b4selectfield[name=MainContragent]').getValue(),
            exportType = baseFormWizard.down('[name=ExportType]').getValue(),
            showValid = baseFormWizard.down('[name=ShowValid]').getValue(),
            isDu,
            isUstav;
            
        isDu = exportType == B4.enums.FormatDataExportType.Du;
        isUstav = exportType == B4.enums.FormatDataExportType.Ustav;

        if (isDu) {
            duUstavWizard.setTitle('Договоры управления');
        }
        if (isUstav) {
            duUstavWizard.setTitle('Уставы');
        }

        operation.params = operation.params || {};

        Ext.apply(operation.params, {
            contragentId: contragentId,
            isDu: isDu,
            isUstav: isUstav,
            showValid: showValid
        });
    }
});