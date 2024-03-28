Ext.override(Ext.Window, {
    constrain: null,
    initComponent: function() {
        var me = this,
            el = me.constrainTo || me.renderTo,
            width,
            height;

        try {
            if (!el) {
                el = B4.getBody().getActiveTab().getEl() || Ext.getBody();
            }

        } catch (e) {

        }

        if (el) {
            width = el.getWidth();
            height = el.getHeight();

            if (!me.maxWidth || me.maxWidth > width) {
                me.maxWidth = width;
            }

            if (!me.maxHeight || me.maxHeight > height) {
                me.maxHeight = height;
            }

            if (me.constrain !== false) {
                me.constrain = true;
                if (!me.renderTo) {
                    me.renderTo = el;
                }

                if (!me.constrainTo) {
                    me.constrainTo = el;
                }
            }
            
        } else {
            me.constrain = false;
        }

        me.callParent();
    }
});