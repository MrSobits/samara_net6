﻿Ext.define('B4.view.version.MassYearChangeWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.versmassyearchangewin',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    bodyPadding: 5,
    minWidth: 450,
    title: 'Массовое изменение года записей',
    closable: false,
    requires: [
        'B4.view.Control.GkhTriggerField',
        
        'B4.ux.button.Close'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            items: [
                    {
                        xtype: 'numberfield',
                        name: 'NewYear',
                        fieldLabel: 'Изменить текущий год на:',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 2014,
                        maxValue: 3000
                    },
                    {
                        xtype: 'gkhtriggerfield',
                        name: 'versRecForChange',
                        fieldLabel: 'Изменяемые записи'
                    }
    
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Изменить год',
                                    tooltip: 'Изменить год',
                                    action: 'changeyear',
                                    iconCls: 'icon-accept'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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