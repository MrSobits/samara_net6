Ext.define('B4.view.protocol197.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    itemId: 'protocol197AddWindow',
    title: 'Протокол по ст.19.7 КоАП РФ',
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
                    xtype: 'textfield',
                    name: 'DocumentNumber',
                    fieldLabel: 'Номер документа'
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