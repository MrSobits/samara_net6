Ext.define('B4.view.administration.risdataexport.GjiWizard', {
    extend: 'Ext.form.Panel',
    alias: 'widget.risdataexportgjiwizard',

    requires: [
        'B4.enums.AuditType',
        'B4.enums.TypeBaseFormatDataExport',
        'B4.model.disposal.FormatDataExport',
        'B4.store.dict.Municipality',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.selection.CheckboxModel',
        'Ext.ux.grid.FilterBar',

        'B4.mixins.WizardPage',
    ],

    mixins: {
        page: 'B4.mixins.WizardPage'
    },

    allowSection: [
        'AuditEntityGroup',
        'AuditPlanEntityGroup'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                autoLoad: false,
                model: 'B4.model.disposal.FormatDataExport',
            });

        store.on('beforeload', me.onBeforeLoadStore, me);

        Ext.apply(me, {
            bodyPadding: 5,
            disabled: true,
            items: [
                {
                    xtype: 'b4enumcombo',
                    name: 'AuditType',
                    fieldLabel: 'Тип проверки',
                    enumName: 'B4.enums.AuditType',
                    listeners: {
                        change: function (field, value) {
                            if (me.isAllowed && value !== undefined) {
                                store.load();
                            }
                        }
                    }
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    padding: '0 0 0 50',
                    defaults: {
                        labelWidth: 150,
                        width: 280,
                        labelAlign: 'right',
                        padding: '5 15 5 0'
                    },
                    name: 'DisposalDateInterval',
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DisposalStartDate',
                            fieldLabel: 'Дата распоряжения с',
                            listeners: {
                                specialkey: function (field, e) {
                                    if (e.getKey() == e.ENTER) {
                                        store.load();
                                    }
                                }
                            }
                        },
                        {
                            xtype: 'datefield',
                            name: 'DisposalEndDate',
                            fieldLabel: 'Дата распоряжения по',
                            listeners: {
                                specialkey: function (field, e) {
                                    if (e.getKey() == e.ENTER) {
                                        store.load();
                                    }
                                }
                            }
                        }
                    ]
                },
                {
                    xtype: 'b4grid',
                    name: 'InspectionList',
                    flex: 1,
                    selModel: Ext.create('B4.ux.grid.selection.CheckboxModel'),
                    store: store,
                    columns: [
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.TypeBaseFormatDataExport',
                            filter: true,
                            text: 'Тип проверки',
                            dataIndex: 'TypeBase',
                            width: 170,
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'MunicipalityName',
                            width: 160,
                            text: 'Муниципальный район',
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListMoAreaWithoutPaging'
                            }
                        },
                        {
                            header: 'Юридическое лицо',
                            dataIndex: 'ContragentName',
                            flex: 4,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'datecolumn',
                            text: 'Дата проверки',
                            dataIndex: 'CheckDate',
                            width: 100,
                            format: 'd.m.Y',
                            filter: { xtype: 'datefield' }
                        },
                        {
                            text: 'Номер ГЖИ',
                            dataIndex: 'DocumentNumber',
                            width: 100,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    plugins: [
                        Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
            formValue = me.getForm().getValues(false, true, false);
        operation.params = operation.params || {};
        Ext.apply(operation.params, formValue);
    }
});