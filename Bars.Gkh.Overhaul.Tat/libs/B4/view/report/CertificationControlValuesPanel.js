Ext.define('B4.view.report.CertificationControlValuesPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reportCertificationControlValuesPanel',
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
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'HouseTypes',
                    itemId: 'tfHouseType',
                    fieldLabel: 'Тип дома',
                    emptyText: 'Все'
                },
                {
                    xtype: 'fieldset',
                    title: 'Cостояние дома',
                    padding: '0 5 0 0 ',
                    layout: {
                        type: 'vbox'
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
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'emergency',
                                    fieldLabel: 'Аварийный',
                                    itemId: 'cbEmergency',
                                    flex: 1
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'dilapidated',
                                    fieldLabel: 'Ветхий',
                                    itemId: 'cbDilapidated',
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
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'serviceable',
                                    fieldLabel: 'Исправный',
                                    itemId: 'cbServiceable',
                                    flex: 1
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'razed',
                                    fieldLabel: 'Cнесен',
                                    itemId: 'cbRazed',
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