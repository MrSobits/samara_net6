Ext.define('B4.view.dict.official.EditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.administration.Operator'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    //autoScroll: true,
    trackResetOnLoad: true,
    bodyPadding: 5,
    width: 500,
    maxHeight: 150,
    minHeight: 150,

    title: 'Должностное лицо',

    alias: 'widget.officialwin',
    closeAction: 'destroy',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Operator',
                    fieldLabel: 'Оператор',
                    store: 'B4.store.administration.Operator',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'Fio',
                    fieldLabel: 'ФИО',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    allowBlank: false,
                    maxLength: 300
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});