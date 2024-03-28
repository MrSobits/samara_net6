Ext.override(Ext.form.field.ComboBox, {
    onLoad: function() {
        var me = this,
            value = me.value;

        if (me.ignoreSelection > 0) {
            --me.ignoreSelection;
        }

        if (me.rawQuery) {
            me.rawQuery = false;
            me.syncSelection();
            if (me.picker && !me.picker.getSelectionModel().hasSelection()) {
                me.doAutoSelect();
            }
        }

        else {

            if (me.value || me.value === 0) {
                if (me.pageSize === 0) { // Не выполнять при смене страницы
                    me.setValue(me.value);
                }
            } else {


                if (me.store.getCount()) {
                    me.doAutoSelect();
                } else {

                    me.setValue(me.value);
                }
            }
        }
    }
});