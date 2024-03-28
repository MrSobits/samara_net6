Ext.define('B4.store.taskcalendar.ListSOPR', {
    extend: 'B4.base.Store',
    requires: ['B4.model.appealcits.AppealOrder'],
    autoLoad: false,
    model: 'B4.model.appealcits.AppealOrder',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar',
        listAction: 'GetListSOPR'
    }
});