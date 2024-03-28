Ext.define('B4.view.paramsgji.Panel', {
    extend: 'Ext.form.Panel',
    closable: true,
    alias: 'widget.paramsgjipanel',

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Save'      
    ],

    title: 'Настройка параметров',
    
    layout: {
        type: 'vbox',
        align: 'left'
    },

    defaults: {
        labelWidth: 300,
        labelAlign: 'right',
        width: 800,
        margin: '5 0'
    },

    initComponent: function() {
        var me = this,
            violLevelStore = Ext.create('Ext.data.Store', {
                fields: ['display', 'value'],
                data: [
                    { 'display': 1, 'value': 1 },
                    { 'display': 2, 'value': 2 }
                ]
            });

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            bodyPadding: 5,
            items: [
                {
                    xtype: 'b4combobox',
                    fieldLabel: 'Количество уровней группировки нарушений',
                    displayField: 'display',
                    valueField: 'value',
                    name: 'ViolationLevelCount',
                    editable: false,
                    store: violLevelStore
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
                                    xtype: 'b4savebutton'
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