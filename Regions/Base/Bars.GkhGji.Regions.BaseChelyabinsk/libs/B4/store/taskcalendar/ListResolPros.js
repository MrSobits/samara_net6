Ext.define('B4.store.taskcalendar.ListResolPros', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ResolPros'],
    autoLoad: false,
    model: 'B4.model.ResolPros',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar',
        listAction: 'GetListResolPros'
    }
});