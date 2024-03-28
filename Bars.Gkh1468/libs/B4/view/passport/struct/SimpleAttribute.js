Ext.define('B4.view.passport.struct.SimpleAttribute', {
    extend: 'B4.view.passport.struct.BaseAttribute',

    alias: 'widget.simpleattr',

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            items: [
                {
                    fieldLabel: 'Наименование',  // annotation -> documentation
                    xtype: 'textfield'
                },
                {
                    fieldLabel: 'Паттерн', //  restriction ??? из справочника
                    xtype: 'textfield'  
                }
            ]
        });
        me.callParent(arguments);
    }
});