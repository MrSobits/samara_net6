Ext.define('B4.view.manorg.RegistryEdit', {
    extend: 'B4.form.Window',
    alias: 'widget.manorgregedit',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',

        'B4.enums.TsjInfoType',

        'B4.form.EnumCombo'
    ],
    mixins: ['B4.mixins.window.ModalMask'],

    title: 'Редактирование реестра',

    width: 500,
    height: 240,
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function() {
        var me = this;

        Ext.apply(me, {
            defaults: {
                allowBlank: false,
                labelWidth: 220,
                margin: '5 5 5 5',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'hidden',
                    name: 'Id',
                    margin: '0 0 0 0'
                },
                {
                    xtype: 'hidden',
                    name: 'ManagingOrganization',
                    margin: '0 0 0 0'
                },
                {
                    xtype: 'datefield',
                    fieldLabel: 'Дата предоставления сведений',
                    name: 'InfoDate'
                },
                {
                    xtype: 'b4enumcombo',
                    fieldLabel: 'Тип сведений',
                    name: 'InfoType',
                    includeEmpty: false,
                    enumName: 'B4.enums.TsjInfoType'
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Регистрационный номер',
                    name: 'RegNumber'
                },
                {
                    xtype: 'datefield',
                    fieldLabel: 'Дата внесения записи в ЕГРЮЛ',
                    name: 'EgrulDate',
                    allowBlank: true
                },
                {
                    xtype: 'b4filefield',
                    fieldLabel: 'Документ',
                    name: 'Doc'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        { xtype: 'b4savebutton' },
                        { xtype: 'tbfill' },
                        { xtype: 'b4closebutton' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});