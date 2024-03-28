Ext.define('B4.view.administration.risdataexport.OverhaulShortTermWizard', {
    extend: 'Ext.form.Panel',
    alias: 'widget.risdataexportoverhaulshorttermwizard',

    requires: [
        'B4.enums.TypeProgramStateCr',
        'B4.enums.TypeVisibilityProgramCr',

        'B4.store.dict.Municipality',
        'B4.store.ObjectCr',
        'B4.store.dict.ProgramCr',

        'B4.form.EnumCombo',
        'B4.form.SelectField',

        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.State',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.selection.CheckboxModel',
        'Ext.ux.grid.FilterBar',

        'B4.mixins.WizardPage',
    ],

    mixins: {
        page: 'B4.mixins.WizardPage'
    },

    allowSection: [
        'KprEntityGroup',
        'DogovorPkrEntityGroup',
        'ActWorkDogovEntityGroup',
        'CrFundSizeEntityGroup',
        'CreditContractEntityGroup'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.ObjectCr', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'ObjectCr',
                    listAction: 'ListWithoutFilter'
                }
            });

        store.on('beforeload', me.onBeforeLoadStore, me);

        Ext.apply(me, {
            bodyPadding: 5,
            disabled: true,
            items: [
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'ProgramCrList',
                    textProperty: 'Name',
                    fieldLabel: 'Программа КР',
                    store: 'B4.store.dict.ProgramCr',
                    selectionMode: 'MULTI',
                    labelWidth: 210,
                    labelAlign: 'right',
                    isRendered: true,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }},
                        { text: 'Период', dataIndex: 'PeriodName', flex: 1, filter: { xtype: 'textfield' }},
                        { text: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' }},
                        { xtype: 'b4enumcolumn', enumName: 'B4.enums.TypeVisibilityProgramCr', text: 'Видимость', dataIndex: 'TypeVisibilityProgramCr', flex: 1, filter: true },
                        { xtype: 'b4enumcolumn', enumName: 'B4.enums.TypeProgramStateCr', text: 'Состояние', dataIndex: 'TypeProgramStateCr', flex: 1, filter: true }
                    ],
                    listeners: {
                        change: function (field, value) {
                            if (me.isAllowed && value !== undefined) {
                                store.load();
                            }
                        }
                    }
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ObjectCrMunicipalityList',
                    fieldLabel: 'Муниципальный район',
                    store: 'B4.store.dict.Municipality',
                    textProperty: 'Name',
                    selectionMode: 'MULTI',
                    labelWidth: 210,
                    labelAlign: 'right',
                    isRendered: true,
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 3,
                            filter: { xtype: 'textfield' }
                        },
                        { text: 'ОКАТО', dataIndex: 'Okato', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'ОКТМО', dataIndex: 'Oktmo', flex: 1, filter: { xtype: 'textfield' } }
                    ],
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
                    name: 'ShortTermList',
                    flex: 1,
                    selModel: Ext.create('B4.ux.grid.selection.CheckboxModel'),
                    store: store,
                    columns: [
                        {
                            xtype: 'statecolumn',
                            typeId: 'cr_object',
                            width: 150,
                            canChange: false
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ProgramCrName',
                            text: 'Программа',
                            flex: 2,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/ProgramCr/ListWithoutPaging?forObjCr=true'
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Municipality',
                            text: 'Муниципальный район',
                            flex: 2,
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
                            xtype: 'gridcolumn',
                            dataIndex: 'RealityObjName',
                            text: 'Адрес',
                            flex: 3,
                            filter: { xtype: 'textfield' }
                        }
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
            programId = me.down('b4selectfield[name=ProgramCrList]').getValue(),
            municipalityId = me.down('b4selectfield[name=ObjectCrMunicipalityList]').getValue();
        operation.params = operation.params || {};

        Ext.apply(operation.params, {
            programId: programId,
            municipalityId: municipalityId
        });
    }
});