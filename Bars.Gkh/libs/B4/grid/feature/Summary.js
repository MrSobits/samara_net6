Ext.define('B4.grid.feature.Summary', {
    extend: 'Ext.grid.feature.Summary',

    alias: 'feature.b4_summary',

    remoteRoot: 'summaryData',
    
    generateSummaryData: function () {
        var me = this,
            store = me.view.store,
            reader = store.proxy.reader,
            columns = me.view.headerCt.getColumnsForTpl(),
            root,
            summaryRows,
            remoteData = {},
            length,
            comp,
            i, 
            data = {};
        
        if (me.remoteRoot && reader.rawData) {
            // reset reader root and rebuild extractors to extract summaries data
            root = reader.root;
            reader.root = me.remoteRoot;
            reader.buildExtractors(true);
            summaryRows = reader.getRoot(reader.rawData);
            
            // Ensure the Reader has a data conversion function to convert a raw data row into a Record data hash
            if (!reader.convertRecordData) {
                reader.buildExtractors();
            }
            //TODO разобраться с пропаданием строк после сохранения в инлайн гриде

            // Convert a raw data row into a Record's hash object using the Reader
            if (summaryRows) {
                reader.convertRecordData(remoteData, summaryRows);
                // restore initial reader configuration
                reader.root = root;
                reader.buildExtractors(true);
            }
            
            if (remoteData) {
                for (i = 0, length = columns.length; i < length; ++i) {
                    comp = Ext.getCmp(columns[i].id);
                    data[comp.id] = me.getSummary(comp.summaryType, comp.dataIndex, remoteData);
                }
            }
            
        }

        return data;
    },
    
    getSummary: function(type, dataIndex, remoteData) {
        if (type) {
            return remoteData[dataIndex];
        }
    },
    
    printSummaryRow: function (index) {
        var inner = this.view.getTableChunker().metaRowTpl.join(''),
            prefix = Ext.baseCSSPrefix;

        inner = inner.replace(prefix + 'grid-row', prefix + 'grid-row-summary x-grid-header-total');
        inner = inner.replace('{{id}}', '{gridSummaryValue}');
        inner = inner.replace(this.nestedIdRe, '{id$1}');
        inner = inner.replace('{[this.embedRowCls()]}', '{rowCls}');
        inner = inner.replace('{[this.embedRowAttr()]}', '{rowAttr}');
        inner = new Ext.XTemplate(inner, {
            firstOrLastCls: Ext.view.TableChunker.firstOrLastCls
        });

        return inner.applyTemplate({
            columns: this.getPrintData(index)
        });
    }
});