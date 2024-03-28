Ext.define('B4.store.taskcalendar.ListDisposals', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Disposal'],
    autoLoad: false,
    model: 'B4.model.Disposal',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar',
        listAction: 'GetListLicRequest'
    }
});