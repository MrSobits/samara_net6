Ext.define('B4.view.actionisolated.actactionisolated.instrexamaction.ActionEditWindow', {
    extend: 'B4.view.actcheck.instrexamaction.ActionEditWindow',

    alias: 'widget.instrexamactactionisolatededitwindow',
    itemId: 'actActionIsolatedActionAddWindow',

    // Скрыть блок "Лица, присутствующие при..."
    personInfoFieldSetHidden: true,

    itemIdInnerMessage: 'ActActionIsolated'
});