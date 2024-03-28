Ext.define('B4.view.administration.risdataexport.RegopWizard', {
    extend: 'Ext.form.Panel',
    alias: 'widget.risdataexportregopwizard',

    requires: [
        'B4.enums.regop.PersonalAccountOwnerType',
        'B4.store.regop.personal_account.BasePersonalAccount',
        'B4.store.dict.Municipality',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.selection.CheckboxModel',
        'Ext.ux.grid.FilterBar',
        'B4.catalogs.ChargePeriodSelectField',
        'B4.model.regop.personal_account.ViewAccountOwnershipHistory',

        'B4.mixins.WizardPage',
        'B4.enums.CrFundFormationType'
    ],

    mixins: {
        page: 'B4.mixins.WizardPage'
    },

    allowSection: [
        'EpdEntityGroup',
        'OplataEntityGroup',
        'ProtocolossEntityGroup',
        'ContragentRschetEntityGroup',
        'RegOpAccountsEntityGroup',
        'KapremDecisionsEntityGroup',
        'KvarEntityGroup',
        'SpecialAccountsEntityGroup',
        'NpaEntityGroup'
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.base.Store', {
                autoLoad: false,
                model: 'B4.model.regop.personal_account.ViewAccountOwnershipHistory'
            });

        store.on('beforeload', me.onBeforeLoadStore, me);

        Ext.apply(me, {
            bodyPadding: 5,
            disabled: true,
            items: [
                {
                    xtype: 'chargeperiodselectfield',
                    name: 'ChargePeriod',
                    isRendered: true,
                    listeners: {
                        change: function (field, value) {
                            if (me.isAllowed && value !== undefined) {
                                store.load();
                            }
                        }
                    }
                },
                {
                    xtype: 'b4grid',
                    name: 'PersAccList',
                    flex: 1,
                    selModel: Ext.create('B4.ux.grid.selection.CheckboxModel'),
                    store: store,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'State',
                            width: 100,
                            text: 'Статус',
                            filter: {
                                xtype: 'b4combobox',
                                url: '/State/GetListByType',
                                storeAutoLoad: false,
                                operand: CondExpr.operands.eq,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                listeners: {
                                    storebeforeload: function(field, store, options) {
                                        options.params.typeId = 'gkh_regop_personal_account';
                                    }
                                }
                            },
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Municipality',
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
                            text: 'Адрес',
                            dataIndex: 'RoomAddress',
                            flex: 1,
                            filter: {
                                xtype: 'textfield'
                            }
                        },
                        {
                            text: 'Номер',
                            minWidth: 100,
                            maxWidth: 120,
                            dataIndex: 'PersonalAccountNum',
                            filter: {
                                xtype: 'textfield',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            text: 'ФИО/Наименование абонента',
                            flex: 1,
                            dataIndex: 'AccountOwner',
                            filter: {
                                xtype: 'textfield'
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.regop.PersonalAccountOwnerType',
                            filter: true,
                            text: 'Тип абонента',
                            dataIndex: 'OwnerType',
                            minWidth: 100,
                            maxWidth: 110
                        },
                        {
                            xtype: 'b4enumcolumn',
                            name: 'crFundFormationTypeColumn',
                            enumName: 'B4.enums.CrFundFormationType',
                            filter: true,
                            text: 'Способ формирования фонда КР',
                            dataIndex: 'AccountFormationVariant',
                            minWidth: 125
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

    onBeforeLoadStore: function(store, operation) {
        var me = this,
            formValue = me.getForm().getValues(false, true, false);
        operation.params = operation.params || {};
        Ext.apply(operation.params, { periodId: formValue.ChargePeriod });
    }
});