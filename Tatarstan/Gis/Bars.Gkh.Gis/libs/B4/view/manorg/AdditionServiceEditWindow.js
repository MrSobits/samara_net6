Ext.define('B4.view.manorg.AdditionServiceEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.manorgadditionserviceeditwindow',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minWidth: 600,
    bodyPadding: 5,
    closeAction: 'hide',

    title: 'Дополнительная услуга',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 120
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'BilService',
                    fieldLabel: 'Наименование',
                    width: 150,
                    store: 'B4.store.dict.service.BilServiceDictionaryAdditional',
                    editable: true,
                    allowBlank: false,
                    textProperty: 'ServiceName',
                    columns: [
                        { text: 'Наименование', dataIndex: 'ServiceName', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Единица измерения', dataIndex: 'MeasureName', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'MeasureName',
                    fieldLabel: 'Единица измерения',
                    readOnly: true
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