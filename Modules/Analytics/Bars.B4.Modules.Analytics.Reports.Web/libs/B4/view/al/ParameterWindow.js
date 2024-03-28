Ext.define('B4.view.al.ParameterWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.parameterwindow',
    closeAction: 'destroy',
    width: 500,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.enums.al.ParamType'
    ],
    bodyPadding: 5,
    title: 'Параметры',

    modal: true,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            defaults: {
                labelAlign: 'right',
                anchor: '100%',
                labelWidth: 130
            },
            items: [
                {
                    xtype: 'hidden',
                    name: 'Id'
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Название',
                    name: 'Label'
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Системное название',
                    name: 'Name'
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'ParamType',
                    enumName: 'B4.enums.al.ParamType',
                    fieldLabel: 'Тип поля'
                },
                {
                    xtype: 'checkbox',
                    name: 'Required',
                    margin: '0 0 0 135',
                    labelStyle: 'vertical-align: 5px;',
                    boxLabel: 'Обязательность'
                },
                {
                    xtype: 'checkbox',
                    name: 'Multiselect',
                    margin: '0 0 0 135',
                    labelStyle: 'vertical-align: 5px;',
                    boxLabel: 'Множественный выбор',
                    disabled: true
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
                                    xtype: 'b4savebutton',
                                    text: 'Добавить'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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