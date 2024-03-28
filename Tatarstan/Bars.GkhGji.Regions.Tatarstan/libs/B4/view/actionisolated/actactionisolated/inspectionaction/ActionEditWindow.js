Ext.define('B4.view.actionisolated.actactionisolated.inspectionaction.ActionEditWindow', {
    extend: 'B4.view.actcheck.inspectionaction.ActionEditWindow',

    alias: 'widget.inspectionactactionisolatededitwindow',
    itemId: 'actActionIsolatedActionAddWindow',

    // Скрыть блок "Лица, присутствующие при..."
    personInfoFieldSetHidden: true,

    itemIdInnerMessage: 'ActActionIsolated'
});