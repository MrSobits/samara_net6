Ext.define('B4.store.taskcalendar.ListAdmonition', {
    extend: 'B4.base.Store',
    requires: ['B4.model.appealcits.Admonition'],
    autoLoad: false,
    model: 'B4.model.appealcits.Admonition',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar',
        listAction: 'GetListAdmonitions'
    }
});