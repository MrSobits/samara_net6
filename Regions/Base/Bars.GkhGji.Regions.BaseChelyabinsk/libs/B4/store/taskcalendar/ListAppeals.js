Ext.define('B4.store.taskcalendar.ListAppeals', {
    extend: 'B4.base.Store',
    requires: ['B4.model.AppealCits'],
    autoLoad: false,
    model: 'B4.model.AppealCits',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar',
        listAction: 'GetListAppeals'
    }
});