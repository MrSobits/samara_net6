Ext.define('B4.view.riscontragentrole.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.riscontragentroleeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    height: 600,
    bodyPadding: 5,
    title: 'Роли контрагента РИС',
    closeAction: 'hide',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.riscontragentrole.RoleGrid',
        'B4.store.RisContragent'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    store: 'B4.store.RisContragent',
                    textProperty: 'FullName',
                    fieldLabel: 'Контрагент РИС',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        {
                            text: 'Наименование',
                            flex: 2,
                            dataIndex: 'FullName',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'ОГРН',
                            flex: 1,
                            dataIndex: 'Ogrn',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Юридический адрес',
                            flex: 3,
                            dataIndex: 'JuridicalAddress',
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'riscontragentrolegrid',
                    flex: 1
                }
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