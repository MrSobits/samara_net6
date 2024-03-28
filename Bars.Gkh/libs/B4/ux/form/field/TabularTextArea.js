Ext.define('B4.ux.form.field.TabularTextArea', {
    extend: 'Ext.form.field.TextArea',

    alias: 'widget.tabtextarea',

    initComponent: function () {
        var me = this;
        me.callParent(arguments);
    },

    onRender: function() {
        var me = this;
        me.callParent(arguments);
        me.getActionEl().dom.onkeydown = function(e) {
            var input = this;
            if (e.keyCode === 9) { // tab was pressed

                // get caret position/selection
                var val = input.value,
                    start = input.selectionStart,
                    end = input.selectionEnd;

                // set textarea value to: text before caret + tab + text after caret
                input.value = val.substring(0, start) + '\t' + val.substring(end);

                // put caret at right position again
                input.selectionStart = input.selectionEnd = start + 1;

                // prevent the focus lose
                return false;
            }
        };
    }
});