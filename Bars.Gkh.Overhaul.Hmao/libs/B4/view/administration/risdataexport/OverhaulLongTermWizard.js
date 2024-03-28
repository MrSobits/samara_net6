Ext.define('B4.view.administration.risdataexport.OverhaulLongTermWizard', {
    extend: 'Ext.form.Panel',
    alias: 'widget.risdataexportoverhaullongtermwizard',

    requires: [
        'B4.store.dict.municipality.ByParam',
        'B4.store.dict.Municipality',
        'B4.store.version.ProgramVersion',
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
        'KprEntityGroup'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.version.ProgramVersion', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'ProgramVersion',
                    listAction: 'ListMainVersions'
                }
            }),
            municipalityStore = Ext.create('B4.store.dict.municipality.ByParam', { remoteFilter: false });

        store.on('beforeload', me.onBeforeLoadStore, me);

        Ext.apply(me, {
            bodyPadding: 5,
            disabled: true,
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramVersionMunicipalityList',
                    fieldLabel: 'Муниципальный район',
                    store: 'B4.store.dict.municipality.ByParam',
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
                        change: function () {
                            if (me.isAllowed) {
                                store.load();
                            }
                        }
                    }
                },
                {
                    xtype: 'b4grid',
                    name: 'LongTermList',
                    flex: 1,
                    selModel: Ext.create('B4.ux.grid.selection.CheckboxModel'),
                    store: store,
                    isRendered: true,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Municipality',
                            text: 'Муниципальный район',
                            flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListByParam'
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Name',
                            flex: 2,
                            text: 'Наименование',
                            filter: {
                                xtype: 'textfield'
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'VersionDate',
                            text: 'Дата',
                            format: 'd.m.Y',
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq,
                                format: 'd.m.Y'
                            }
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
            municipalityId = me.down('b4selectfield[name=ProgramVersionMunicipalityList]').getValue();
        operation.params = operation.params || {};
        Ext.apply(operation.params, { municipalityId: municipalityId });
    }
});