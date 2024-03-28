Ext.define('B4.view.service.GroupActionWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 800,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Действия',
    itemId: 'groupActionWindow',
    layout: 'fit'
});
