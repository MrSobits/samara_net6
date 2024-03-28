Ext.define('B4.view.dict.templateotherservice.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    width: 800,
    height: 450,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Услуга',
    alias: 'widget.templateotherserviceeditwindow',
    layout: { type: 'vbox', align: 'stretch' },
    closable: false,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.store.dict.UnitMeasure'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 100
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            fieldLabel: 'Код',
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'Characteristic',
                            fieldLabel: 'Характеристика',
                            maxLength: 300
                        },
                        {
                            xtype: 'b4selectfield',
                            editable: false,
                            name: 'UnitMeasure',
                            fieldLabel: 'Ед. измерения',
                            anchor: '100%',
                            store: 'B4.store.dict.UnitMeasure'
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
                            columns: 1,
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
