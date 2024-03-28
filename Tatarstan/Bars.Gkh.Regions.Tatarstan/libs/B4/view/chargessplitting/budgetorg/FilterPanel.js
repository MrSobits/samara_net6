Ext.define('B4.view.chargessplitting.budgetorg.FilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.budgetorgfilterpanel',

    closable: false,
    header: false,
    layout: 'hbox',
    autoScroll: true,
    frame: true,
    border: false,
    height: 40,
    width: '100%',

    bodyPadding: '3px 0 0 0',

    requires: [
        'B4.form.SelectField',
        'B4.enums.TypeServiceGis'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 60,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.chargessplitting.contrpersumm.ContractPeriod',
                    selectionMode: 'SINGLE',
                    windowCfg: { modal: true },
                    windowContainerSelector: 'budgetorggrid',
                    textProperty: 'Name',
                    labelWidth: 50,
                    flex: .7,
                    labelAlign: 'right',
                    fieldLabel: 'Период',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    name: 'Period'
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Service',
                    selectionMode: 'MULTI',
                    windowCfg: { modal: true },
                    windowContainerSelector: 'budgetorggrid',
                    textProperty: 'Name',
                    labelWidth: 50,
                    flex: .7,
                    labelAlign: 'right',
                    fieldLabel: 'Услуга',
                    editable: false,
                    onSelectAll: function () {
                        var me = this;

                        me.setValue('All');
                        me.updateDisplayedText('Выбраны все');
                        me.selectWindow.hide();
                    },
                    columns: [
                        {
                            text: 'Код',
                            dataIndex: 'Code',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    name: 'Service',
                    listeners: {
                        beforeload: function (asp, options, store) {
                            options.params.typeGroupServiceDi = B4.enums.TypeServiceGis.Communal;
                        }
                    }
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Municipality',
                    selectionMode: 'MULTI',
                    windowCfg: { modal: true },
                    windowContainerSelector: 'budgetorggrid',
                    textProperty: 'Name',
                    labelWidth: 140,
                    labelAlign: 'right',
                    fieldLabel: 'Муниципальные район',
                    editable: false,
                    onSelectAll: function () {
                        var me = this;

                        me.setValue('All');
                        me.updateDisplayedText('Выбраны все');
                        me.selectWindow.hide();
                    },
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    name: 'Municipality'
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.Contragent',
                    selectionMode: 'MULTI',
                    windowCfg: { modal: true },
                    windowContainerSelector: 'budgetorggrid',
                    textProperty: 'Name',
                    labelWidth: 100,
                    labelAlign: 'right',
                    fieldLabel: 'Организация',
                    editable: false,
                    onSelectAll: function () {
                        var me = this;

                        me.setValue('All');
                        me.updateDisplayedText('Выбраны все');
                        me.selectWindow.hide();
                    },
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'ИНН',
                            dataIndex: 'Inn',
                            flex: .5,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'КПП',
                            dataIndex: 'Kpp',
                            flex: .5,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    name: 'Organization'
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.PublicServiceOrg',
                    selectionMode: 'MULTI',
                    windowCfg: { modal: true },
                    windowContainerSelector: 'budgetorggrid',
                    textProperty: 'ContragentName',
                    labelWidth: 50,
                    labelAlign: 'right',
                    fieldLabel: 'РСО',
                    editable: false,
                    onSelectAll: function () {
                        var me = this;

                        me.setValue('All');
                        me.updateDisplayedText('Выбраны все');
                        me.selectWindow.hide();
                    },
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'ContragentName',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'ИНН',
                            dataIndex: 'Inn',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    name: 'PublicServiceOrg',

                    listeners: {
                        beforeload: function(asp, operation, store) {
                            operation.params.operatorHasContragent = true;
                        }
                    }
                }
            ],

            getFilterParams: function () {
                var periodId = me.down('[name=Period]').getValue(),
                    serviceIds = me.down('[name=Service]').getValue(),
                    municipalityIds = me.down('[name=Municipality]').getValue(),
                    orgIds = me.down('[name=Organization]').getValue(),
                    pubServOrgIds = me.down('[name=PublicServiceOrg]').getValue();

                return {
                    periodId: periodId,
                    serviceIds: serviceIds === 'All' ? null : Ext.encode(serviceIds),
                    municipalityIds: municipalityIds === 'All' ? null : Ext.encode(municipalityIds),
                    orgIds: orgIds === 'All' ? null : Ext.encode(orgIds),
                    pubServOrgIds: pubServOrgIds === 'All' ? null : Ext.encode(pubServOrgIds)
                };
            }
        });

        me.callParent(arguments);
    }
});