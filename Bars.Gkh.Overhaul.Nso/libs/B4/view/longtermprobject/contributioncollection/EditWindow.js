Ext.define('B4.view.longtermprobject.contributioncollection.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.contributioncollectioneditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    height: 170,
    minHeight: 100,
    width: 650,
    minWidth: 600,
    bodyPadding: 5,
    title: 'Показатель сбора взносов на КР',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 230
            },
            items: [
                {
                    xtype: 'datefield',
                    format: 'm.Y',
                    name: 'Date',
                    padding: '0 0 5 0',
                    fieldLabel: 'Месяц'
                },
                {
                    xtype: 'textfield',
                    name: 'PersonalAccount',
                    padding: '0 0 5 0',
                    fieldLabel: 'Лицевой счет собственника дома'
                },
                {
                    xtype: 'textfield',
                    name: 'AreaOwnerAccount',
                    padding: '0 0 5 0',
                    fieldLabel: 'Площадь помещения собственника ЛС'
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