Ext.define('B4.signalR.hub.ReportStatus', {
    extend: 'B4.signalR.Hub',

    singleton: true,

    requires: [
        'B4.controller.al.ReportPanel',
    ],

    initHandlers: function (handlers) {
        handlers['UpdateStatus'] = function (reportId, status, fileId) {
            var controller = b4app.controllers.get('B4.controller.al.ReportPanel');

            if (controller) {
                controller.updateStatus(reportId, status, fileId);
            }
        };
    }
});
