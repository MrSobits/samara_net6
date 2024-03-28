Ext.define('B4.view.chargessplitting.fuelenergyresrc.FilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.fuelenergyresourcecontractfilterpanel',

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
        'B4.form.SelectField'
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
                    windowContainerSelector: 'fuelenergyresourcecontractgrid',
                    textProperty: 'Name',
                    maxWidth: 300,
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
                    store: 'B4.store.dict.Municipality',
                    selectionMode: 'MULTI',
                    windowCfg: { modal: true },
                    windowContainerSelector: 'fuelenergyresourcecontractgrid',
                    textProperty: 'Name',
                    maxWidth: 500,
                    labelWidth: 130,
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
                    store: 'B4.store.PublicServiceOrg',
                    selectionMode: 'MULTI',
                    windowCfg: { modal: true },
                    windowContainerSelector: 'fuelenergyresourcecontractgrid',
                    textProperty: 'ContragentName',
                    maxWidth: 500,
                    labelWidth: 60,
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