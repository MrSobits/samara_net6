Ext.define('B4.view.frgufunction.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.frgufunctioneditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    bodyPadding: 5,
    itemId: 'frgufunctionEditWindow',
    title: 'Функции из ФРГУ',
    closeAction: 'hide',

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование контрольно-надзорной функции из ФРГУ',
                    maxLength: 1024
                },
                {
                    xtype: 'textfield',
                    name: 'FrguId',
                    fieldLabel: 'Идентификатор контрольно-надзорной функции из ФРГУ',
                    maxLength: 255
                },
                {
                    xtype: 'textfield',
                    name: 'Guid',
                    fieldLabel: 'Идентификатор контрольно-надзорной функции формата GUID',
                    regex: /^[0-9A-Fa-f]{8}-([0-9A-Fa-f]{4}-){3}[0-9A-Fa-f]{12}$/,
                    regexText: 'Введен некорректный GUID'
                },
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [ { xtype: 'b4savebutton' } ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [ { xtype: 'b4closebutton' } ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});