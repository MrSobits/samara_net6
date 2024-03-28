Ext.define('B4.view.resolpros.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    itemId: 'resolProsAddWindow',
    title: 'Постановление прокуратуры',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Municipality',
                    name: 'Municipality',
                    fieldLabel: 'МР органа прокуратуры',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                },
                {
                    xtype: 'textfield',
                    name: 'Official',
                    fieldLabel: 'Должностное лицо',
                    maxLength: 100
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4combobox',
                    itemId: 'cbExecutant',
                    name: 'Executant',
                    editable: false,
                    fieldLabel: 'Тип исполнителя',
                    fields: ['Id', 'Name', 'Code'],
                    url: '/ExecutantDocGji/List',
                    queryMode: 'local',
                    triggerAction: 'all'
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