Ext.define('B4.view.efficiencyrating.FactorForm', {
    extend: 'Ext.form.Panel',
    alias: 'widget.efFactorFormPanel',

    requires: [],
    border: null,
    closable: false,
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: '5',
    layout: 'form',

    initComponent: function() {
        var me = this;

        Ext.apply(me,
        {
            items: [
                {
                    xtype: 'form',
                    name: 'efFactorForm',
                    flex: 1,
                    bodyStyle: Gkh.bodyStyle,
                    border: null,
                    layout: { type: 'vbox', align: 'stretch' },

                    defaults: {
                        xtype: 'textfield',
                        labelAlign: 'right',
                        labelWidth: 190,
                        flex: 1,
                        allowBlank: false
                    },

                    items: [
                        {
                            fieldLabel: 'Наименование фактора',
                            name: 'Name',
                            maxLength: 255

                        },
                        {
                            fieldLabel: 'Код фактора',
                            name: 'Code',
                            regex: /^[A-ZА-Я]{1,3}[a-zа-я0-9]{0,3}$/,
                            regexText:
                                'Формат ввода код: AAAxxx, где AAA - русские или латинские буквы, ххх - русские или латинские буквы, числа'
                        }
                    ]
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
                            items: [
                                {
                                    xtype: 'b4savebutton'
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