Ext.define('B4.view.dict.constructiveelement.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'constructiveElementEditWindow',
    title: 'Конструктивный элемент',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.dict.constructiveelementgroup.Grid',
        'B4.view.dict.normativedoc.Grid',
        'B4.store.dict.ConstructiveElementGroup',
        'B4.store.dict.NormativeDoc',
        'B4.store.dict.UnitMeasure'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Group',
                    fieldLabel: 'Группа',
                    listView: 'B4.view.dict.constructiveelementgroup.Grid',
                    store: 'B4.store.dict.ConstructiveElementGroup',
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'RepairCost',
                    decimalSeparator: ',',
                    fieldLabel: 'Стоимость ремонта (руб.)'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'Lifetime',
                    fieldLabel: 'Срок эксплуатации'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'NormativeDoc',
                    fieldLabel: 'Нормативный документ',
                    listView: 'B4.view.dict.normativedoc.Grid',
                    store: 'B4.store.dict.NormativeDoc'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'UnitMeasure',
                    fieldLabel: 'Единица измерения',
                    listView: 'B4.view.dict.UnitMeasure.Grid',
                    store: 'B4.store.dict.UnitMeasure'
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
                            items: [ { xtype: 'b4savebutton' } ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [ { xtype: 'b4closebutton' } ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});