# CatalogWebApi

In CatalogWebApi/appsetting.json
Change ConnectionStrings->DefaultConnection->Server:{Your Computer Name}

MS SQL needs to be up and running in the background

Then open project in your IDE and enter to your terminal:  
dotnet run --project CatalogWebApi  
dotnet run

Finally you can open swagger in your web browser  
https://localhost:7089/swagger/index.html

Invalid Register 1
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/account-controller-register1(invalid).png)
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/account-controller-register2(invalid).png)

Invalid Register 2
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/account-controller-register(existing-email).png)

Invalid Register 3
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/account-controller-register(existing-username).png)

Valid Register
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/account-controller-register1(valid).png)
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/account-controller-register2(valid).png)

Registration Email Service  
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/account-controller-register(email-service).png)

Salting
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/salting-proof-1.png)
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/salting-proof-2.png)

Invalid Login
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/token-controller-invalid-access.png)

3 Invalid Login
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/token-controller-3-invalid-tries.png)

Your Account Is Blocked (Email Service 1)
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/account-controller-account-is-blocked(email-service).png)

Valid Login
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/token-controller-valid-access.png)

Add New Category
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/category-controller-add-new-category.png)

Update Category
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/category-controller-update-category.png)

Delete Category
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/category-controller-delete-category.png)

Add New Product
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/product-controller-add-new-product.png)

Get By Product Id
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/product-controller-get-by-id.png)

Pagination

![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/product-controller-get-pagination.png)

Get Product By Category Id (cache)
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/product-controller-get-product-by-category-id(with%20cache).png)

My Products
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/product-controller-my-products.png)

Invalid Update Product
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/product-controller-update(invalid).png)

Valid Update Product
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/product-controller-update(valid).png)

Invalid Make Offer
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/offer-controller-make-offer(invalid).png)

Make Offer(Not Offerable)
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/offer-controller-make-offer(not%20offerable).png)

Valid Make Offer
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/offer-controller-make-offer(valid).png)

Invalid Cancel Offer
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/offer-controller-cancel-offer(invalid).png)

Valid Cancel Offer
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/offer-controller-cancel-offer(valid).png)

My Offers
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/offer-controller-my-offers.png)

My Products' Offer Status
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/offer-controller-my-products-offers.png)

Invalid Sell By Id
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/offer-controller-sell-by-id(invalid).png)

Valid Sell By Id
![](https://github.com/ozanpempegul/CatalogWebApi/blob/main/images/offer-controller-sell-by-id(valid).png)

