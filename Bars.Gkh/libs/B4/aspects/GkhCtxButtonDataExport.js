/*
Данный аспект предназначен для Экспорта грида в эксель
*/

Ext.define('B4.aspects.GkhCtxButtonDataExport', {
    extend: 'B4.aspects.ButtonDataExport',

    alias: 'widget.gkhctxbuttondataexportaspect',

    getGrid: function () {
        var me = this;
        return me.componentQuery(me.gridSelector);
    },
    btnAction: function() {
        var grid = this.getGrid();

        if (grid) {
            var store = grid.getStore();
            var columns = grid.columns;

            var headers = [];
            var dataIndexes = [];

            Ext.each(columns, function (res) {
                if (!res.hidden && res.header != "&#160;" && (res.dataIndex || res.dataExportAlias)) {
                    var dataIndex = res.dataIndex || res.dataExportAlias,
                        index = dataIndex.indexOf("."),
                        //Замена тега переноса пробелом
                        clearText = res.text.replaceAll('<br/>', ' ');
                    
                    headers.push(clearText);
                    dataIndexes.push(index >= 0 ? dataIndex.substring(0, index) : dataIndex);
                }
            });

            var params = {};

            if (headers.length > 0) {
                Ext.apply(params, { headers: headers, dataIndexes: dataIndexes });
            }

            if (store.sortInfo != null) {
                Ext.apply(params, {
                    sort: store.sortInfo.field,
                    dir: store.sortInfo.direction
                });
            }

            Ext.apply(params, store.lastOptions.params);

            if (this.usePost) {
                this.downloadViaPost(params);
            } else {
                this.downloadViaGet(params);
            }
        }
    },
});