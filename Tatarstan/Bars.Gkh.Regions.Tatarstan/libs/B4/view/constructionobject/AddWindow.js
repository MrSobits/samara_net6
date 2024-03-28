Ext.define('B4.view.constructionobject.AddWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 100,
    maxHeight: 130,
    bodyPadding: 5,
    alias: 'widget.constructionobjectaddwindow',
    title: 'Объект строительства',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FiasSelectAddress',
        
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4fiasselectaddress',
                    labelWidth: 100,
                    name: 'FiasAddress',
                    flatIsReadOnly: true,
                    fieldsToHideNames: ['tfFlat'],
                    fieldsRegex: {
                        tfHousing: {
                            regex: /^\d+$/,
                            regexText: 'В это поле можно вводить только цифры'
                        },
                        tfBuilding: {
                            regex: /^\d+$/,
                            regexText: 'В это поле можно вводить только цифры'
                        }
                    },
                    fieldLabel: 'Адрес',
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