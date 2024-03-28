Ext.define('B4.view.reformarestore.Panel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField',
        'B4.form.EnumCombo',
        'B4.enums.RestoreType'
    ],

    title: 'Восстановление данных Реформы',
    alias: 'widget.restorepanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelAlign: 'right',
                allowBlank: false,
                layout: {
                    type: 'anchor'
                }
            },
            items: [
                {
                    xtype: 'container',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите восстанавливаемые данные. Допустимые типы файлов: csv.'
                },
                {
                    xtype: 'form',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    name: 'RestoreForm',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4enumcombo',
                            enumName: B4.enums.RestoreType,
                            name: 'RestoreType',
                            fieldLabel: 'Тип восстановления',
                            allowBlank: false,
                            width: 320,
                            labelWidth: 120
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'RestoreFile',
                            fieldLabel: 'Файл',
                            allowBlank: false,
                            width: 500,
                            flex: 1,
                            possibleFileExtensions: 'csv',
                            labelWidth: 45
                        },
                        {
                            xtype: 'button',
                            text: 'Загрузить',
                            tooltip: 'Загрузить',
                            iconCls: 'icon-accept',
                            style: 'margin-left: 10px'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});