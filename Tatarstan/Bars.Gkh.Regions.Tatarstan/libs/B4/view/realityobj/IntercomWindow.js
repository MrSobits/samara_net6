Ext.define('B4.view.realityobj.IntercomWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    width: 750,

    alias: 'widget.intercomwindow',
    title: 'Домофон',
    constrain: true,
    resizable: false,
    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNoNotSet',
        'B4.form.GkhMonthPicker',
        'B4.enums.IntercomUnitMeasure',
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelWidth: 150,
                labelAlign: 'right',
                margin: 5
            },
            layout: 'anchor',
            items: [
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Количество подъездных домофонов в доме (ед.)',
                    name: 'IntercomCount',
                    allowBlank: false,
                    hideTrigger: true,
                    margin: '5 15 5 5'
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'right',
                        pack: 'end'
                    },
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 'auto',
                        margin: '5 17 5 5',
                    },
                    margin: '5 15 5 5',
                    items:
                    [
                        {
                            xtype: 'numberfield',
                            fieldLabel: 'Минимальный тариф',
                            name: 'Tariff',
                            hideTrigger: true,
                            allowBlank: false
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'UnitMeasure',
                            items: B4.enums.IntercomUnitMeasure.getItems(),
                            displayField: 'Display',
                            valueField: 'Value',
                            fieldLabel: 'Ед. изм.',
                            editable: false
                        },
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Нет единого тарифа',
                            name: 'HasNotTariff'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Аналоговые домофоны',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 140
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            fieldLabel: 'Количество подъездных аналоговых домофонов (ед.)',
                            name: 'AnalogIntercomCount',
                            hideTrigger: true
                        },
                        {
                            xtype: 'gkhmonthpicker',
                            name: 'InstallationDate',
                            fieldLabel: 'Планируемая дата установки IP-домофона'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'IP домофоны',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 140
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            fieldLabel: 'Количество подъездных IP домофонов (ед.)',
                            name: 'IpIntercomCount',
                            hideTrigger: true
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'Recording',
                            items: B4.enums.YesNoNotSet.getItems(),
                            displayField: 'Display',
                            valueField: 'Value',
                            fieldLabel: 'Наличие видеозаписи',
                            editable: false
                        },
                        {
                            xtype: 'numberfield',
                            fieldLabel: 'Глубина архива видеозаписи (сут.)',
                            name: 'ArchiveDepth',
                            hideTrigger: true
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'ArchiveAccess',
                            items: B4.enums.YesNoNotSet.getItems(),
                            displayField: 'Display',
                            valueField: 'Value',
                            fieldLabel: 'Постоянный удаленный доступ к архиву у МВД РТ',
                            editable: false
                        }
                    ]
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Количество подъездов без домофонов в доме (ед.)',
                    name: 'EntranceCount',
                    hideTrigger: true,
                    margin: '5 15 5 5'
                },
                {
                    xtype: 'gkhmonthpicker',
                    name: 'IntercomInstallationDate',
                    fieldLabel: 'Планируемая дата установки IP-домофона',
                    margin: '5 15 5 5'
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});