Ext.define('B4.view.objectcr.FilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.objectcrfilterpnl',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    border: false,
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    border: false,
                    width: 800,
                    padding: '5 0 0 0',
                    layout: 'hbox',
                    defaults: {
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'tfProgramCr',
                            itemId: 'tfProgramCr',
                            fieldLabel: 'Программа КР',
                            width: 500
                        },
                        {
                            width: 100,
                            xtype: 'component'
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbbuildContr',
                            boxLabel: 'Заключены договора',
                            labelAlign: 'right',
                            checked: false,
                        }
                       
                    ]
                },
                {
                    xtype: 'container',
                    border: false,
                    width: 1000,
                    padding: '5 0 0 0',
                    layout: 'hbox',
                    defaults: {
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'tfMunicipality',
                            itemId: 'tfMunicipality',
                            fieldLabel: 'Муниципальные районы',
                            width: 500
                        },
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'tfWorks',
                            itemId: 'tfWorks',
                            fieldLabel: 'Виды работ',
                            width: 500
                        }

                    ]
                }        
            ]
        });

        me.callParent(arguments);
    }
});