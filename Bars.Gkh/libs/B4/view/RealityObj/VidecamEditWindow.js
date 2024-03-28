Ext.define('B4.view.realityobj.VidecamEditWindow', {
    extend: 'B4.form.Window',
    itemId: 'realityobjVidecamEditWindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 600,
    minWidth: 500,
    minHeight: 350,
    bodyPadding: 5,
    title: 'Камера видеонаблюдения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.enums.YesNoNotSet',       
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                
                {
                    xtype: 'container',
                    layout: 'hbox',
                    itemId: 'avail',
                    flex: 1,
                    margin: '10 0 0 0',
                    hidden: false,
                    defaults: {
                        labelWidth: 120,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'Workability',
                            flex: 1,
                            fieldLabel: 'Функционирует',
                            displayField: 'Display',
                            itemId: 'availchild',
                            store: B4.enums.YesNoNotSet.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        }
                    ]
                },                
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 120,
                        width: 370,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textarea',
                            name: 'InstallPlace',
                            allowBlank: false,
                            fieldLabel: 'Место установки',
                            flex: 1,
                            maxLength: 1500
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 120,
                        width: 370,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'UnicalNumber',
                            allowBlank: true,
                            fieldLabel: 'Идентификатор',
                            flex: 1,
                            maxLength: 250
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 120,
                        width: 370,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'VidecamAddress',
                            allowBlank: true,
                            fieldLabel: 'URL вебкамеры',
                            flex: 1,
                            maxLength: 250
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 120,
                        width: 370,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textarea',
                            name: 'TypeVidecam',
                            allowBlank: true,
                            fieldLabel: 'Описание',
                            flex: 1,
                            maxLength: 1500
                        }
                    ]
                },
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});