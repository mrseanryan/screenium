/// <reference path="LogPlotHeaderLayoutStrategy.js" />
/// <reference path="LogPlotHeaderLayoutPartsCreator.js" />
/**
 * @constructor 
 * @description Create the layout for the header of 1 series.
 * Can be arranged to be suitable for Top or Bottom position.
 */
var LogPlotSeriesHeaderLayout = function (isAtTop, serieId, serieName, headerConfig, headerContainerDiv) {
    if (typeof isAtTop === "undefined" || typeof serieId === "undefined" ||
        typeof serieName === "undefined" || typeof headerConfig === "undefined" || !headerContainerDiv) {
        throw "bad args!";
    }

    this._isAtTop = isAtTop;
    this._serieId = serieId;
    this._serieName = serieName;
    this._config = headerConfig;

    this._partsCreator = new LogPlotHeaderLayoutPartsCreator(headerConfig, headerContainerDiv, serieId);
    var layoutFactory = new LogPlotHeaderLayoutStrategyFactory(headerConfig, isAtTop, this._partsCreator);
    this._layoutStrategy = layoutFactory.create();
};
/**
 * @function 
 */
LogPlotSeriesHeaderLayout.prototype.getParts = function () {
    return this._partsCreator;
};
/**
 * @function 
 */
LogPlotSeriesHeaderLayout.prototype.getInitialHtml = function () {
    //TODO refactor to take in a div, add the HTML, and then cache the divs for each part
    //TODO add accessors and use them to set the values etc.

    /*
    name
    value
    units
    metaDataIndicators
    valueAxis
    errorBar
    */

    var html = "";

    html += "<div id='" + this._partsCreator.getIdOfSerieDiv() + "' class='logPlotHeaderSerieCell'>";

    var strategyHtml = this._layoutStrategy.createLayoutHtml();

    html += strategyHtml;

    if (!strategyHtml) {
        throw 'invalid operation';
    }

    html += "</div>";

    return html;
};
/** @function
*/
LogPlotSeriesHeaderLayout.prototype.getInitialEmptyHtml = function () {
    var html = "";

    html += "<div class='logPlotHeaderSerieCell'>";
    //TODO refactor - extract CSS
    html += "<div class='pure-g pure-g-valign-fix'  style='height: 100%;   border: solid 1px blue;' >";
    html += "</div>";
    html += "</div>";

    return html;
};


LogPlotSeriesHeaderLayout.prototype._getHeightOfValueAxis = function () {
    return 37;
};

LogPlotSeriesHeaderLayout.prototype.onResizeLayout = function () {
    this._hidePartsToMatchHeight();

    this._sizeValueFontToMatchHeight();
};

LogPlotSeriesHeaderLayout.prototype._hidePartsToMatchHeight = function () {
    var minHeightToShowTwoRows = 66;
    //TODO if height < minHeightToShowTwoRows then hide: name, value, units
};

LogPlotSeriesHeaderLayout.prototype._sizeValueFontToMatchHeight = function () {
    var minWidthToShowValue = 130;

    //TODO if width < minWidthToShowValue then hide the value
    //TODO if width > minWidthToShowValue then show the value (if configured to show)

    //TODO implement me
    /*
    $.fn.shrinkText = function() {

    var $_me = this;
    var $_parent = $_me.parent();

    var int_my_width = $_me.width();
    var int_parent_width = $_parent.width();

    if ( int_my_width > int_parent_width ){

        rl_ratio =   int_parent_width / int_my_width;

        var int_my_fontSize = $_me.css("font-size").replace(/[^-\d\.]/g, '');

        int_my_fontSize = Math.floor(int_my_fontSize * rl_ratio);

        $_me.css("font-size", int_my_fontSize + "px");

    }
    };

    */
};
