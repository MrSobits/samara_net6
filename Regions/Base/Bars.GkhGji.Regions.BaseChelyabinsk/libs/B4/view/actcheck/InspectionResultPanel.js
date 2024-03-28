Ext.define('B4.view.actcheck.InspectionResultPanel', {
    extend: 'Ext.panel.Panel',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    alias: 'widget.actcheckinspectionresultpanel',
    style: 'background: none repeat scroll 0 0 #DFE9F6',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.view.actcheck.RealityObjectGrid'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: 5,
                    defaults: {
                        xtype: 'textarea',
                        labelAlign: 'right',
                        labelWidth: 170,
                        flex: 1
                    },
                    style: 'background: none repeat scroll 0 0 #DFE9F6',
                    items: [
                        {
                            name: 'ViolationDescription',
                            fieldLabel: 'Описание нарушения',
                            height: 100
                        },
                        {
                            name: 'PersonViolationInfo',
                            fieldLabel: 'Сведения о лицах допустивших нарушения',
                            height: 100
                        },
                        {
                            name: 'PersonViolationActionInfo',
                            fieldLabel: 'Сведения о том, что нарушения допущены'
                                + ' в результате виновных действий (бездействия)'
                                + ' должностных лиц и (или) работников проверяемого лица',
                            height: 100,
                            labelWidth: 200
                        }
                    ]
                },
                {
                    xtype: 'actCheckRealityObjectGrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});