﻿Ext.define('B4.view.chargessplitting.contrpersumm.FilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.contrpersummfilterpanel',

    closable: false,
    header: false,
    layout: 'hbox',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    border: false,
    height: 40,
    width: '100%',

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
                    windowContainerSelector: 'contractperiodsummarygrid',
                    textProperty: 'Name',
                    labelWidth: 120,
                    labelAlign: 'right',
                    fieldLabel: 'Отчетный период',
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
                    windowContainerSelector: 'contractperiodsummarygrid',
                    textProperty: 'Name',
                    labelWidth: 120,
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
                    windowContainerSelector: 'contractperiodsummarygrid',
                    textProperty: 'Name',
                    labelWidth: 120,
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
                    store: 'B4.store.ManagingOrganization',
                    selectionMode: 'MULTI',
                    windowCfg: { modal: true },
                    windowContainerSelector: 'contractperiodsummarygrid',
                    textProperty: 'ContragentName',
                    labelWidth: 120,
                    labelAlign: 'right',
                    fieldLabel: 'УО',
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
                            dataIndex: 'ContragentInn',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    name: 'ManagingOrganization'
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.PublicServiceOrg',
                    selectionMode: 'MULTI',
                    windowCfg: { modal: true },
                    windowContainerSelector: 'contractperiodsummarygrid',
                    textProperty: 'ContragentName',
                    labelWidth: 120,
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
            ]
        });

        me.callParent(arguments);
    }
});