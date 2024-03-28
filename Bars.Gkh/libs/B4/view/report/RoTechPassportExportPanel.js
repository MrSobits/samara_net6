Ext.define('B4.view.report.RoTechPassportExportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.rotechpassportexportpanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'fieldset',
                    title: 'Тип дома',
                    padding: '0 5 0 0 ',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 195
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'typeManyApartments',
                                    fieldLabel: 'Многоквартирный',
                                    flex: 1,
                                    checked: true
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'typeIndividual',
                                    fieldLabel: 'Индивидуальный',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 195
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'typeSocialBehavior',
                                    fieldLabel: 'Общежитие',
                                    flex: 1
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'typeBlockedBuilding',
                                    fieldLabel: 'Блокированной застроки',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 195
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'typeNotSet',
                                    fieldLabel: 'Не задано',
                                    flex: 1
                                },
                                {
                                    xtype: 'component',
                                    flex: 1
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});