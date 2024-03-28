Ext.define('B4.view.gasuindicator.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.gasuindicatoreditwin',
    layout: {type: 'vbox', align: 'stretch'},
    width: 600,
    bodyPadding: 5,
    title: 'Показатель ГАСУ',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.Periodicity',
        'B4.enums.EbirModule',
        'B4.store.dict.UnitMeasure',
        'B4.form.SelectField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 110,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    flex: 1
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    minLength: 9,
                    maxLength: 9,
                    fieldLabel: 'Код',
                    flex: 1
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    name: 'Periodicity',
                    fieldLabel: 'Периодичность',
                    displayField: 'Display',
                    store: B4.enums.Periodicity.getStore(),
                    valueField: 'Value'
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    name: 'EbirModule',
                    fieldLabel: 'Модуль ЕБИР',
                    displayField: 'Display',
                    store: B4.enums.EbirModule.getStore(),
                    valueField: 'Value'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'UnitMeasure',
                    textProperty: 'ShortName',
                    store: 'B4.store.dict.UnitMeasure',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Сокр. наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    fieldLabel: 'Ед.измерения',
                    editable: false,
                    allowBlank: false
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