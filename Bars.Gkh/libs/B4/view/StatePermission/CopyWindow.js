Ext.define('B4.view.StatePermission.CopyWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.statepermissioncopywindow',

    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 500,
    width: 500,
    resizable: false,
    title: 'Копирование настроек ограничений по статусам',

    closeAction: 'destroy',
    modal: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.Role',
        'B4.store.StateByType',
        'B4.form.SelectField',
        'B4.form.ComboBox'
    ],

    roleStore: null,
    statusStore: null,

    initComponent: function () {
        var me = this,
            roleStore = me.roleStore || Ext.create('B4.store.Role'),
            statusStore = me.statusStore || Ext.create('B4.store.StateByType');

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 40,
                maxWidth: 450
            },
            items: [
                {
                    xtype: 'container',
                    style: 'border: none; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">' +
                        'Выберите Роль и Статус, в которые будут скопированы настройки ограничений для данного типа объекта' +
                        '</span>'
                },
                {
                    xtype: 'b4combobox',
                    name: 'Role',
                    editable: false,
                    fieldLabel: 'Роль',
                    store: roleStore,
                    pageSize: 30,
                    triggerAction: 'all',
                    valueField: 'Id',
                    displayField: 'Name',
                    margins: '10 15 0'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'State',
                    editable: false,
                    fieldLabel: 'Статус',
                    store: statusStore,
                    idProperty: 'Id',
                    textProperty: 'Name',
                    selectionMode: 'MULTI',
                    margins: '10 15 10',
                    columns: [
                        {
                            dataIndex: 'Name',
                            text: 'Наименование',
                            flex: 1
                        }
                    ]
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