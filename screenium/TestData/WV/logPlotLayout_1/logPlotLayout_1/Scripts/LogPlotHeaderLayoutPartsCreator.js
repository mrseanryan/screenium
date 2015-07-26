/** @constructor
* @description Create the divs for the parts, and also find them so they can be cached & populated.
* Means we can manage the part Ids in one class.
*/
var LogPlotHeaderLayoutPartsCreator = function (config) {
    if (!config) {
        throw 'bad args!';
    }

    this._config = config;
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.createDivHtmlForMetadata = function () {
    //TODO could use grid for the 2x2 layout
    return "<div class='logPlotMetaDataIndicators'>"+"<div>b</div><div>l</div>"+"</div>"
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.createDivHtmlForValueAxis = function () {
    return "<div class='logPlotValueAxis'>{value axis here!}</div>"
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.getMetaDataWidth = function () {
    //TODO ask config how many indicators we have (2x2 layout)
    return 26;
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.getHeightOfValueAxis = function () {
    return 25;
};
