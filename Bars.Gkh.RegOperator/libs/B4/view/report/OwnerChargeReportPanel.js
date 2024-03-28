Ext.define('B4.view.report.OwnerChargeReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.ownerChargeReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.form.TreeSelectField',
        'B4.store.dict.MunicipalityTree',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.municipality.MoArea',
        'B4.store.RealtyObjectByMo',
        'B4.store.MunicipalityByParent',
        'B4.model.regop.personal_account.PersonalAccountsByRo',
        'B4.store.regop.ClosedChargePeriod'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'treeselectfield',
                    name: 'Municipality',
                    itemId: 'fiasMunicipalitiesTrigerField',
                    fieldLabel: 'Муниципальное образование',
                    titleWindow: 'Выборите муниципального образования',
                    store: 'B4.store.dict.MunicipalitySelectTree',
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    fieldLabel: 'Адрес МКД',
                    allowBlank: false,
                    emptyText: 'Выберите МКД',
                    store: 'B4.store.RealtyObjectByMo',
                    disabled: true,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Адрес', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Owners',
                    fieldLabel: 'Собственники',
                    store: 'B4.store.regop.personal_account.PersonalAccountsByRo',
                    selectionMode: 'MULTI',
                    textProperty: 'AccountOwner',
                    editable: false,
                    emptyText: 'Все собственники',
                    disabled: true,
                    columns: [
                        {
                            text: 'Помещение',
                            dataIndex: 'RoomNum',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Лицевой счет',
                            dataIndex: 'AccountNum',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Собственник',
                            dataIndex: 'AccountOwner',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.regop.ClosedChargePeriod',
                    textProperty: 'Name',
                    allowBlank: false,
                    editable: false,
                    name: 'ChargePeriod',
                    windowCfg: {
                        modal: true
                    },
                    trigger2Cls: '',
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Дата открытия',
                            xtype: 'datecolumn',
                            format: 'd.m.Y',
                            dataIndex: 'StartDate',
                            flex: 1,
                            filter: { xtype: 'datefield' }
                        },
                        {
                            text: 'Дата закрытия',
                            xtype: 'datecolumn',
                            format: 'd.m.Y',
                            dataIndex: 'EndDate',
                            flex: 1,
                            filter: { xtype: 'datefield' }
                        },
                        {
                            text: 'Состояние',
                            dataIndex: 'IsClosed',
                            flex: 1,
                            renderer: function (value) {
                                return value ? 'Закрыт' : 'Открыт';
                            }
                        }
                    ],
                    fieldLabel: 'Период'
                }
            ]
        });

        me.callParent(arguments);
    }
});