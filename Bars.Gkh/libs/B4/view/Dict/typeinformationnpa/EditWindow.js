Ext.define('B4.view.dict.typeinformationnpa.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'typeInformationNpaEditWindow',
    title: 'Тип информации НПА',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.store.dict.CategoryInformationNpa'
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
                    fieldLabel: 'Тип информации',
                    allowBlank: false,
                    maxLength: 100
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    allowBlank: false,
                    maxLength: 255
                },
                {
                    xtype: 'b4enumcombo',
                    fieldLabel: 'Категория',
                    name: 'Category',
                    enumName: 'B4.enums.CategoryInformationNpa',
                    allowBlank: false,
                    editable: false,
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
                            columns: 1,
                            items: [ { xtype: 'b4savebutton' } ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [ { xtype: 'b4closebutton' } ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});