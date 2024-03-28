Ext.define('B4.view.efficiencyrating.analitics.GraphPanel', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.ux.grid.Panel',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete'
    ],

    title: 'Графики',
    alias: 'widget.efanaliticsgraphpanel',

    layout: { type: 'vbox', align: 'stretch' },
    bodyStyle: Gkh.bodyStyle,
    closable: true,
    border: false,

    storeData: {},

    initComponent: function() {
        var me = this;

        Ext.applyIf(me,
        {
            items: [
                {
                    xtype: 'container',
                    style:
                        'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 5px; padding: 5px 10px; line-height: 16px;',
                    html:
                        '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Аналитические показатели рейтинга эффективности по управляющим организациям:</span>'
                },
                {
                    xtype: 'panel',
                    border: false,
                    flex: 1,
                    layout: { type: 'vbox', align: 'stretch' },
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        collapsible: true,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'efanaliticsgraphgrid',
                            title: 'Изменение значений показателей УО по годам',
                            name: 'CategoryRatingValue'
                        },
                        {
                            xtype: 'efanaliticsgraphgrid',
                            title: 'Изменение значений показателей УО по разделам',
                            name: 'CategoryFactorValue'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    getStore: function(key) {
        return this.down('b4grid[key=' + key + ']'.getStore());
    }
});