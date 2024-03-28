Ext.define('B4.view.Permission.CopyRoleWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.copyrolewindow',

    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 500,
    width: 500,
    resizable: false,
    title: 'Копирование настроек в выбранную роль',

    closeAction: 'destroy',
    modal: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.Role',
        'B4.form.SelectField',
        'B4.form.ComboBox'
    ],

    roleStore: null,

    initComponent: function () {
        var me = this,
            store = me.roleStore || Ext.create('B4.store.Role');

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 40
            },
            items: [
                {
                    xtype: 'container',
                    style: 'border: none; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">' +
                        'Выберите Роль, в которую будут скопированы настройки ограничений' +
                        '</span>'
                },
                {
                    xtype: 'b4combobox',
                    name: 'Role',
                    editable: false,
                    fieldLabel: 'Роль',
                    store: store,
                    pageSize: 30,
                    triggerAction: 'all',
                    valueField: 'Id',
                    displayField: 'Name',
                    maxWidth: 450,
                    margins: '10 15 10'
                }
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