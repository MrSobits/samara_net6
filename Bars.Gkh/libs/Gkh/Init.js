(function() {
    window.Gkh = window.Gkh || {};
    Ext.apply(window.Gkh, {
        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
        bgColor: '#DFE9F6',
        borderColor: '#99BCE8',

        Object: {
            toBool: function (v) {
                return Ext.data.Types.BOOL.convert(v);
            }
        }
    });
})();