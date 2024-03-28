Ext.define('B4.store.taskcalendar.ListCourt', {
    extend: 'B4.base.Store',
    requires: ['B4.model.courtpractice.CourtPractice'],
    autoLoad: false,
    model: 'B4.model.courtpractice.CourtPractice',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar',
        listAction: 'GetListCourtPractice'
    }
});