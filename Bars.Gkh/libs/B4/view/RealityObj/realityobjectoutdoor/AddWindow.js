Ext.define('B4.view.realityobj.realityobjectoutdoor.AddWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    minWidth: 500,
    minHeight: 120,
    bodyPadding: 5,

    alias: 'widget.realityobjectoutdooraddwindow',
    title: 'Добавление двора',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 130,
                allowBlank: false,
                maxLength: 255
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'MunicipalityFiasOktmo',
                    fieldLabel: 'Населенный пункт',
                    textProperty: 'OffName',
                    store: 'B4.store.dict.MunicipalityFiasOktmo',
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Населенный пункт',
                            dataIndex: 'OffName',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Муниципальное образование',
                            dataIndex: 'Municipality',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование двора'
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код двора'
                },
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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
                            columns: 1,
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