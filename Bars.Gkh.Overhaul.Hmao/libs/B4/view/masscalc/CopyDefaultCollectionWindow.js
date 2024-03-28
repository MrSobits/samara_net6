Ext.define('B4.view.masscalc.CopyDefaultCollectionWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.copydefaultcollectionwin',

    requires: ['B4.store.dict.municipality.ByParam'],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    height: 600,
    title: 'Копирование плановой собираемости',
    trackResetOnLoad: true,

    initComponent: function() {
        var me = this,
            storeMu = Ext.create('B4.store.dict.municipality.ByParam', {autoLoad: true}),
            selModel = Ext.create('Ext.selection.CheckboxModel', {
                mode: 'MULTI'
            });

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 170,
                flex: 1
            },
            items: [
                {
                    xtype: 'grid',
                    name: 'DestMunicipality',
                    store: storeMu,
                    selModel: selModel,
                    columns: [
                        {
                            dataIndex: 'Name',
                            flex: 1,
                            text: 'Наименование',
                            filter: {
                                xtype: 'textfield'
                            }
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
                            items: [
                                {
                                    xtype: 'button',
                                    action: 'Copy',
                                    iconCls: 'icon-page-copy',
                                    text: 'Копировать'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    action: 'Close',
                                    iconCls: 'icon-decline',
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