angular.
    module('core').
    filter('numberCut', function () {
        return (input, sign) => {
            let signSymbol = "";
            switch (sign) {
                case "UAH": signSymbol = '\u20B4'; break
                default: signSymbol = "";
            }
            if ((input / 1000000) > 1) {
                return (input / 1000000).toFixed(2) + " млн." + signSymbol;
            } else if ((input / 1000) > 1) {
                return (input / 1000).toFixed(2) + " тис." + signSymbol;
            } else return input + signSymbol;
        };
    });