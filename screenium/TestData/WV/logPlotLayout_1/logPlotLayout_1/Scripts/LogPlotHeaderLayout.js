/// <reference path="LogPlotSeriesHeaderLayout.js" />
/* create layout for contents of header row
*/
var LogPlotHeaderLayout = function () {
    //TODO use a config class!
    this._headerConfig = {
        isValueShowing: true
    };
};
/**
 * @function
 */
LogPlotHeaderLayout.prototype._getHeaderLayoutPropertyName = function (isAtTop) {
    return isAtTop ? "headerLayoutAtTop" : "headerLayoutAtBottom";
};
/**
 * @function
 */
LogPlotHeaderLayout.prototype._getHtmlForColumn = function (seriesInColumn, isAtTop) {
    var html = "";
    var htmlTableStart = "<table style='width:100%; height:100%; padding-bottom: 1px;'>";
    var htmlRowStart = "<tr><td style='height:33%;'>"; //xxx calc height% ?
    var htmlRowEnd = "</td></tr>";
    var htmlTableEnd = "</table>";

    html += htmlTableStart;
    for (var serie in seriesInColumn) {
        if (seriesInColumn.hasOwnProperty(serie)) {
            serie = seriesInColumn[serie];

            var headerLayoutProp = this._getHeaderLayoutPropertyName(isAtTop);

            serie[headerLayoutProp] = new LogPlotSeriesHeaderLayout(isAtTop, serie.Id, serie.Name, this._headerConfig.isValueShowing);

            html += htmlRowStart;
            html += serie.headerLayoutAtTop.getInitialHtml();
            html += htmlRowEnd;
        }
    }

    html += htmlTableEnd;

    return html;
};
/**
 * @function 
 */
LogPlotHeaderLayout.prototype.getHtmlForColumnHeader = function (columnId, seriesInColumn) {
    return this._getHtmlForColumn(seriesInColumn, true);
};

/**
 * @function 
 */
LogPlotHeaderLayout.prototype.getHtmlForColumnFooter = function (columnId, seriesInColumn) {
    return this._getHtmlForColumn(seriesInColumn, false);
};

/**
 * @function 
 */
LogPlotHeaderLayout.prototype.onResizeLayout = function(seriesInColumn) {
    //tell each of the series header layouts, to resize:

    var headerAtTop = this._getHeaderLayoutPropertyName(true);
    var headerAtBottom = this._getHeaderLayoutPropertyName(false);

    for (var serie in seriesInColumn) {
        if (seriesInColumn.hasOwnProperty(serie)) {
            serie = seriesInColumn[serie];

            serie[headerAtTop].onResizeLayout();
            serie[headerAtBottom].onResizeLayout();
        }
    }
};
