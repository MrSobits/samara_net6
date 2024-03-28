Ext.define('B4.view.publicservorg.contractqualitylevel.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.contractqualitylevelEditWindow',
    mixins: ['B4.mixins.window.ModalMask'],

    width: 500,
    bodyPadding: 5,
    title: 'Показатель качества',
    trackResetOnLoad: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    defaults: {
                        anchor: '100%',
                        labelWidth: 150,
                        labelAlign: 'right',
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование показателя',
                            allowBlank: false
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Value',
                            fieldLabel: 'Установленное значение показателя',
                            allowBlank: false
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'UnitMeasure',
                            fieldLabel: 'Единица измерения',
                            textProperty: 'Name',
                            store: 'B4.store.dict.UnitMeasure',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                            ]
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