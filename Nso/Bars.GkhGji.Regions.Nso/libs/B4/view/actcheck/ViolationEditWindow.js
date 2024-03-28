Ext.define('B4.view.actcheck.ViolationEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    bodyPadding: 5,
    title: 'Форма редактирования нарушения',
    closeAction: 'hide',
    trackResetOnLoad: true,
    alias: 'widget.actcheckviolationwin',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'CodesPin',
                    fieldLabel: 'Пункты НПД',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    name: 'ViolationGjiName',
                    fieldLabel: 'Текст нарушения',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    name: 'Features',
                    fieldLabel: 'Характеристика нарушения',
                    readOnly: true
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание нарушения'
                },
                {
                    xtype: 'datefield',
                    name: 'DatePlanRemoval',
                    fieldLabel: 'Срок устранения',
                    format: 'd.m.Y'
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