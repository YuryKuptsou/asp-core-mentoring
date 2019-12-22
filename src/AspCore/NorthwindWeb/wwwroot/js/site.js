var uri_product = 'api/products';
var uri_category = 'api/categories';

$("document").ready(function () {

    function Product(data) {
        this.productName = data.productName;
        this.companyName = data.companyName;
        this.categoryName = data.categoryName;
        this.quantityPerUnit = data.quantityPerUnit;
        this.unitPrice = data.unitPrice;
        this.unitsInStock = data.unitsInStock;
        this.unitsOnOrder = data.unitsOnOrder;
        this.reorderLevel = data.reorderLevel;
        this.discontinued = data.discontinued;
        this.editPath = '@Url.Action("Update")' + '?id=' + data.productID;
    }

    function ProductsViewModel() {
        var self = this;

        self.products = ko.observableArray([]);

        $.getJSON(uri_product, function (allData) {
            var mappedProducts = $.map(allData, function (item) { return new Product(item); });
            self.products(mappedProducts);
        });
    }
    

    function Category(data) {
        this.categoryName = data.categoryName;
        this.imagePath = '@Url.Action("Image")' + '?id=' + data.categoryID;
        this.editPath = '@Url.Action("Update")' + '?id=' + data.categoryID;
    }

    function CategoryViewModel() {
        var self = this;

        self.categories = ko.observableArray([]);

        $.getJSON(uri_category, function (allData) {
            var mappedCategories = $.map(allData, function (item) { return new Category(item); });
            self.categories(mappedCategories);
        });
    }

    ko.applyBindings({ products: new ProductsViewModel(), categories: new CategoryViewModel() });
});