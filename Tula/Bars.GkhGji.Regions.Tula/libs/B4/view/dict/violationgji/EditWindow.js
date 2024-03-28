Ext.define('B4.view.dict.violationgji.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    height: 460,
    maxHeight: 460,
    width: 900,
    itemId: 'violationGjiEditWindow',
    title: 'Нарушение',
    closeAction: 'hide',

    border: false,
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.NormativeDoc',
        'B4.store.dict.NormativeDocItem',
        'B4.store.dict.NormativeDocItemGrouping',
        'B4.store.dict.ViolationFeatureGroupsGji',
        'B4.view.dict.violationgji.ViolationGroupsGjiGrid',
        'B4.view.dict.violationgji.ViolationNormativeDocItemGrid',
        'B4.store.dict.NormativeDocItemTreeStore'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 300
            },
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'panel',
                            layout: { type: 'vbox', align: 'stretch' },
                            border: false,
                            frame: true,
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 180
                            },
                            title: 'Нарушение',
                            items: [
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'Name',
                                    fieldLabel: 'Текст нарушения',
                                    allowBlank: false,
                                    maxLength: 2000
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'CodePin',
                                    fieldLabel: 'Код ПИН',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'PpRf170',
                                    fieldLabel: 'ПП РФ №170',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'PpRf25',
                                    fieldLabel: 'ПП РФ №25',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'PpRf307',
                                    fieldLabel: 'ПП РФ №307',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'PpRf491',
                                    fieldLabel: 'ПП РФ №491',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'OtherNormativeDocs',
                                    fieldLabel: 'Прочие норм. док.',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'GkRf',
                                    fieldLabel: 'ЖК РФ',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'violationNormativeDocItemGrid',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'violationGroupsGjiGrid',
                            flex: 1
                        },
                        {
                            xtype: 'violationActionsRemovGrid',
                            flex: 1
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