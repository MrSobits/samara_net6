Ext.define('B4.view.Role.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.roleeditwindow',

    requires: [
        'B4.form.SelectField',
        'B4.store.Role',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
    ],

    mixins: ['B4.mixins.window.ModalMask'],

    layout: 'form',

    width: 700,
    bodyPadding: 5,

    name: 'RoleEditWindow',
    title: 'Роль',

    closeAction: 'destroy',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            fieldDefaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 200
                },
                {
                    xtype: 'checkbox',
                    name: 'IsLocalAdmin',
                    fieldLabel: 'Локальный администратор',
                },
                {
                    xtype: 'b4selectfield',
                    name: 'RoleList',
                    fieldLabel: 'Настройка ролей',
                    store: 'B4.store.Role',
                    hidden: true,
                    disabled: true,
                    editable: false,
                    allowBlank: false,
                    selectionMode: 'MULTI',
                    idProperty: 'Id',
                    textProperty: 'Name',
                    blankText: 'Это поле обязательно для заполнения',
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                },
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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