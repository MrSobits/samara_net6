Ext.define('B4.aspects.GridEditCtxWindow', {
    extend: 'B4.aspects.GridEditWindow',

    alias: 'widget.grideditctxwindowaspect',

    getForm: function () {
        var me = this,
            editWindow;

        if (me.editFormSelector) {
            editWindow = me.componentQuery(me.editFormSelector);

            if (editWindow && !editWindow.getBox().width) {
                editWindow = editWindow.destroy();
            }

            if (!editWindow) {

                editWindow = me.controller.getView(me.editWindowView).create(
                    {
                        constrain: true,
                        renderTo: B4.getBody().getActiveTab().getEl(),
                        closeAction: 'destroy',
                        ctxKey: me.controller.getCurrentContextKey()
                    });

                editWindow.show();
            }

            return editWindow;
        }
    }
});