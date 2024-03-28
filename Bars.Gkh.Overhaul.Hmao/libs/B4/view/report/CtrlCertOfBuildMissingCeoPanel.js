Ext.define('B4.view.report.CtrlCertOfBuildMissingCeoPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reportCtrlCertOfBuildMissingCeoPanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.enums.TypeHouse'
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
                                    itemId: 'cbTypeManyApartments',
                                    flex:1,
                                    checked:true
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'typeIndividual',
                                    fieldLabel: 'Индивидуальный',
                                    itemId: 'cbTypeIndividual',
                                    flex:1
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
                                    itemId: 'cbTypeSocialBehavior',
                                    flex: 1
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'typeBlockedBuilding',
                                    fieldLabel: 'Блокированной застроки',
                                    itemId: 'cbTypeBlockedBuilding',
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