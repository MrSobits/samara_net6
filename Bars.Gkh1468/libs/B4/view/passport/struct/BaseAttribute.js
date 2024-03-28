Ext.define('B4.view.passport.struct.BaseAttribute', {
    extend: 'Ext.form.Panel',
    
    alias: 'widget.baseattr',

    bbar: {
        items: [
            {
                text: 'Добавить',
                iconCls: 'icon-add'
            }
        ]
    },

    initComponent: function () {
        var me = this;
        
        me.callParent(arguments);
    }
});