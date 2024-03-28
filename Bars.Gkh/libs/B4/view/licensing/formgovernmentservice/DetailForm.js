Ext.define('B4.view.licensing.formgovernmentservice.DetailForm', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',

        'B4.ux.grid.toolbar.Paging',
        'B4.store.licensing.GovernmenServiceDetail'
    ],

    alias: 'widget.formgovernmentservicedetailform',
    closable: false,
    autoScroll: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 15,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me,
        {
            items: [], // будем динамически собирать форму
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
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'label',
                            name: 'ServiceDetailSectionType',
                            margin: '0 0 0 15px'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});