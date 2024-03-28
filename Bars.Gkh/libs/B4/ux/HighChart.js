// usage:
// {
//   xtype: 'highchart',
//   highchartCfg: {
//     // ...
//   }
// }

Ext.define('Ext.ux.Highchart', {
    extend: 'Ext.Container',
    alias: 'widget.highchart',

    _chart: null,

    listeners: {
        boxready: function () {
            const cfg = this.initialConfig.highchartCfg;

            this._chart = Highcharts.chart(this.getId(), cfg);
        },

        resize: function () {
            this._chart.reflow();
        },

        beforedestroy: function () {
            this._chart.destroy();
        }
    },

    // Обновить указанную (или первую) точку указанного (или первого) ряда
    updatePoint: function (value, pointIndex, seriesIndex) {
        var me = this,
            series = me._chart.series[seriesIndex || 0],
            point = series.points[pointIndex || 0];

        value = value || {};

        point.update(value);
        point.onMouseOver();
    },
    
    updateSeries: function(data, dataMapFn, seriesIndex){
       var me = this,
           series = me._chart.series[seriesIndex || 0],
           mappedData = dataMapFn.call(this, data);

        series.setData(mappedData, true, true, false);
    },

    // Обновить точки указанного (или первого) ряда
    updatePoints: function(rawValues, pointCreateFn, seriesIndex) {
        var me = this,
            // Смещение всего спектра цветов (чтобы каждое обновление не было похоже на предыдущее)
            degreesShift = Math.random() * 360,
            // Соотношение цветовых сегментов (по два цвета на 90 градусов)
            segmentColorRatio = Math.floor(rawValues.length / 2) + 1,
            // Соотношение градусов всех точек
            // Если точек больше 5-ти, то точки распределяются по всем 360 градусам равномерно
            pointDegreesRatio = (segmentColorRatio > 3 ? 360 : segmentColorRatio * 90) / rawValues.length,
            series = me._chart.series[seriesIndex || 0];

        while (series.points.length > 0) {
            series.removePoint(0);
        }

        rawValues.forEach(function (value, index) {
            var point = pointCreateFn.call(this, value);

            if (!point.color) {
                // Вычисляем положение цвета точки со смещением
                var degrees = pointDegreesRatio * index + degreesShift;

                // Указываем цвет с корректировкой (при достижении лимита в 360 положение
                // определяется "продолжением" по окружности, т.е 0 + дельта превышения лимита)
                point.color = 'hsl(' + (degrees % 360 === 0 ? 360 : degrees - 360 * Math.floor(degrees / 360)) + ', 100%, 50%)';
            }

            series.addPoint(point);
        });

        series.redraw();
    },

    // Получить случайный светлый цвет
    getRandomLightColor: function () {
        return 'hsl(' + Math.random() * 360 + ', 100%, 50%)';
    }
});