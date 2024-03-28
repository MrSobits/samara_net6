Ext.define('B4.view.persaccbenefits.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.persaccbenefitseditwindow',

    modal: true,

    width: 550,
    height: 400,
    bodyPadding: 5,
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.ChangeValue',
        'B4.view.persaccbenefits.HistoryGrid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Сумма начисленной льготы',

    initComponent: function() {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            xtype: 'panel',
                            border: false,
                            title: 'Общие сведения',
                            bodyStyle: Gkh.bodyStyle,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '5 0 0 0',
                                    defaults: {
                                        labelWidth: 200
                                    },
                                    items: [
                                        {
                                            fieldLabel: 'Сумма начисленной льготы',
                                            xtype: 'numberfield',
                                            name: 'Sum',
                                            editable: false,
                                            hideTrigger: true,
                                            allowDecimals: true,
                                            decimalSeparator: ',',
                                            decimalPrecision: 2,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'changevalbtn',
                                            margins: '0 0 0 5',
                                            flex: 1,
                                            className: 'PersonalAccountBenefits',
                                            valueFieldXtype: 'numberfield',
                                            propertyName: 'Sum',
                                            decimalSeparator: ',',
                                            decimalPrecision: 2,
                                            allowDecimals: true,
                                            onValueSaved: function(val) {
                                                var field = this.up('container').down('numberfield[name=Sum]');
                                                field.setValue(val);
                                            }
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'persaccbenefitshistorygrid',
                            border: false
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
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    text: 'Закрыть'
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