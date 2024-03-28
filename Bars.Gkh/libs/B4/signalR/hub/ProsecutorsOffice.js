Ext.define('B4.signalR.hub.ProsecutorsOffice', {
    extend: 'B4.signalR.Hub',

    singleton: true,

    requires: [
        'B4.controller.dict.ProsecutorOffice',
    ],

    initHandlers: function (handlers) {
        handlers['RefreshGrid'] = function () {
            var controller = b4app.controllers.get('B4.controller.dict.ProsecutorOffice');

            if (!controller.mainViewIsClosed) {
                controller.reloadGrid();
            }
        };
    }
});
