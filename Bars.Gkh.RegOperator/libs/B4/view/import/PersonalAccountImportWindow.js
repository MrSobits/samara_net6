Ext.define('B4.view.import.PersonalAccountImportWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.personalaccountimportwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 400,
    bodyPadding: 5,
    title: 'Импорт',
    resizable: false,

    requires: [
        'B4.form.FileField',
        'B4.form.EnumCombo',
        'B4.enums.BenefitsCategoryImportIdentificationType'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4filefield',
                    name: 'FileImport',
                    labelWidth: 100,
                    labelAlign: 'right',
                    fieldLabel: 'Файл',
                    allowBlank: false,
                    itemId: 'fileImport'
                },
                {
                    fieldLabel: 'Тип идентификации',
                    labelAlign: 'right',
                    name: 'IdentificationType',
                    xtype: 'b4enumcombo',
                    enumName: 'B4.enums.BenefitsCategoryImportIdentificationType',
                    includeEmpty: false,
                    enumItems: [],
                    value: B4.enums.BenefitsCategoryImportIdentificationType.getStore().first().get('Value'),
                    hideTrigger: false
                },
                {
                    xtype: 'checkbox',
                    name: 'replaceData',
                    checked: false,
                    style: 'margin-left: 10px; margin-top: 20px; ',
                    boxLabel: 'Заменять данные',
                    margin: '0 0 0 105'
                },
                {
                    xtype: 'displayfield',
                    itemId: 'log'
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