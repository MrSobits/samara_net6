Ext.define('B4.view.program.thirddetails.ThirdStageDetails', {
    extend: 'Ext.form.Panel',

    requires: ['B4.ux.breadcrumbs.Breadcrumbs'],
    alias: 'widget.thirdstagepanel',

    title: 'Объект КР',
    closable: true,
    layout: { type: 'vbox', align: 'stretch' },

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            items: [
                {
                    xtype: 'breadcrumbs',
                    itemId: 'thirddetailsinfolabel'
                },
                {
                    xtype: 'form',
                    layout: 'hbox',
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            margins: '5 5 0 5',
                            flex: 1,
                            items: [
                                {
                                    xtype: 'textfield',
                                    labelWidth: 200,
                                    name: 'IndexNumber',
                                    fieldLabel: 'Номер очередности в программе',
                                    readOnly: true
                                },
                                {
                                    xtype: 'textfield',
                                    labelWidth: 200,
                                    name: 'Year',
                                    fieldLabel: 'Плановый год КР',
                                    readOnly: true
                                },
                                {
                                    xtype: 'textfield',
                                    labelWidth: 200,
                                    name: 'WorkSum',
                                    fieldLabel: 'Сумма по работам (руб.)',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            margins: '5 5 0 5',
                            flex: 1,
                            items: [
                                {
                                    xtype: 'textfield',
                                    labelWidth: 200,
                                    name: 'Point',
                                    fieldLabel: 'Балл очередности',
                                    readOnly: true,
                                    hidden: true
                                },
                                {
                                    xtype: 'textfield',
                                    labelWidth: 200,
                                    name: 'ServiceAndWorkSum',
                                    fieldLabel: 'Сумма по работам, в т.ч. услуги (руб.)',
                                    readOnly: true
                                },
                                {
                                    xtype: 'textfield',
                                    labelWidth: 200,
                                    name: 'ServiceSum',
                                    fieldLabel: 'Сумма по услугам (руб.)',
                                    readOnly: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    layout: 'fit',
                    items: [
                        { title: 'Объекты общего имущества', xtype: 'thirddetailscommonestatetree' },
                        { title: 'Виды работ', xtype: 'thirddetailsworktypegrid' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});