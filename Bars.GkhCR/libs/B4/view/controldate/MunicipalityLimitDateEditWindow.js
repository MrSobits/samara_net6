Ext.define('B4.view.controldate.MunicipalityLimitDateEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 650,
    height: 150,
    minWidth: 500,
    minHeight: 100,
    bodyPadding: 5,
    alias: 'widget.controldatemunicipalitylimitdateeditwindow',
    title: 'Срок по муниципальному образованию',
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                allowBlank: false,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальное образование',
                    editable: false,
                    store: 'B4.store.dict.Municipality',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'LimitDate',
                    fieldLabel: 'Срок',
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