Ext.define('B4.view.gisrealestatetype.IndicatorGridEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.indicatorgrideditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    height: 170,
    width: 500,
    bodyPadding: 5,
    requires: [
        'B4.enums.GisTypeIndicator',
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],
    title: 'Выбор индикатора для услуги',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 120,
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Service',
                    fieldLabel: 'Наименование',
                    store: 'B4.store.dict.Service',
                    editable: false,
                    padding: '10 5 10 0',
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Код',
                            dataIndex: 'Code',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'GisTypeIndicator',
                    fieldLabel: 'Индикатор',
                    enumName: 'B4.enums.GisTypeIndicator',
                    includeEmpty: false,
                    padding: '0 5 10 0',
                    enumItems: [],
                    hideTrigger: false
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
                        '->',
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