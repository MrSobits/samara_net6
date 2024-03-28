Ext.define('B4.view.realityobj.structelement.Panel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.structelementpanel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 800,
    minWidth: 800,
    //layout: 'anchor',
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    title: 'Конструктивные характеристики',
    //trackResetOnLoad: true,
    //autoScroll: true,
    requires: [
        'B4.view.realityobj.structelement.Grid',
        'B4.view.realityobj.missingceo.Grid',
        'B4.ux.button.Save',
        'B4.enums.TypePresence',
        'B4.view.realityobj.structelement.HistoryGrid'
    ],

    initComponent: function () {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'Points',
                            fieldLabel: 'Всего баллов',
                            width: 400,
                            minValue: 0,
                            maxValue: 100,
                            allowDecimals: false,
                            hideTrigger: true,
                            editable: false
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    itemId: 'fsHasDocs',
                    title: 'Наличие документации',
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'ProjectDocs',
                            fieldLabel: 'Наличие проектной документации',
                            displayField: 'Display',
                            valueField: 'Value',
                            store: B4.enums.TypePresence.getStore(),
                            labelWidth: 250,
                            width: 400,
                            itemId: 'cbHasProjectDoc',
                            margin: '0 5 10 0',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'combobox',
                            name: 'EnergyPassport',
                            fieldLabel: 'Наличие энергетического паспорта',
                            displayField: 'Display',
                            valueField: 'Value',
                            store: B4.enums.TypePresence.getStore(),
                            labelWidth: 250,
                            width: 400,
                            itemId: 'cbHasEnergId',
                            margin: '0 5 10 0',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'combobox',
                            name: 'ConfirmWorkDocs',
                            fieldLabel: 'Документы по гос.кадастровому учету зем.участка',
                            displayField: 'Display',
                            valueField: 'Value',
                            store: B4.enums.TypePresence.getStore(),
                            labelWidth: 250,
                            width: 400,
                            itemId: 'cbHasWorkConfirmDoc',
                            margin: '0 5 0 0',
                            allowBlank: false,
                            editable: false
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'structelementgrid',
                            flex: 1
                        },
                        {
                            xtype: 'missingcommonestobjgrid',
                            flex: 1
                        },
                        {
                            xtype: 'rostructelhistorygrid',
                            flex: 1
                        }
                    ]
                }
            ],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [
                    {
                        xtype: 'buttongroup',
                        columns: 2,
                        itemId: 'structElpPanelGroupButton',
                        items: [
                            { xtype: 'b4savebutton' }
                        ]
                    }
                ]
            }]
        });

        me.callParent(arguments);
    }
});