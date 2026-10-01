// Einheitliche Diagramme (Chart.js) für die Dashboards
window.CoreCharts = (function () {
    "use strict";
    var ink = "#1B2A4A", marker = "#FFD84D", grid = "#E9EDF3", muted = "#5E6B82";
    if (window.Chart) {
        Chart.defaults.font.family = "Bricolage, system-ui, sans-serif";
        Chart.defaults.color = muted;
        Chart.defaults.plugins.legend.display = false;
        Chart.defaults.plugins.tooltip.backgroundColor = ink;
        Chart.defaults.plugins.tooltip.padding = 10;
        Chart.defaults.plugins.tooltip.cornerRadius = 8;
        Chart.defaults.maintainAspectRatio = false;
        Chart.defaults.locale = "de-DE";
    }
    function el(id) { return document.getElementById(id); }

    return {
        bar: function (id, labels, values, label, horizontal) {
            if (!el(id)) return;
            new Chart(el(id), {
                type: "bar",
                data: { labels: labels, datasets: [{ label: label, data: values, backgroundColor: values.map(function (_, i) { return i === 0 ? marker : ink; }), borderRadius: 6, maxBarThickness: 34 }] },
                options: {
                    indexAxis: horizontal ? "y" : "x",
                    scales: {
                        x: { grid: { display: !horizontal ? false : true, color: grid }, border: { display: false }, beginAtZero: true },
                        y: { grid: { display: horizontal ? false : true, color: grid }, border: { display: false }, beginAtZero: true, ticks: { precision: 0 } }
                    }
                }
            });
        },
        line: function (id, labels, values, label) {
            if (!el(id)) return;
            new Chart(el(id), {
                type: "line",
                data: { labels: labels, datasets: [{ label: label, data: values, borderColor: ink, backgroundColor: "rgba(255,216,77,.35)", fill: true, tension: .3, cubicInterpolationMode: "monotone", pointBackgroundColor: marker, pointBorderColor: ink, pointRadius: 5, pointHoverRadius: 7 }] },
                options: { scales: { x: { grid: { display: false }, border: { display: false } }, y: { beginAtZero: true, grid: { color: grid }, border: { display: false }, ticks: { precision: 0 } } } }
            });
        },
        doughnut: function (id, labels, values, colors) {
            if (!el(id)) return;
            new Chart(el(id), {
                type: "doughnut",
                data: { labels: labels, datasets: [{ data: values, backgroundColor: colors, borderWidth: 3, borderColor: "#fff" }] },
                options: { cutout: "62%", plugins: { legend: { display: true, position: "right", labels: { boxWidth: 12, boxHeight: 12, usePointStyle: true, padding: 14 } } } }
            });
        }
    };
})();
