Ext.define('B4.view.rapidresponsesystem.AppealResponsePanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.appealresponsepanel',

    itemId: 'appealResponsePanel',
    trackResetOnLoad: true,
    bodyStyle: Gkh.bodyStyle,
    border: false,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.form.FileField',
        'B4.enums.TypeCorrespondent'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                    },
                    margin: '0 0 5 0',
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'ResponseDate',
                            fieldLabel: 'Дата ответа',
                            format: 'd.m.Y',
                            flex: 0.4
                        },
                        {
                            xtype: 'container',
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'Theme',
                    fieldLabel: 'Тема',
                    maxLength: 255
                },
                {
                    xtype: 'textarea',
                    name: 'Response',
                    fieldLabel: 'Ответ',
                    maxLength: 8000
                },
                {
                    xtype: 'textarea',
                    name: 'CarriedOutWork',
                    fieldLabel: 'Проведенные работы',
                    maxLength: 2000
                },
                {
                    xtype: 'b4filefield',
                    name: 'ResponseFile',
                    fieldLabel: 'Файл'
                }
            ]});

        me.callParent(arguments);
    }
});