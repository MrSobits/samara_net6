Ext.define('B4.controller.documentsgjiregister.TatDisposal', {
    extend: 'B4.controller.documentsgjiregister.Disposal',

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            if (this.params.typeDocumentGji) {
                operation.params.typeDocumentGji = this.params.typeDocumentGji;
            }
        }

        this.callParent(arguments);
    }
});