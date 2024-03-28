Ext.define('B4.view.dict.municipality.FiasLoader', {
    extend: 'B4.form.Window',
    alias: 'widget.municipalityFiasLoadWindow',
    requires: ['B4.view.Control.GkhTriggerField'],
    modal: true,
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    title: 'Добавление МО из ФИАС',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
               {
                   xtype: 'gkhtriggerfield',
                   name: 'Municipalities',
                   itemId: 'tfMunicipalityFias',
                   fieldLabel: 'Муниципальные образования',
                   emptyText: 'Выбор МО'
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
                                    xtype: 'b4savebutton',
                                    text: 'Загрузить МО из ФИАС'
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