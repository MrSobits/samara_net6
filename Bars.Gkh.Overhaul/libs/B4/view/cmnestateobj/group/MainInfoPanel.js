Ext.define('B4.view.cmnestateobj.group.MainInfoPanel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.structelgroupmaininfopanel',
    
    requires: [
        'B4.ux.button.Save',

        'B4.view.cmnestateobj.group.attributes.Grid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Общие сведения',
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    margin: -1,
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            anchor: '100%',
                            maxLength: 500,
                            allowBlank: false
                        },
                        {
                            xtype: 'checkbox',
                            fieldLabel: 'Является обязательным',
                            name: 'Required',
                            anchor: '100%'
                        },
                        {
                            xtype: 'checkbox',
                            fieldLabel: 'Используется в расчете ДПКР',
                            name: 'UseInCalc',
                            anchor: '100%'
                        },
                        {
                            xtype: 'textarea',
                            name: 'FormulaDuplicate',
                            fieldLabel: 'Формула',
                            anchor: '100%',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'groupattributesgrid',
                    flex: 1,
                    margin: -1,
                    disabled: true
                }
            ]
        });

        me.callParent(arguments);
    }
});